using System;
using System.Reflection;

namespace Cryptonite.Infrastructure.Helpers
{
    public static class GenericsHelpers
    {
        public static TReturn GetPropertyValue<T, TReturn>(this T obj, string property, bool throwExceptionIfNull = false)
            where T : class
        {
            obj.ValidateProperty(property, out var propertyInfo);
            var value = propertyInfo.GetValue(obj);

            if (throwExceptionIfNull && value == null)
            {
                throw new ArgumentException($"Property {property} was found and it was null");
            }

            return (TReturn)value;
        }

        public static object GetPropertyObjectValue<T>(this T obj, string property, bool throwExceptionIfNull = false)
            where T : class
        {
            obj.ValidateProperty(property, out var propertyInfo);
            var value = propertyInfo.GetValue(obj);

            if (throwExceptionIfNull && value == null)
            {
                throw new ArgumentException($"Property {property} was found and it was null");
            }

            return value;
        }

        public static bool IsPropertyNull<T, TReturn>(this T obj, string property) where T : class
        {
            return obj.GetPropertyValue<T, TReturn>(property) == null;
        }

        public static bool IsPropertyNull<T>(this T obj, string property) where T : class
        {
            return obj.GetPropertyObjectValue(property) == null;
        }

        public static void SetPropertyValue<T, TValue>(this T obj, string property, TValue value) where T : class
        {
            obj.ValidateProperty(property, out var propertyInfo);

            if (propertyInfo.PropertyType != typeof(TValue))
            {
                throw new ArgumentException($"Property {property} does not have the expected type {typeof(TValue)}");
            }

            propertyInfo.SetValue(obj, value);
        }

        public static Type GetPropertyType<T>(this T obj, string property) where T : class
        {
            obj.ValidateProperty(property, out var propertyInfo);
            return propertyInfo.PropertyType;
        }

        private static void ValidateProperty<T>(this T obj, string property, out PropertyInfo propertyInfo)
        {
            propertyInfo = obj.GetType().GetProperty(property);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property {property} was not found on object of type {typeof(T)}");
            }
        }
    }
}