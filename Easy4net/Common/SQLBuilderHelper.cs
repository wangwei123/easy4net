using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easy4net.DBUtility;

namespace Easy4net.Common
{
    public class SQLBuilderHelper
    {
        private static string mssqlPageTemplate = "select {0} from (select ROW_NUMBER() OVER(order by {1}) AS RowNumber, {2}) as tmp_tbl where RowNumber BETWEEN @pageStart and @pageEnd ";
        private static string mysqlOrderPageTemplate = "{0} order by {1} limit ?offset,?limit";
        private static string mysqlPageTemplate = "{0} limit ?offset,?limit";

        public static string fetchColumns(string strSQL)
        {
            String columns = strSQL.Substring(6, strSQL.IndexOf("from") - 6);
            return columns;
        }

        public static string fetchPageBody(string strSQL)
        {
            string body = strSQL.Substring(6, strSQL.Length - 6);
            return body;
        }

        public static string fetchWhere(string strSQL)
        {
            int index = strSQL.LastIndexOf("where");
            if (index == -1) return "";

            String where = strSQL.Substring(index, strSQL.Length - index);
            return where;
        }

        public static bool isPage(string strSQL)
        { 
            string strSql = strSQL.ToLower();

            /*if (AdoHelper.DbType == DatabaseType.SQLSERVER && strSql.IndexOf("from") != strSql.LastIndexOf("from"))
            {
                return true;
            }*/

            if (AdoHelper.DbType == DatabaseType.SQLSERVER && strSql.IndexOf("row_number()") == -1)
            {
                return false;
            }

            if(AdoHelper.DbType == DatabaseType.MYSQL && strSql.IndexOf("limit") == -1)
            {
                return false;
            }

            if (AdoHelper.DbType == DatabaseType.ORACLE && strSql.IndexOf("rowid") == -1)
            {
                return false;
            }

            return true;
        }

        public static string builderPageSQL(string strSql, string order, bool desc)
        {
            string columns = fetchColumns(strSql);
            string orderBy = order + (desc ? " desc " : " asc ");
            

            if (AdoHelper.DbType == DatabaseType.SQLSERVER && strSql.IndexOf("row_number()") == -1)
            {
                if (string.IsNullOrEmpty(order))
                {
                    throw new Exception(" SqlException: order field is null, you must support the order field for sqlserver page. ");
                }

                string pageBody = fetchPageBody(strSql);
                strSql = string.Format(mssqlPageTemplate, columns, orderBy, pageBody);
            }

            if (AdoHelper.DbType == DatabaseType.MYSQL)
            {
                if (!string.IsNullOrEmpty(order))
                {
                    strSql = string.Format(mysqlOrderPageTemplate, strSql, orderBy);
                }
                else
                {
                    strSql = string.Format(mysqlPageTemplate, strSql);
                }
            }
            
            return strSql;
        }
    }
}
