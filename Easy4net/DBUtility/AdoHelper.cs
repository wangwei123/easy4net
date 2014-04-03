using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OracleClient;
using Easy4net.Common;

namespace Easy4net.DBUtility
{
    public class AdoHelper
    {
        //获取数据库类型
        private static string strDbType = CommonUtils.GetConfigValueByKey("dbType").ToUpper();

        //将数据库类型转换成枚举类型
        public static DatabaseType DbType = DatabaseTypeEnumParse<DatabaseType>(strDbType);       

        //获取数据库连接字符串
        public static string ConnectionString = GetConnectionString("connectionString");

        //获取数据库命名参数符号，比如@(SQLSERVER)、:(ORACLE)
        public static string DbParmChar = DbFactory.CreateDbParmCharacter();
        
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        ///通过提供的参数，执行无结果集的数据库操作命令
        /// 并返回执行数据库操作所影响的行数。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
        /// <param name="commandParameters">执行命令所需的参数数组</param>
        /// <returns>返回通过执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            IDbCommand cmd = DbFactory.CreateDbCommand();

            using (IDbConnection conn = DbFactory.CreateDbConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        /// <summary>
        ///通过提供的参数，执行无结果集的数据库操作命令
        /// 并返回执行数据库操作所影响的行数。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
        /// <returns>返回通过执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText)
        {
            IDbCommand cmd = DbFactory.CreateDbCommand();

            using (IDbConnection conn = DbFactory.CreateDbConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        ///通过提供的参数，执行无结果集返回的数据库操作命令
        ///并返回执行数据库操作所影响的行数。
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
        /// <param name="commandParameters">执行命令所需的参数数组</param>
        /// <returns>返回通过执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(IDbConnection connection, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            IDbCommand cmd = DbFactory.CreateDbCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        ///通过提供的参数，执行无结果集返回的数据库操作命令
        ///并返回执行数据库操作所影响的行数。
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
        /// <returns>返回通过执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(IDbConnection connection, CommandType cmdType, string cmdText)
        {
            IDbCommand cmd = DbFactory.CreateDbCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        ///通过提供的参数，执行无结果集返回的数据库操作命令
        ///并返回执行数据库操作所影响的行数。
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">sql事务对象</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
        /// <param name="commandParameters">执行命令所需的参数数组</param>
        /// <returns>返回通过执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(IDbTransaction trans, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            int val = 0;
            IDbCommand cmd = DbFactory.CreateDbCommand();

            if (trans == null || trans.Connection == null)
            {
                using (IDbConnection conn = DbFactory.CreateDbConnection(AdoHelper.ConnectionString))
                {
                    PrepareCommand(cmd, conn, trans, cmdType, cmdText, commandParameters);
                    val = cmd.ExecuteNonQuery();
                }
            }
            else
            {
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
                val = cmd.ExecuteNonQuery();
            }

            cmd.Parameters.Clear();
            return val;
        }
        /// <summary>
        ///通过提供的参数，执行无结果集返回的数据库操作命令
        ///并返回执行数据库操作所影响的行数。
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">sql事务对象</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
        /// <returns>返回通过执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(IDbTransaction trans, CommandType cmdType, string cmdText)
        {
            IDbCommand cmd = DbFactory.CreateDbCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, null);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        /// <summary>
        /// 使用提供的参数，执行有结果集返回的数据库操作命令
        /// 并返回SqlDataReader对象
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
        /// <param name="commandParameters">执行命令所需的参数数组</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static IDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            IDbCommand cmd = DbFactory.CreateDbCommand();
            IDbConnection conn = DbFactory.CreateDbConnection(connectionString);

            //我们在这里使用一个 try/catch,因为如果PrepareCommand方法抛出一个异常，我们想在捕获代码里面关闭
            //connection连接对象，因为异常发生datareader将不会存在，所以commandBehaviour.CloseConnection
            //将不会执行。
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                cmd.Dispose();
                throw;
            }
        }
        /// <summary>
        ///使用提供的参数，执行有结果集返回的数据库操作命令
        /// 并返回SqlDataReader对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static IDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText)
        {
            IDbCommand cmd = DbFactory.CreateDbCommand();
            IDbConnection conn = DbFactory.CreateDbConnection(connectionString);

            //我们在这里使用一个 try/catch,因为如果PrepareCommand方法抛出一个异常，我们想在捕获代码里面关闭
            //connection连接对象，因为异常发生datareader将不会存在，所以commandBehaviour.CloseConnection
            //将不会执行。
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch(Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// 查询数据填充到数据集DataSet中
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns>数据集DataSet对象</returns>
        public static DataSet dataSet(string connectionString, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            IDbCommand cmd = DbFactory.CreateDbCommand();
            IDbConnection conn = DbFactory.CreateDbConnection(connectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                IDbDataAdapter sda = DbFactory.CreateDataAdapter(cmd);
                sda.Fill(ds);
                return ds;
            }
            catch
            {
                conn.Close();
                cmd.Dispose();
                throw;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 查询数据填充到数据集DataSet中
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">命令文本</param>
        /// <returns>数据集DataSet对象</returns>
        public static DataSet dataSet(string connectionString, CommandType cmdType, string cmdText)
        {
            DataSet ds = new DataSet();
            IDbCommand cmd = DbFactory.CreateDbCommand();
            IDbConnection conn = DbFactory.CreateDbConnection(connectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                IDbDataAdapter sda = DbFactory.CreateDataAdapter(cmd);
                sda.Fill(ds);
                return ds;
            }
            catch
            {
                conn.Close();
                cmd.Dispose();
                throw;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 依靠数据库连接字符串connectionString,
        /// 使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
        /// <param name="commandParameters">执行命令所需的参数数组</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            IDbCommand cmd = DbFactory.CreateDbCommand();

            using (IDbConnection connection = DbFactory.CreateDbConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }
        /// <summary>
        /// 依靠数据库连接字符串connectionString,
        /// 使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText)
        {
            IDbCommand cmd = DbFactory.CreateDbCommand();

            using (IDbConnection connection = DbFactory.CreateDbConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }
        /// <summary>
        ///依靠数据库连接字符串connectionString,
        /// 使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
        /// <param name="commandParameters">执行命令所需的参数数组</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(IDbConnection connection, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            IDbCommand cmd = DbFactory.CreateDbCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }
        /// <summary>
        ///依靠数据库连接字符串connectionString,
        /// 使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
        /// <param name="commandParameters">执行命令所需的参数数组</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(IDbConnection connection, CommandType cmdType, string cmdText)
        {
            IDbCommand cmd = DbFactory.CreateDbCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        ///依靠数据库连接字符串connectionString,
        /// 使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
        /// <param name="commandParameters">执行命令所需的参数数组</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(IDbConnection conn, IDbTransaction trans, CommandType cmdType, string cmdText)
        {
            IDbCommand cmd = DbFactory.CreateDbCommand();

            PrepareCommand(cmd, conn, trans, cmdType, cmdText, null);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        ///依靠数据库连接字符串connectionString,
        /// 使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
        /// <param name="commandParameters">执行命令所需的参数数组</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(IDbTransaction trans, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            IDbCommand cmd = DbFactory.CreateDbCommand();

            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params IDbDataParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// 查询缓存参数
        /// </summary>
        /// <param name="cacheKey">使用缓存名称查找值</param>
        /// <returns>缓存参数数组</returns>
        public static IDbDataParameter[] GetCachedParameters(string cacheKey)
        {
            IDbDataParameter[] cachedParms = (IDbDataParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            IDbDataParameter[] clonedParms = new IDbDataParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (IDbDataParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// 为即将执行准备一个命令
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <param name="conn">SqlConnection对象</param>
        /// <param name="trans">IDbTransaction对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(IDbCommand cmd, IDbConnection conn, IDbTransaction trans, CommandType cmdType, string cmdText, IDbDataParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (IDbDataParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// 根据传入的Key获取配置文件中
        /// 相应Key的数据库连接字符串
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string GetConnectionString(string Key)
        {
            try
            {
                return CommonUtils.GetConfigValueByKey(Key);
            }
            catch
            {
                throw new Exception("web.config文件appSettings中数据库连接字符串未配置或配置错误，必须为Key=\"connectionString\"");
            }
        }      

        /// <summary>
        /// 用于数据库类型的字符串枚举转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DatabaseTypeEnumParse<T>(string value)
        {
            try
            {
                return CommonUtils.EnumParse<T>(value);
            }
            catch
            {
                throw new Exception("数据库类型\"" + value + "\"错误，请检查！");
            }
        }                             
    }
}
