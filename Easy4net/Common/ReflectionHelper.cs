using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Easy4net.Common
{
    public class ReflectionHelper
    {
        public static PropertyInfo[] GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static FieldInfo[] GetFields(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        }

        #region 快速执行方法
        /// <summary>
        /// 快速执行Method
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object FastMethodInvoke(object obj, MethodInfo method, params object[] parameters)
        {
            return DynamicCalls.GetMethodInvoker(method)(obj, parameters);
        }

        /// <summary>
        /// 快速实例化一个T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>()
        {
            return (T)Create(typeof(T))();
        }

        /// <summary>
        /// 快速实例化一个FastCreateInstanceHandler
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FastCreateInstanceHandler Create(Type type)
        {
            return DynamicCalls.GetInstanceCreator(type);
        }
        #endregion

        #region 设置属性值
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object obj, PropertyInfo property, object value)
        {
            if (property.CanWrite)
            {
                var propertySetter = DynamicCalls.GetPropertySetter(property);
                value = TypeUtils.ConvertForType(value, property.PropertyType);
                propertySetter(obj, value);
            }
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            SetPropertyValue(obj.GetType(), obj, propertyName, value);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(Type type, object obj, string propertyName, object value)
        {
            PropertyInfo property = type.GetProperty(propertyName);
            if (property != null)
            {
                SetPropertyValue(obj, property, value);
            }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue<T>(T entity, string propertyName)
        {
            PropertyInfo property = entity.GetType().GetProperty(propertyName);
            if (property != null)
            {
                return property.GetValue(entity, null);
            }

            return null;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue<T>(T entity, PropertyInfo property)
        {
            if (property != null)
            {
                return property.GetValue(entity, null);
            }

            return null;
        }
        #endregion

        /*public static void SetPropertyValue(Object obj, PropertyInfo property, Object value)
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
        }*/        
    }
}
