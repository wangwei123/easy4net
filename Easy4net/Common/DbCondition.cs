using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easy4net.DBUtility;

namespace Easy4net.Common
{
    public class DbCondition : Map
    {
        private static string WHERE = " WHERE ";
        private static string EQUAL = " {0} = {1} ";

        private static string AND_EQ = " AND {0} = {1} ";
        private static string OR_EQ = " OR {0} = {1} ";

        private static string GT = " {0} > {1} ";
        private static string GT_EQ = " {0} >= {1} ";

        private static string AND_GT = " AND {0} > {1} ";
        private static string AND_GT_EQ = " AND {0} >= {1} ";

        private static string OR_GT = " OR {0} > {1} ";
        private static string OR_GT_EQ = " OR {0} >= {1} ";

        private static string LT = " {0} < {1} ";
        private static string LT_EQ = " {0} <= {1} ";

        private static string AND_LT = " AND {0} < {1} ";
        private static string AND_LT_EQ = " AND {0} <= {1} ";
       
        private static string OR_LT = " OR {0} < {1} ";
        private static string OR_LT_EQ = " OR {0} <= {1} ";
        
        private static string ORDER_BY_ASC = " ORDER BY {0} ASC ";
        private static string ORDER_BY_DESC = " ORDER BY {0} DESC ";

        private static string paramChar = DbFactory.CreateDbParmCharacter();
        private StringBuilder sbSQL = new StringBuilder();
        public string queryString = String.Empty;
        public ColumnInfo Columns = new ColumnInfo();

        public DbCondition()
        {

        }

        public DbCondition(string query)
        {
            this.queryString = query;
            sbSQL.Append(query);
        }

        public DbCondition Query(string query)
        {
            this.queryString = query;
            sbSQL.Append(query);
            return this;
        }

        public DbCondition Where()
        {
            sbSQL.Append(WHERE);
            return this;
        }

        public DbCondition Where(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(WHERE + EQUAL, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition Equal(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(EQUAL, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition AndEqual(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(AND_EQ, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition OrEqual(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(OR_EQ, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition GreaterThan(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(GT, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition GreaterThanEqual(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(GT_EQ, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition AndGreaterThan(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(AND_GT, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition AndGreaterThanEqual(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(AND_GT_EQ, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition OrGreaterThan(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(OR_GT, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition OrGreaterThanEqual(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(OR_GT_EQ, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition LessThan(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(LT, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition LessThanEqual(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(LT_EQ, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition AndLessThan(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(AND_LT, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition AndLessThanEqual(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(AND_LT_EQ, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition OrLessThan(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(OR_LT, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition OrLessThanEqual(string fieldName, object fieldValue)
        {
            string formatName = formatKey(fieldName);
            sbSQL.AppendFormat(OR_LT_EQ, fieldName, paramChar + formatName);
            Columns[formatName] = fieldValue;

            return this;
        }

        public DbCondition And(string fieldName, object fieldValue)
        {
            return this.AndEqual(fieldName, fieldValue);
        }

        public DbCondition Or(string fieldName, object fieldValue)
        {
            return this.OrEqual(fieldName, fieldValue);
        }

        public DbCondition OrderByASC(string fieldName)
        {
            sbSQL.AppendFormat(ORDER_BY_ASC, fieldName);

            return this;
        }

        public DbCondition OrderByDESC(string fieldName)
        {
            sbSQL.AppendFormat(ORDER_BY_DESC, fieldName);

            return this;
        }

        public DbCondition Like(string fieldName, object fieldValue)
        {
            sbSQL.AppendFormat(" {0} LIKE '%{1}%' ", fieldName, fieldValue);
            return this;
        }

        public DbCondition AndLike(string fieldName, object fieldValue)
        {
            sbSQL.AppendFormat(" AND {0} LIKE '%{1}%' ", fieldName, fieldValue);
            return this;
        }

        public DbCondition OrLike(string fieldName, object fieldValue)
        {
            sbSQL.AppendFormat(" OR {0} LIKE '%{1}%' ", fieldName, fieldValue);
            return this;
        }

        public DbCondition LeftLike(string fieldName, object fieldValue)
        {
            sbSQL.AppendFormat(" {0} LIKE '%{1}' ", fieldName, fieldValue);
            return this;
        }

        public DbCondition AndLeftLike(string fieldName, object fieldValue)
        {
            sbSQL.AppendFormat(" AND {0} LIKE '%{1}' ", fieldName, fieldValue);
            return this;
        }

        public DbCondition OrLeftLike(string fieldName, object fieldValue)
        {
            sbSQL.AppendFormat(" OR {0} LIKE '%{1}' ", fieldName, fieldValue);
            return this;
        }

        public DbCondition RightLike(string fieldName, object fieldValue)
        {
            sbSQL.AppendFormat(" {0} LIKE '{1}%' ", fieldName, fieldValue);
            return this;
        }

        public DbCondition AndRightLike(string fieldName, object fieldValue)
        {
            sbSQL.AppendFormat(" AND {0} LIKE '{1}%' ", fieldName, fieldValue);
            return this;
        }

        public DbCondition OrRightLike(string fieldName, object fieldValue)
        {
            sbSQL.AppendFormat(" OR {0} LIKE '{1}%' ", fieldName, fieldValue);
            return this;
        }

        public override string ToString()
        {
            return sbSQL.ToString();
        }

        private string formatKey(string key)
        {
            int index = key.IndexOf('.');
            if (index >= 0)
            {
                key = key.Substring(index + 1, key.Length-(index+1));
            }

            return key;
        }
    }
}
