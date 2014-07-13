using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsDemo
{
    public class TypeHelper
    {
        public static string GetType(string type)
        {
            string newType = "String";

            switch (type)
            {
                case "varchar":
                case "varchar2":
                case "nvarchar":
                case "char":
                    newType = "String";
                    break;
                case "int":
                case "integer":
                case "bit":
                case "smallint":
                    newType = "int";
                    break;
                case "long":
                case "bitint":
                    newType = "long";
                    break;
                case "date":
                case "datetime":
                case "datetime2":
                case "datetimeoffset":
                    newType = "DateTime";
                    break;
                case "decimal":
                case "number":
                case "money":
                case "numeric":
                    newType = "Decimal";
                    break;
                case "double":
                    newType = "double";
                    break;
                case "float":
                    newType = "float";
                    break;
            }

            return newType;
        }
    }
}
