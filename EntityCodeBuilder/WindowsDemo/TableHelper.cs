using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Easy4net;
using Easy4net.DBUtility;
using EntityCodeBuilder.Entity;

namespace WindowsDemo
{
    public class TableHelper
    {
        private static DBHelper db = DBHelper.getInstance();


        /// <summary>  
        /// 获取局域网内的所有数据库服务器名称  
        /// </summary>  
        /// <returns>服务器名称数组</returns>  
        public static List<string> GetSqlServerNames()
        {
            DataTable dataSources = SqlClientFactory.Instance.CreateDataSourceEnumerator().GetDataSources();

            DataColumn column = dataSources.Columns["InstanceName"];
            DataColumn column2 = dataSources.Columns["ServerName"];

            DataRowCollection rows = dataSources.Rows;
            List<string> Serverlist = new List<string>();
            string array = string.Empty;
            for (int i = 0; i < rows.Count; i++)
            {
                string str2 = rows[i][column2] as string;
                string str = rows[i][column] as string;
                if (((str == null) || (str.Length == 0)) || ("MSSQLSERVER" == str))
                {
                    array = str2;
                }
                else
                {
                    array = str2 + @"/" + str;
                }

                Serverlist.Add(array);
            }

            Serverlist.Sort();

            return Serverlist;
        }

        /// <summary>  
        /// 查询sql中的非系统库  
        /// </summary>  
        /// <param name="connection"></param>  
        /// <returns></returns>  
        public static List<string> databaseList(string connection)
        {
            List<string> getCataList = new List<string>();
            string cmdStirng = "select name from sys.databases where database_id > 4";
            SqlConnection connect = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand(cmdStirng, connect);
            try
            {
                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                    IDataReader dr = cmd.ExecuteReader();
                    getCataList.Clear();
                    while (dr.Read())
                    {
                        getCataList.Add(dr["name"].ToString());
                    }
                    dr.Close();
                }

            }
            catch (SqlException e)
            {
                //MessageBox.Show(e.Message);  
            }
            finally
            {
                if (connect != null && connect.State == ConnectionState.Open)
                {
                    connect.Dispose();
                }
            }
            return getCataList;
        }

        /// <summary>  
        /// 获取列名  
        /// </summary>  
        /// <param name="connection"></param>  
        /// <returns></returns>  
        public static List<TableName> GetTables()
        {
            SqlConnection connection = (SqlConnection)DbFactory.CreateDbConnection(AdoHelper.ConnectionString);
            List<TableName> tablelist = new List<TableName>();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    DataTable objTable = connection.GetSchema("Tables");
                    foreach (DataRow row in objTable.Rows)
                    {
                        TableName tb = new TableName();
                        tb.Name = row[2].ToString();
                        tablelist.Add(tb);
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Dispose();
                }
            }

            return tablelist;
        }

        /// <summary>  
        /// 获取字段  
        /// </summary>  
        /// <param name="connection"></param>  
        /// <param name="TableName"></param>  
        /// <returns></returns>  
        public static List<TableColumn> GetColumnField(string TableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT a.name,");
            sb.Append(" b.name as type,");
            sb.Append(" CASE COLUMNPROPERTY(a.id,a.name,'IsIdentity') WHEN 1 THEN '√' ELSE '' END as IsIdentity, ");
            sb.Append(" CASE WHEN EXISTS ( SELECT * FROM sysobjects WHERE xtype='PK' AND name IN ( SELECT name FROM sysindexes WHERE id=a.id AND indid IN ( SELECT indid FROM sysindexkeys ");
            sb.Append(" WHERE id=a.id AND colid IN ( SELECT colid FROM syscolumns WHERE id=a.id AND name=a.name ) ) ) ) THEN '√' ELSE '' END as IsPrimaryKey,");
            sb.Append(" CASE a.isnullable WHEN 1 THEN '√' ELSE '' END as IsNull ");
            sb.Append(" FROM syscolumns a ");
            sb.Append(" LEFT  JOIN systypes      b ON a.xtype=b.xusertype ");
            sb.Append(" INNER JOIN sysobjects    c ON a.id=c.id AND c.xtype='U' AND c.name<>'dtproperties' ");
            sb.Append(" LEFT  JOIN syscomments   d ON a.cdefault=d.id ");
            sb.Append(" WHERE c.name = '").Append(TableName).Append("' ");
            sb.Append(" ORDER BY c.name, a.colorder");

            List<TableColumn> list = db.FindBySql<TableColumn>(sb.ToString());
            return list;
        }
    }
}
