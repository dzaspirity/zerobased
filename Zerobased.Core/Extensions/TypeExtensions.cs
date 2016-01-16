using System;
using System.Reflection;
using System.Linq;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Zerobased.Extensions
{
    public static class TypeExtensions
    {
        private static readonly ConcurrentDictionary<Type, TypeConverter> _typeConverterCache = new ConcurrentDictionary<Type, TypeConverter>();
        
        /// <summary>
        /// Check if <paramref name="type"/> is System.Nullable[T].
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        ///     Returns the underlying type argument of the specified <paramref name="type"/>.
        ///     If <paramref name="type"/> is not System.Nullable[T], 
        ///     returns specified <paramref name="type"/> itself.
        /// </summary>
        /// <param name="type">A System.Type object that describes a closed generic nullable type.</param>
        /// <returns>
        ///     If <paramref name="type"/> is System.Nullable[T] returns underlying type argument.
        ///     Overwise returns <paramref name="type"/> itself.
        /// </returns>
        public static Type ExtractNullable(this Type type)
        {
            Type t = Nullable.GetUnderlyingType(type);
            return t ?? type;
        }

        public static bool Is(this Type source, Type type)
        {
            return type.IsAssignableFrom(source);
        }

        public static bool Is<T>(this Type source)
        {
            return source.Is(typeof(T));
        }

        public static bool IsPlain(this Type type)
        {
            type = type.ExtractNullable();

            bool isPlain = type.IsEnum ||
                           type.IsPrimitive ||
                           type == typeof(DateTime) ||
                           type == typeof(string) ||
                           type == typeof(decimal) ||
                           type == typeof(Guid);

            return isPlain;
        }

        public static PropertyInfo GetKeyProperty(this Type type, string baseClass = null)
        {
            PropertyInfo[] allProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo keyProp = allProperties.FirstOrDefault(prop => prop.IsKey(baseClass));

            return keyProp;
        }

        /// <summary>
        ///     Returns a type converter for the specified type.
        /// </summary>
        /// <param name="type">The System.Type of the target component.</param>
        /// <returns>A System.ComponentModel.TypeConverter for the specified type.</returns>
        public static TypeConverter GetConverter(this Type type)
        {
            return _typeConverterCache.GetOrAdd(type, TypeDescriptor.GetConverter);
        }
    }
}
