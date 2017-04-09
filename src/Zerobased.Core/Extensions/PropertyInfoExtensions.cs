using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Zerobased.Extensions
{
    /// <summary>
    ///     Additional extensions for <see cref="PropertyInfo"/>
    /// </summary>
    public static class PropertyInfoExtensions
    {
        internal static readonly IReadOnlyCollection<string> KeyPropertyNames = new ReadOnlyCollection<string>(new[]
            {
                "Id",
                "{0}Id",
                "{0}_Id"
            });

        /// <summary>
        ///     Determines that property has <see cref="RequiredAttribute"/>
        ///     or its type does not allow NULL assignment.
        /// </summary>
        /// <param name="property">Property to check.</param>
        public static bool IsRequired(this PropertyInfo property)
        {
            bool isRequired = true;
            if (!property.IsDefined<RequiredAttribute>())
            {
                Type propType = property.PropertyType;
                isRequired = !propType.IsNullable() && !propType.GetTypeInfo().IsClass;
            }
            return isRequired;
        }

        /// <summary>
        ///     Check if property has System.<see cref="KeyAttribute"/>
        ///     or its name matches with one of <see cref="KeyPropertyNames"/>.
        /// </summary>
        /// <param name="property">Property to check.</param>
        /// <param name="baseType"></param>
        public static bool IsKey(this PropertyInfo property, Type baseType)
        {
            bool isKey = property.IsDefined<KeyAttribute>();
            if (!isKey)
            {
                HashSet<string> keyPropertyNames = KeyPropertyNames.Select(n => n.FormatWith(baseType.Name)).ToHashSet();
                isKey = keyPropertyNames.Contains(property.Name);
            }
            return isKey;
        }

        /// <summary>
        ///     Check if property type is a plain type.
        /// </summary>
        /// <param name="property">Property to check.</param>
        /// <returns>
        ///     true if property type is plain. Otherwise returns false
        /// </returns>
        public static bool IsPlain(this PropertyInfo property)
        {
            return property.PropertyType.IsPlain();
        }
    }
}
