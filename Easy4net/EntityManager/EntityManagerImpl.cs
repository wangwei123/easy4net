using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Easy4net.CustomAttributes;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Linq;
using Easy4net.DBUtility;
using Easy4net.Common;
using System.Text.RegularExpressions;

namespace Easy4net.EntityManager
{
    public class EntityManagerImpl : EntityManager
    {
        IDbTransaction transaction = null;

        #region 将实体数据保存到数据库
        public int Save<T>(T entity) 
        {
            object val = 0;
            try
            {
                TableInfo tableInfo = EntityHelper.GetTableInfo(entity, DbOperateType.INSERT);

                String strSql = EntityHelper.GetInsertSql(tableInfo);
                strSql += EntityHelper.GetAutoSql();

                IDbDataParameter[] parms = tableInfo.GetParameters();
                
                if (transaction != null) 
                    val = AdoHelper.ExecuteScalar(transaction, CommandType.Text, strSql, parms);
                else
                    val = AdoHelper.ExecuteScalar(AdoHelper.ConnectionString, CommandType.Text, strSql, parms);

                if (Convert.ToInt32(val) > 0 && (AdoHelper.DbType == DatabaseType.MYSQL || AdoHelper.DbType == DatabaseType.SQLSERVER))
                {
                    PropertyInfo propertyInfo = EntityHelper.GetPrimaryKeyPropertyInfo(entity);
                    ReflectionHelper.SetPropertyValue(entity, propertyInfo, val);
                }
            }
            catch (Exception e) 
            {
                throw e;
            }

            return Convert.ToInt32(val);
        }
        #endregion

        #region 将实体数据修改到数据库
        public int Update<T>(T entity)
        {
            object val = 0;
            try
            {
                TableInfo tableInfo = EntityHelper.GetTableInfo(entity, DbOperateType.UPDATE);

                String strSql = EntityHelper.GetUpdateSql(tableInfo);

                IDbDataParameter[] parms = tableInfo.GetParameters();

                if (transaction != null) 
                    val = AdoHelper.ExecuteNonQuery(transaction, CommandType.Text, strSql, parms);
                else
                    val = AdoHelper.ExecuteNonQuery(AdoHelper.ConnectionString, CommandType.Text, strSql, parms);
            }
            catch (Exception e)
            {
                throw e;
            }

            return Convert.ToInt32(val);
        }
        #endregion

        #region 删除实体对应数据库中的数据
        public int Remove<T>(T entity)
        {
            object val = 0;
            try
            {
                TableInfo tableInfo = EntityHelper.GetTableInfo(entity, DbOperateType.DELETE);

                String strSql = EntityHelper.GetDeleteByIdSql(tableInfo);

                IDbDataParameter[] parms = DbFactory.CreateDbParameters(1);
                parms[0].ParameterName = tableInfo.Id.Key;
                parms[0].Value = tableInfo.Id.Value;

                if (transaction != null)
                    val = AdoHelper.ExecuteNonQuery(transaction, CommandType.Text, strSql, parms);
                else
                    val = AdoHelper.ExecuteNonQuery(AdoHelper.ConnectionString, CommandType.Text, strSql, parms);
            }
            catch (Exception e)
            {
                throw e;
            }

            return Convert.ToInt32(val);
        }
        #endregion

        #region 根据主键id删除实体对应数据库中的数据
        public int Remove<T>(object id) where T : new()
        {
            object val = 0;
            try
            {
                TableInfo tableInfo = EntityHelper.GetTableInfo(new T(), DbOperateType.DELETE);

                String strSql = EntityHelper.GetDeleteByIdSql(tableInfo);

                IDbDataParameter[] parms = DbFactory.CreateDbParameters(1);
                parms[0].ParameterName = tableInfo.Id.Key;
                parms[0].Value = id;

                if (transaction != null)
                    val = AdoHelper.ExecuteNonQuery(transaction, CommandType.Text, strSql, parms);
                else
                    val = AdoHelper.ExecuteNonQuery(AdoHelper.ConnectionString, CommandType.Text, strSql, parms);
            }
            catch (Exception e)
            {
                throw e;
            }

            return Convert.ToInt32(val);
        }
        #endregion

        #region 查询实体类对应的表中所有的记录数
        public int FindCount<T>() where T : new()
        {
            int count = 0;
            try
            {
                PropertyInfo[] properties = ReflectionHelper.GetProperties(new T().GetType());
                TableInfo tableInfo = EntityHelper.GetTableInfo(new T(), DbOperateType.COUNT);
                string strSql = EntityHelper.GetFindCountSql(tableInfo);

                count = Convert.ToInt32(AdoHelper.ExecuteScalar(AdoHelper.ConnectionString, CommandType.Text, strSql));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return count;
        }
        #endregion

        #region 根据查询条件查询实体类对应的表中的记录数
        public int FindCount<T>(DbCondition condition) where T : new()
        {
            int count = 0;
            try
            {
                PropertyInfo[] properties = ReflectionHelper.GetProperties(new T().GetType());
                TableInfo tableInfo = EntityHelper.GetTableInfo(new T(), DbOperateType.COUNT);
                tableInfo.Columns = condition.Columns;

                string strSql = EntityHelper.GetFindCountSql(tableInfo, condition);

                count = Convert.ToInt32(AdoHelper.ExecuteScalar(AdoHelper.ConnectionString, CommandType.Text, strSql, tableInfo.GetParameters()));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return count;
        }
        #endregion 

        #region 根据一个查询条件查询实体类对应的表中所有的记录数
        public int FindCount<T>(string propertyName, object propertyValue) where T : new()
        {
            int count = 0;
            try
            {
                PropertyInfo[] properties = ReflectionHelper.GetProperties(new T().GetType());
                TableInfo tableInfo = EntityHelper.GetTableInfo(new T(), DbOperateType.COUNT);

                string strSql = EntityHelper.GetFindCountSql(tableInfo);
                strSql += string.Format(" WHERE {0} = @{1}", propertyName, propertyName);

                ColumnInfo columnInfo = new ColumnInfo();
                columnInfo.Add(propertyName, propertyValue);
                IDbDataParameter[] parameters = DbFactory.CreateDbParameters(1);
                EntityHelper.SetParameters(columnInfo, parameters);

                count = Convert.ToInt32(AdoHelper.ExecuteScalar(AdoHelper.ConnectionString, CommandType.Text, strSql, parameters));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return count;
        }
        #endregion

        #region 查询实体对应表的所有数据
        public List<T> FindAll<T>() where T : new()
        {
            IDataReader sdr = null;
            List<T> listArr = new List<T>();
            try
            {
                PropertyInfo[] properties = ReflectionHelper.GetProperties(new T().GetType());

                TableInfo tableInfo = EntityHelper.GetTableInfo(new T(), DbOperateType.SELECT);
                String strSql = EntityHelper.GetFindAllSql(tableInfo).ToUpper();

                sdr = AdoHelper.ExecuteReader(AdoHelper.ConnectionString, CommandType.Text, strSql);
                while (sdr.Read())
                {
                    T entity = new T();
                    foreach (PropertyInfo property in properties)
                    {
                        if (EntityHelper.IsCaseColumn(property, DbOperateType.SELECT)) continue;

                        String name = tableInfo.PropToColumn[property.Name].ToString();

                        String columns = strSql.Substring(0, strSql.IndexOf("FROM"));
                        if (columns.Contains(name.ToUpper()) || columns.Contains("*") || columns.Contains("top"))
                        {
                            ReflectionHelper.SetPropertyValue(entity, property, sdr[name]);
                        }
                    }

                    listArr.Add(entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sdr != null) sdr.Close();
            }

            return listArr;
        }
        #endregion

        #region 通过自定义条件查询数据
        public List<T> Find<T>(DbCondition condition) where T : new()
        {
            List<T> listArr = new List<T>();
            IDataReader sdr = null;
            try
            {
                PropertyInfo[] properties = ReflectionHelper.GetProperties(new T().GetType());
                TableInfo tableInfo = EntityHelper.GetTableInfo(new T(), DbOperateType.SELECT);

                String strSql = EntityHelper.GetFindSql(tableInfo, condition);

                tableInfo.Columns = condition.Columns;

                IDbDataParameter[] parameters = tableInfo.GetParameters();

                sdr = AdoHelper.ExecuteReader(AdoHelper.ConnectionString, CommandType.Text, strSql, parameters);
                while (sdr.Read())
                {
                    T entity = new T();
                    foreach (PropertyInfo property in properties)
                    {
                        if (EntityHelper.IsCaseColumn(property, DbOperateType.SELECT)) continue;

                        String name = tableInfo.PropToColumn[property.Name].ToString();
                        ReflectionHelper.SetPropertyValue(entity, property, sdr[name]);
                    }

                    listArr.Add(entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sdr != null) sdr.Close();
            }

            return listArr;
        }
        #endregion

        #region 通过自定义SQL语句查询数据
        public List<T> FindBySql<T>(string strSql) where T : new()
        {
            List<T> listArr = new List<T>();
            IDataReader sdr = null;
            try
            {
                strSql = strSql.ToUpper();
                PropertyInfo[] properties = ReflectionHelper.GetProperties(new T().GetType());
                TableInfo tableInfo = EntityHelper.GetTableInfo(new T(), DbOperateType.SELECT);

                String columns = strSql.Substring(0, strSql.IndexOf("FROM"));

                sdr = AdoHelper.ExecuteReader(AdoHelper.ConnectionString, CommandType.Text, strSql);
                while (sdr.Read())
                {
                    T entity = new T();
                    foreach (PropertyInfo property in properties)
                    {
                        if (EntityHelper.IsCaseColumn(property, DbOperateType.SELECT)) continue;

                        String name = tableInfo.PropToColumn[property.Name].ToString();
                        if (columns.Contains(name.ToUpper()) || columns.Contains("*"))
                        {
                            ReflectionHelper.SetPropertyValue(entity, property, sdr[name]);
                        }
                    }

                    listArr.Add(entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sdr != null) sdr.Close();
            }

            return listArr;
        }
        #endregion

        #region 通过自定义SQL语句查询数据
        public List<T> FindBySql<T>(string strSql, IDbDataParameter[] parameters) where T : new()
        {
            List<T> listArr = new List<T>();
            IDataReader sdr = null;
            try
            {
                strSql = strSql.ToUpper();
                PropertyInfo[] properties = ReflectionHelper.GetProperties(new T().GetType());
                TableInfo tableInfo = EntityHelper.GetTableInfo(new T(), DbOperateType.SELECT);

                String columns = strSql.Substring(0, strSql.IndexOf("FROM"));

                sdr = AdoHelper.ExecuteReader(AdoHelper.ConnectionString, CommandType.Text, strSql, parameters);
                while (sdr.Read())
                {
                    T entity = new T();
                    foreach (PropertyInfo property in properties)
                    {
                        if (EntityHelper.IsCaseColumn(property, DbOperateType.SELECT)) continue;
                       
                        String name = tableInfo.PropToColumn[property.Name].ToString();
                        if (columns.Contains(name.ToUpper()) || columns.Contains("*"))
                        {
                            ReflectionHelper.SetPropertyValue(entity, property, sdr[name]);
                        }
                    }

                    listArr.Add(entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sdr != null) sdr.Close();
            }

            return listArr;
        }
        #endregion

        #region 根据一个查询条件查询数据
        public List<T> FindByProperty<T>(string propertyName, object propertyValue) where T : new()
        {
            List<T> listArr = new List<T>();
            IDataReader sdr = null;
            try
            {
                PropertyInfo[] properties = ReflectionHelper.GetProperties(new T().GetType());
                TableInfo tableInfo = EntityHelper.GetTableInfo(new T(), DbOperateType.SELECT);

                String strSql = EntityHelper.GetFindAllSql(tableInfo);
                strSql += string.Format(" WHERE {0} = @{1}", propertyName, propertyName);
                strSql = strSql.ToUpper();

                String columns = strSql.Substring(0, strSql.IndexOf("FROM"));

                ColumnInfo columnInfo = new ColumnInfo();
                columnInfo.Add(propertyName, propertyValue);
                IDbDataParameter[] parameters = DbFactory.CreateDbParameters(1);
                EntityHelper.SetParameters(columnInfo, parameters);

                sdr = AdoHelper.ExecuteReader(AdoHelper.ConnectionString, CommandType.Text, strSql, parameters);
                while (sdr.Read())
                {
                    T entity = new T();
                    foreach (PropertyInfo property in properties)
                    {
                        if (EntityHelper.IsCaseColumn(property, DbOperateType.SELECT)) continue;

                        String name = tableInfo.PropToColumn[property.Name].ToString();
                        if (columns.Contains(name.ToUpper()) || columns.Contains("*"))
                        {
                            ReflectionHelper.SetPropertyValue(entity, property, sdr[name]);
                        }
                    }

                    listArr.Add(entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sdr != null) sdr.Close();
            }

            return listArr;
        }
        #endregion

        #region 通过主键ID查询数据
        public T FindById<T>(object id) where T : new()
        {
            List<T> list = new List<T>();
           
            IDataReader sdr = null;
            try
            {
                PropertyInfo[] properties = ReflectionHelper.GetProperties(new T().GetType());

                TableInfo tableInfo = EntityHelper.GetTableInfo(new T(), DbOperateType.SELECT);

                String strSql = EntityHelper.GetFindByIdSql(tableInfo);

                IDbDataParameter[] parms = DbFactory.CreateDbParameters(1);
                parms[0].ParameterName = tableInfo.Id.Key;
                parms[0].Value = id;

                sdr = AdoHelper.ExecuteReader(AdoHelper.ConnectionString, CommandType.Text, strSql, parms);
                while (sdr.Read())
                {
                    T entity = new T();
                    foreach (PropertyInfo property in properties)
                    {
                        if (EntityHelper.IsCaseColumn(property, DbOperateType.SELECT)) continue;

                        String name = tableInfo.PropToColumn[property.Name].ToString();
                        ReflectionHelper.SetPropertyValue(entity, property, sdr[name]);
                    }
                    list.Add(entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sdr != null) sdr.Close();
            }

            return list.FirstOrDefault();
        }
        #endregion        

        #region Transaction 注入事物对象属性
        public IDbTransaction Transaction
        {
            get
            {
                return transaction;
            }
            set
            {
                transaction = value;
            }
        }
        #endregion
    }
}
