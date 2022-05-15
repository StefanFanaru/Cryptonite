using System;
using System.Collections.Generic;

namespace Cryptonite.Infrastructure.Helpers
{
    public static class ReflectionHelpers
    {
        private static readonly HashSet<Type> NumericTypes = new()
        {
            typeof(int), typeof(double), typeof(decimal),
            typeof(long), typeof(short), typeof(sbyte),
            typeof(byte), typeof(ulong), typeof(ushort),
            typeof(uint), typeof(float)
        };

        public static List<string> GetTypePropertyNames<T>(this T obj) where T : class
        {
            return typeof(T).GetProperties().Select(x => x.Name).ToList();
        }

        public static List<string> GetTypePropertyNames<T>() where T : class
        {
            return typeof(T).GetProperties().Select(x => x.Name).ToList();
        }

        public static bool IsNumericType(this Type myType)
        {
            return NumericTypes.Contains(Nullable.GetUnderlyingType(myType) ?? myType);
        }
    }
}