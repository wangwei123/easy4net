using System;
using System.Collections.Generic;
using System.Text;

namespace Easy4net.Common
{
    public class TypeUtils
    {
        public static object ConvertForType(object value,Type type)
        {
            if (Convert.IsDBNull(value) || (value == null))
            {
                return null;
            }

            string typeName = type.FullName.ToString();
            System.Console.WriteLine(typeName);

            if (type == typeof(System.Nullable<UInt16>))
            {
                value = Convert.ToUInt16(value);
            }
            else if (type == typeof(System.Nullable<UInt32>))
            {
                value = Convert.ToUInt32(value);
            }
            else if (type == typeof(System.Nullable<UInt64>))
            {
                value = Convert.ToUInt64(value);
            }
            else if (type == typeof(System.Nullable<Int32>))
            {
                value = Convert.ToInt32(value);
            }
            else if (type == typeof(System.Nullable<Int64>))
            {
                value = Convert.ToInt64(value);
            }

            switch (typeName)
            {
                case "System.String":
                    if (!isNullOrEmpty(value))
                        value = value.ToString();
                    break;
                case "System.Boolean":
                    if (!isNullOrEmpty(value))
                        value = Convert.ToBoolean(value);
                    break;
                case "System.Int16":
                    if (!isNullOrEmpty(value))
                        value = Convert.ToInt16(value);
                    break;
                case "System.Int32":
                    if (!isNullOrEmpty(value))
                        value = Convert.ToInt32(value);
                    break;
                case "System.Int64":
                    if (!isNullOrEmpty(value))
                        value = Convert.ToInt64(value);
                    break;
                case "System.Double":
                    if (!isNullOrEmpty(value))
                        value = Convert.ToDouble(value);
                    break;
                case "System.Float":
                    if (!isNullOrEmpty(value))
                        value = Convert.ToDouble(value);
                    break;
                case "System.Single":
                    if (!isNullOrEmpty(value))
                        value = Convert.ToDouble(value);
                    break;
                case "System.Decimal":
                    if (!isNullOrEmpty(value))
                        value = Convert.ToDecimal(value);
                    break;
                case "System.DateTime":
                    if (!isNullOrEmpty(value))
                        value = Convert.ToDateTime(value);
                    break;
            }

            return value;
        }

        static bool isNullOrEmpty(object val)
        {
            if (val == null) return true;
            if (val.ToString() == "") return true;
            return false;
        }
    }
}
