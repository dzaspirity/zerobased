using System;
using System.Reflection;

namespace Zerobased
{
    public static class MemberInfoExtensions
    {
        /// <summary>
        ///     Generic wrapper for System.Attribute.GetCustomAttribute method.
        ///     Retrieves a custom attribute applied to a member of a type. Parameters specify
        ///     the member, and the type of the custom attribute to search for.
        /// </summary>
        /// <typeparam name="TAttr">
        ///      The type, or a base type, of the custom attribute to search for.
        /// </typeparam>
        /// <param name="info">
        ///     An object derived from the System.Reflection.MemberInfo class that describes
        ///     a constructor, event, field, method, or property member of a class.
        /// </param>
        /// <returns>
        ///     A reference to the single custom attribute of type <typeparamref name="TAttr"/> that is
        ///     applied to element, or null if there is no such attribute.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="info"/> is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        ///     <paramref name="info"/> is not a constructor, method, property, event, type, or field.
        /// </exception>
        /// <exception cref="System.Reflection.AmbiguousMatchException">
        ///     More than one of the requested attributes was found.
        /// </exception>
        /// <exception cref="System.TypeLoadException">
        ///     A custom attribute type cannot be loaded.
        /// </exception>
        public static TAttr GetAttribute<TAttr>(this MemberInfo info)
            where TAttr : Attribute
        {
            var attribute = ZAttribute.GetCustomAttribute<TAttr>(info);
            return attribute;
        }

        /// <summary>
        ///     Determines if a custom attribute applied to a member of a type. Parameters specify
        ///     the member, and the type of the custom attribute to search for.
        /// </summary>
        /// <typeparam name="T">
        ///      The type, or a base type, of the custom attribute to search for.
        /// </typeparam>
        /// <param name="info">
        ///     An object derived from the System.Reflection.MemberInfo class that describes
        ///     a constructor, event, field, method, or property member of a class.
        /// </param>
        /// <returns>
        ///     <value>TRUE</value> if <paramref name="info"/> has some attributes attached to it.
        ///     Overwise returns <value>FALSE</value>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="info"/> is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        ///     <paramref name="info"/> is not a constructor, method, property, event, type, or field.
        /// </exception>
        /// <exception cref="System.TypeLoadException">
        ///     A custom attribute type cannot be loaded.
        /// </exception>
        public static bool HasAttribute<TAttr>(this MemberInfo info)
            where TAttr : Attribute
        {
            var attributes = ZAttribute.GetCustomAttributes<TAttr>(info);
            return attributes.Length > 0;
        }
    }
}
