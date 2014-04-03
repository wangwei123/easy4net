using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.Common;
using Orm.DBUtility;

namespace Orm.DBTransaction
{
    public class TransactionManager
    {   
        public static IDbTransaction CreateTransaction()
        {
            return DbFactory.CreateDbTransaction();
        }
    }
}
