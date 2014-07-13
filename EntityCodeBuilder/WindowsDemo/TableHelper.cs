using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Easy4net;
using Easy4net.DBUtility;

namespace WindowsDemo
{
    public class TableHelper
    {
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
        public static List<string> GetTables(string connection)
        {
            List<string> tablelist = new List<string>();
            SqlConnection objConnetion = new SqlConnection(connection);
            try
            {
                if (objConnetion.State == ConnectionState.Closed)
                {
                    objConnetion.Open();
                    DataTable objTable = objConnetion.GetSchema("Tables");
                    foreach (DataRow row in objTable.Rows)
                    {
                        tablelist.Add(row[2].ToString());
                    }
                }
            }
            catch
            {

            }
            finally
            {
                if (objConnetion != null && objConnetion.State == ConnectionState.Closed)
                {
                    objConnetion.Dispose();
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
        public static List<string> GetColumnField(string connection, string TableName)
        {
            List<string> Columnlist = new List<string>();
            SqlConnection objConnetion = new SqlConnection(connection);
            try
            {
                if (objConnetion.State == ConnectionState.Closed)
                {
                    objConnetion.Open();
                }


                StringBuilder sb = new StringBuilder("SELECT a.name, CASE COLUMNPROPERTY(a.id,a.name,'IsIdentity') WHEN 1 THEN 'Y' ELSE 'N' END as IsIdentity, ");
                sb.Append(" CASE WHEN EXISTS ( SELECT * FROM sysobjects WHERE xtype='PK' AND name IN ( SELECT name FROM sysindexes WHERE id=a.id AND indid IN ( SELECT indid FROM sysindexkeys ");
                sb.Append(" WHERE id=a.id AND colid IN ( SELECT colid FROM syscolumns WHERE id=a.id AND name=a.name ) ) ) ) THEN 'Y' ELSE 'N' END,");
                sb.Append(" b.name as type, CASE a.isnullable WHEN 1 THEN 'Y' ELSE 'N' END as isNull ");
                sb.Append(" FROM syscolumns a ");
                sb.Append(" LEFT  JOIN systypes      b ON a.xtype=b.xusertype ");
                sb.Append(" INNER JOIN sysobjects    c ON a.id=c.id AND c.xtype='U' AND c.name<>'dtproperties' ");
                sb.Append(" LEFT  JOIN syscomments   d ON a.cdefault=d.id ");
                sb.Append(" WHERE c.name = 'student' ");
                sb.Append(" ORDER BY c.name, a.colorder");

               

                SqlCommand cmd = new SqlCommand(sb.ToString(), objConnetion);
                SqlDataReader objReader = cmd.ExecuteReader();

                while (objReader.Read())
                {
                    Columnlist.Add(objReader[0].GetType().FullName + " : " + objReader[0].ToString());

                }
            }
            catch
            {

            }
            objConnetion.Close();
            return Columnlist;
        }
    }
}
