using System;
using System.Collections.Generic;
using System.Text;
using Orm.CustomAttributes;
using Orm.DBUtility;
using System.Collections;
using System.Reflection;
using System.Data;

namespace Orm.Common
{
    public class DbEntityUtils
    {
        public static string GetTableName(Type classType)
        {
            string strTableName = string.Empty;
            string strEntityName = string.Empty;

            strEntityName = classType.FullName;

            object classAttr = classType.GetCustomAttributes(false)[0];
            if (classAttr is TableAttribute)
            {
                TableAttribute tableAttr = classAttr as TableAttribute;
                strTableName = tableAttr.Name;
            }
            if (string.IsNullOrEmpty(strTableName))
            {
                throw new Exception("实体类:" + strEntityName + "的属性配置[Table(name=\"tablename\")]错误或未配置");
            }

            return strTableName;
        }

        public static string GetPrimaryKey(object attribute, DbOperateType type)
        {
            string strPrimary = string.Empty;
            IdAttribute attr = attribute as IdAttribute;
            if (type == DbOperateType.INSERT)
            {
                switch (attr.Strategy)
                {
                    case GenerationType.INDENTITY:
                        break;
                    case GenerationType.SEQUENCE:
                        strPrimary = System.Guid.NewGuid().ToString();
                        break;
                    case GenerationType.TABLE:
                        break;
                }
            }
            else {
                strPrimary = attr.Name;
            }

            return strPrimary;
        }

        public static string GetColumnName(object attribute)
        {
            string columnName = string.Empty;
            if (attribute is ColumnAttribute)
            {
                ColumnAttribute columnAttr = attribute as ColumnAttribute;
                columnName = columnAttr.Name;
            }
            if (attribute is IdAttribute)
            {
                IdAttribute idAttr = attribute as IdAttribute;
                columnName = idAttr.Name;
            }

            return columnName;
        }

        public static TableInfo GetTableInfo(object entity, DbOperateType dbOpType)
        {
            bool breakForeach = false;
            string strPrimaryKey = string.Empty;
            TableInfo tableInfo = new TableInfo();
            Type type = entity.GetType();

            tableInfo.TableName = GetTableName(type);
            if (dbOpType == DbOperateType.COUNT)
            {
                return tableInfo;
            }

            PropertyInfo[] properties = ReflectionUtils.GetProperties(type);
            foreach (PropertyInfo property in properties)
            {
                object propvalue = null;
                string columnName = string.Empty;
                string propName = columnName = property.Name;
          
                propvalue = ReflectionUtils.GetPropertyValue(entity, property);

                object[] propertyAttrs = property.GetCustomAttributes(false);
                for (int i = 0; i < propertyAttrs.Length; i++)
                {
                    object propertyAttr = propertyAttrs[i];
                    if (DbEntityUtils.IsCaseColumn(propertyAttr, dbOpType))
                    {
                        breakForeach = true;break;
                    }

                    string tempVal = GetColumnName(propertyAttr);
                    columnName = tempVal == string.Empty ? propName : tempVal;

                    if (propertyAttr is IdAttribute)
                    {
                        if (dbOpType == DbOperateType.INSERT || dbOpType == DbOperateType.DELETE)
                        {
                            IdAttribute idAttr = propertyAttr as IdAttribute;
                            tableInfo.Strategy = idAttr.Strategy;

                            if (CommonUtils.IsNullOrEmpty(propvalue))
                            {
                                strPrimaryKey = DbEntityUtils.GetPrimaryKey(propertyAttr, dbOpType);
                                if (!string.IsNullOrEmpty(strPrimaryKey))
                                    propvalue = strPrimaryKey;
                            }
                        }

                        tableInfo.Id.Key = columnName;
                        tableInfo.Id.Value = propvalue;
                        tableInfo.PropToColumn.Put(propName, columnName);
                        breakForeach = true;
                    }
                }
                if (breakForeach && dbOpType == DbOperateType.DELETE) break;
                if (breakForeach) { breakForeach = false; continue; }
                tableInfo.Columns.Put(columnName, propvalue);
                tableInfo.PropToColumn.Put(propName, columnName);
            }

            return tableInfo;
        }

        public static PropertyInfo GetPrimaryKeyPropertyInfo(object entity)
        {
            bool breakForeach = false;
            Type type = entity.GetType();
            PropertyInfo properyInfo = null;
        
            PropertyInfo[] properties = ReflectionUtils.GetProperties(type);
            foreach (PropertyInfo property in properties)
            {
                string columnName = string.Empty;
                string propName = columnName = property.Name;

                object[] propertyAttrs = property.GetCustomAttributes(false);
                for (int i = 0; i < propertyAttrs.Length; i++)
                {
                    object propertyAttr = propertyAttrs[i];

                    if (propertyAttr is IdAttribute)
                    {
                        properyInfo = property;
                        breakForeach = true;
                        break;
                    }
                }
                if (breakForeach) break;
            }

            return properyInfo;
        }

        public static string GetFindAllSql(TableInfo tableInfo)
        {
            StringBuilder sbColumns = new StringBuilder();

            tableInfo.Columns.Put(tableInfo.Id.Key, tableInfo.Id.Value);
            foreach (String key in tableInfo.Columns.Keys)
            {
                sbColumns.Append(key).Append(",");
            }

            if (sbColumns.Length > 0) sbColumns.Remove(sbColumns.ToString().Length - 1, 1);

            string strSql = "SELECT {0} FROM {1}";
            strSql = string.Format(strSql, sbColumns.ToString(), tableInfo.TableName);

            return strSql;
        }

        public static string GetFindByIdSql(TableInfo tableInfo)
        {
            StringBuilder sbColumns = new StringBuilder();

            if (tableInfo.Columns.ContainsKey(tableInfo.Id.Key))
                tableInfo.Columns[tableInfo.Id.Key] = tableInfo.Id.Value;
            else
                tableInfo.Columns.Put(tableInfo.Id.Key, tableInfo.Id.Value);

            foreach (String key in tableInfo.Columns.Keys)
            {
                sbColumns.Append(key).Append(",");
            }

            if (sbColumns.Length > 0) sbColumns.Remove(sbColumns.ToString().Length - 1, 1);

            string strSql = "SELECT {0} FROM {1} WHERE {2} = " + AdoHelper.DbParmChar + "{2}";
            strSql = string.Format(strSql, sbColumns.ToString(), tableInfo.TableName, tableInfo.Id.Key);

            return strSql;
        }

        public static string GetFindCountSql(TableInfo tableInfo)
        {
            StringBuilder sbColumns = new StringBuilder();

            string strSql = "SELECT COUNT(0) FROM {1} ";
            strSql = string.Format(strSql, sbColumns.ToString(), tableInfo.TableName);

            foreach (String key in tableInfo.Columns.Keys)
            {
                sbColumns.Append(key).Append("=").Append(AdoHelper.DbParmChar).Append(key);
            }

            if (sbColumns.Length > 0)
            {
                strSql += " WHERE " + sbColumns.ToString();
            }

            return strSql;
        }

        public static string GetFindByPropertySql(TableInfo tableInfo)
        {
            StringBuilder sbColumns = new StringBuilder();

            tableInfo.Columns.Put(tableInfo.Id.Key, tableInfo.Id.Value);
            foreach (String key in tableInfo.Columns.Keys)
            {
                sbColumns.Append(key).Append(",");
            }

            if (sbColumns.Length > 0) sbColumns.Remove(sbColumns.ToString().Length - 1, 1);

            string strSql = "SELECT {0} FROM {1} WHERE {2} = " + AdoHelper.DbParmChar + "{2}";
            strSql = string.Format(strSql, sbColumns.ToString(), tableInfo.TableName, tableInfo.Id.Key);

            return strSql;
        }

        public static string GetAutoSql()
        {
            string autoSQL = "";
            if (AdoHelper.DbType == DatabaseType.SQLSERVER)
            {
                autoSQL = " select scope_identity() as AutoId ";
            }

            return autoSQL;
        }

        public static string GetInsertSql(TableInfo tableInfo)
        {
            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();

            if(tableInfo.Strategy != GenerationType.INDENTITY)
                tableInfo.Columns.Put(tableInfo.Id.Key, tableInfo.Id.Value);
            
            foreach (String key in tableInfo.Columns.Keys)
            {
                if (!string.IsNullOrEmpty(key.Trim()))
                {
                    Object value = tableInfo.Columns[key];
                    sbColumns.Append(key).Append(",");
                    sbValues.Append(AdoHelper.DbParmChar).Append(key).Append(",");
                }
            }

            if (sbColumns.Length > 0 && sbValues.Length > 0)
            {
                sbColumns.Remove(sbColumns.ToString().Length - 1, 1);
                sbValues.Remove(sbValues.ToString().Length - 1, 1);
            }

            string strSql = "INSERT INTO {0}({1}) VALUES({2})";
            strSql = string.Format(strSql, tableInfo.TableName, sbColumns.ToString(), sbValues.ToString());

            return strSql;
        }

        public static string GetUpdateSql(TableInfo tableInfo)
        {
            StringBuilder sbBody = new StringBuilder();

            foreach (String key in tableInfo.Columns.Keys)
            {
                Object value = tableInfo.Columns[key];

                sbBody.Append(key).Append("=").Append(AdoHelper.DbParmChar + key).Append(",");
            }

            if (sbBody.Length > 0) sbBody.Remove(sbBody.ToString().Length - 1, 1);

            tableInfo.Columns.Put(tableInfo.Id.Key, tableInfo.Id.Value);

            string strSql = "update {0} set {1} where {2} =" + AdoHelper.DbParmChar + tableInfo.Id.Key;
            strSql = string.Format(strSql, tableInfo.TableName, sbBody.ToString(), tableInfo.Id.Key);

            return strSql;
        }

        public static string GetDeleteByIdSql(TableInfo tableInfo)
        {
            string strSql = "delete from {0} where {1} =" + AdoHelper.DbParmChar + tableInfo.Id.Key;
            strSql = string.Format(strSql, tableInfo.TableName, tableInfo.Id.Key);

            return strSql;
        }

        public static void SetParameters(ColumnInfo columns, params IDbDataParameter[] parms)
        {
            int i = 0;
            foreach (string key in columns.Keys)
            {
                if (!string.IsNullOrEmpty(key.Trim()))
                {
                    object value = columns[key];
                    if (value == null) value = DBNull.Value;
                    parms[i].ParameterName = key;
                    parms[i].Value = value;
                    i++;
                }
            }
        }

        public static bool IsCaseColumn(object attribute, DbOperateType dbOperateType)
        {
            if (attribute is ColumnAttribute)
            {
                ColumnAttribute columnAttr = attribute as ColumnAttribute;
                if (columnAttr.Ignore)
                {
                    return true;
                }
                if (!columnAttr.IsInsert && dbOperateType == DbOperateType.INSERT)
                {
                    return true;
                }
                if (!columnAttr.IsUpdate && dbOperateType == DbOperateType.UPDATE)
                {
                    return true;
                } 
            }

            return false;
        }
    }
}
