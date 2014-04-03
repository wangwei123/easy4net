using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using Easy4net.DBUtility;

namespace Easy4net.Common
{
    public class TableInfo
    {
        private string tableName;
        private int strategy;        
        private IdInfo id = new IdInfo();
        private ColumnInfo columns = new ColumnInfo();
        private Map propToColumn = new Map();                

        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        public int Strategy
        {
            get { return strategy; }
            set { strategy = value; }
        }

        public IdInfo Id
        {
            get { return id; }
            set { id = value; }
        }        
                
        public ColumnInfo Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        public Map PropToColumn
        {
            get { return propToColumn; }
            set { propToColumn = value; }
        } 

        public IDbDataParameter[] GetParameters()
        {
            IDbDataParameter[] parameters = null;
            if (this.Columns != null && this.Columns.Count > 0)
            {
                parameters = DbFactory.CreateDbParameters(this.Columns.Count);
                EntityHelper.SetParameters(this.Columns, parameters);
            }
            return parameters;
        }       
    }
}
