using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy4net.Common
{
    public class Field
    {
        private String fieldName;

        //不允许不传参数的构造函数
        private Field() { }
        public Field(String name)
        {
            fieldName = name;
        }

        public static WhereExpression operator ==(Field field, Object value)
        {
            return null;
        }

        public static WhereExpression operator !=(Field field, Object value)
        {
            return null;
        }

        public static WhereExpression operator >(Field field, Object value)
        {
            return null;
        }

        public static WhereExpression operator >=(Field field, Object value)
        {
            return null;
        }

        public static WhereExpression operator <(Field field, Object value)
        {
            return null;
        }

        public static WhereExpression operator <=(Field field, Object value)
        {
            return null;
        }
    }
}
