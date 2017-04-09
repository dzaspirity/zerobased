using System;
using System.Linq;
using System.Reflection;

namespace Zerobased.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        ///     Check if <paramref name="type"/> is <see cref="Nullable{T}"/>.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        ///     Returns the underlying type argument of the specified <paramref name="type"/>.
        ///     If <paramref name="type"/> is not <see cref="Nullable{T}"/>,
        ///     returns specified <paramref name="type"/> itself.
        /// </summary>
        /// <param name="type">A <see cref="Type"/> object that describes a closed generic null-able type.</param>
        /// <returns>
        ///     If <paramref name="type"/> is <see cref="Nullable{T}"/> returns underlying type argument.
        ///     Otherwise returns <paramref name="type"/> itself.
        /// </returns>
        public static Type ExtractNullable(this Type type)
        {
            Type t = Nullable.GetUnderlyingType(type);
            return t ?? type;
        }

        public static bool Is(this Type source, Type type)
        {
            return type.GetTypeInfo().IsAssignableFrom(source.GetTypeInfo());
        }

        public static bool Is<T>(this Type source)
        {
            return source.Is(typeof(T));
        }

        public static bool IsPlain(this Type type)
        {
            type = type.ExtractNullable();
            TypeInfo typeInfo = type.GetTypeInfo();
            bool isPlain = typeInfo.IsEnum ||
                           typeInfo.IsPrimitive ||
                           type == typeof(DateTime) ||
                           type == typeof(string) ||
                           type == typeof(decimal) ||
                           type == typeof(Guid);
            return isPlain;
        }

        public static PropertyInfo GetKeyProperty(this Type type)
        {
            PropertyInfo[] allProperties = type.GetTypeInfo().GetProperties();
            PropertyInfo keyProp = allProperties.FirstOrDefault(prop => prop.IsKey(type));
            return keyProp;
        }
    }
}
