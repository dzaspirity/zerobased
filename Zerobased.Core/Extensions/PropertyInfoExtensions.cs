using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Zerobased.Extensions
{
    public static class PropertyInfoExtensions
    {
        internal static readonly IReadOnlyCollection<string> KeyPropertyNames = new ReadOnlyCollection<string>( new[]
            {
                "Id",
                "{0}Id",
                "{0}_Id"
            });

        /// <summary>
        ///     Determines that property has System.ComponentModel.DataAnnotations.RequiredAttribute
        ///     or its type does not support <value>NULL</value>.
        /// </summary>
        /// <param name="property">Property to check.</param>
        public static bool IsRequired(this PropertyInfo property)
        {
            bool isRequired = true;

            if (!property.HasAttribute<RequiredAttribute>())
            {
                Type propType = property.PropertyType;
                isRequired = !propType.IsNullable() && !propType.IsClass;
            }

            return isRequired;
        }

        /// <summary>
        ///     Check if property has System.ComponentModel.DataAnnotations.KeyAttribute
        ///     or its name matchs with one of PropertyInfoExtensions.KeyPropertyNames.
        /// </summary>
        /// <param name="property">Property to check.</param>
        /// <param name="baseClass"></param>
        public static bool IsKey(this PropertyInfo property, string baseClass = null)
        {
            if (property.ReflectedType == null)
            {
                throw new NotSupportedException("Method IsKey does not supports global members.");
            }

            bool isKey = property.HasAttribute<KeyAttribute>();

            if (!isKey)
            {
                baseClass = baseClass ?? property.ReflectedType.Name;
                string[] keyPropertyNames = KeyPropertyNames.Select(n => n.FormatWith(baseClass)).ToArray();
                isKey = keyPropertyNames.Contains(property.Name);
            }

            return isKey;
        }

        /// <summary>
        /// Check if property type is a plain type.
        /// </summary>
        /// <param name="property">Property to check.</param>
        /// <returns>
        ///     <value>TRUE</value> if property type is plain.
        ///     Overwise returns <value>FALSE</value>
        /// </returns>
        public static bool IsPlain(this PropertyInfo property)
        {
            return property.PropertyType.IsPlain();
        }
    }
}
