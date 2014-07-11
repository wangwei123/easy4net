using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easy4net.DBUtility;

namespace Easy4net.Common
{
    public class SQLBuilderHelper
    {
        private static string mssqlPageTemplate = "select {0} from (select {1},ROW_NUMBER() OVER(order by {2}) AS RowNumber FROM {3}) a where RowNumber BETWEEN {4} and {5} order by {6}";
        private static string mysqlPageTemplate = "{0} order by {1} limit {2},{3}";

        public static string fetchColumns(string strSQL)
        {
            String columns = strSQL.Substring(6, strSQL.LastIndexOf("from") - 6);
            return columns;
        }

        public static string builderPageSQL(string strSql, string columns, string tableName, string order, bool desc, int pageIndex, int pageSize)
        {
            string orderBy = order + (desc ? " desc " : " asc ");
            if (AdoHelper.DbType == DatabaseType.SQLSERVER && strSql.IndexOf("ROW_NUMBER()") == -1)
            {
                int start = (pageIndex-1) * pageSize + 1;
                int end = pageIndex * pageSize;
                strSql = string.Format(mssqlPageTemplate, columns, columns, orderBy, tableName, start, end, orderBy);
            }

            if (AdoHelper.DbType == DatabaseType.MYSQL)
            {
                int offset = (pageIndex-1) * pageSize;
                int limit = pageSize;
                strSql = string.Format(mysqlPageTemplate, strSql, orderBy, offset, limit);
            }
            
            return strSql;
        }
    }
}
