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
            switch (type.FullName)
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
                case "System.Int32":
                case "System.Int64":
                    if (!isNullOrEmpty(value))
                        value = Convert.ToInt32(value);
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
