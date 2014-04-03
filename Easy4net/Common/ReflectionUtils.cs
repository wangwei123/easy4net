using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Orm.Common
{
    public class ReflectionUtils
    {
        public static PropertyInfo[] GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static FieldInfo[] GetFields(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        }

        public static void SetPropertyValue(Object obj, PropertyInfo property, Object value)
        {
            //创建Set委托
            SetHandler setter = DynamicMethodCompiler.CreateSetHandler(obj.GetType(),property);
                        
            //先获取该私有成员的数据类型
            Type type = property.PropertyType;

            //通过数据类型转换
            value = TypeUtils.ConvertForType(value, type);
            
            //将值设置到对象中
            setter(obj, value);  
        }

        public static Object GetPropertyValue(Object obj, PropertyInfo property)
        {
            //创建Set委托
            GetHandler getter = DynamicMethodCompiler.CreateGetHandler(obj.GetType(), property);

            //获取属性值
            return getter(obj);
            
        }

        public static void SetFieldValue(Object obj, FieldInfo field, Object value)
        {
            //创建Set委托
            SetHandler setter = DynamicMethodCompiler.CreateSetHandler(obj.GetType(), field);
            
            //先获取该私有成员的数据类型
            Type type = field.FieldType;

            //通过数据类型转换
            value = TypeUtils.ConvertForType(value, type);

            //将值设置到对象中
            setter(obj, value);
        }

        public static Object GetFieldValue(Object obj, FieldInfo field)
        {
            //创建Set委托
            GetHandler getter = DynamicMethodCompiler.CreateGetHandler(obj.GetType(), field);

            //获取字段值
            return getter(obj);           
        }        
    }
}
