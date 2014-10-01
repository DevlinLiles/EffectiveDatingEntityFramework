using System;
using System.Reflection;

namespace EffectiveDating.Core.Extensions
{
    public static class ReflectionHelper
    {
        private static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            PropertyInfo propInfo = null;
            do
            {
                propInfo = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            }
            while (propInfo == null && type != null);
            return propInfo;
        }

        private static MethodInfo GetMethodInfo(Type type, string methodName)
        {
            MethodInfo methodInfo = null;
            do
            {
                methodInfo = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            }
            while (methodInfo == null && type != null);
            return methodInfo;
        }

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            Type objType = obj.GetType();
            PropertyInfo propInfo = GetPropertyInfo(objType, propertyName);
            if (propInfo == null)
                throw new ArgumentOutOfRangeException("propertyName",
                  string.Format("Couldn't find property {0} in type {1}", propertyName, objType.FullName));
            return propInfo.GetValue(obj, null);
        }

        public static object CallMethod(this object obj, string methodName, params object[] args)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            Type objType = obj.GetType();
            MethodInfo methodInfo = GetMethodInfo(objType, methodName);
            if (methodInfo == null)
            {
                throw new ArgumentOutOfRangeException("methodName", string.Format("Couldn't find method {0} in type {1}", methodName, objType.FullName));
            }
            return methodInfo.Invoke(obj, null);
        }

        public static void SetPropertyValue(this object obj, string propertyName, object val)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            Type objType = obj.GetType();
            PropertyInfo propInfo = GetPropertyInfo(objType, propertyName);
            if (propInfo == null)
            {
                throw new ArgumentOutOfRangeException("propertyName", string.Format("Couldn't find property {0} in type {1}", propertyName, objType.FullName));
            }
            propInfo.SetValue(obj, val, null);
        }

        public static object InvokeGenericMethod(this object instance, string methodName, Type genericType, params object[] methodArgs)
        {
            return instance.InvokeGenericMethod(methodName, new[] {genericType}, methodArgs);
        }

        public static object InvokeGenericMethod(this object instance, string methodName, Type[] genericTypes, params object[] methodArgs)
        {
            var type = instance.GetType();
            MethodInfo method = type.GetMethod(methodName);
            MethodInfo generic = method.MakeGenericMethod(genericTypes);
            return generic.Invoke(instance, methodArgs);
        }
    }
}