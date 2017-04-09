using System;
using System.Reflection;

namespace Zerobased
{
    /// <summary>
    /// Class represent methods that wrap some of System.Attribute static methods to their generic versions.
    /// </summary>
    public static class ZAttribute
    {
        /// <summary>
        ///     Generic wrapper for System.Attribute.GetCustomAttribute method.
        ///     Retrieves a custom attribute applied to a member of a type. Parameters specify
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
        ///     A reference to the single custom attribute of type attributeType that is
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
        public static T GetCustomAttribute<T>(MemberInfo info) where T : Attribute
        {
            Attribute attribute = Attribute.GetCustomAttribute(info, typeof(T));
            return attribute as T;
        }

        /// <summary>
        ///     Generic wrapper for System.Attribute.GetCustomAttributes method.
        ///     Retrieves an array of the custom attributes applied to a member of a type.
        ///     Parameters specify the member, and the type of the custom attribute to search for.
        /// </summary>
        /// <typeparam name="T">
        ///      The type, or a base type, of the custom attribute to search for.
        /// </typeparam>
        /// <param name="info">
        ///     An object derived from the System.Reflection.MemberInfo class that describes
        ///     a constructor, event, field, method, or property member of a class.
        /// </param>
        /// <returns>
        ///     An <typeparamref name="T"/> array that contains the custom attributes of type type
        ///     applied to element, or an empty array if no such custom attributes exist.
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
        public static T[] GetCustomAttributes<T>(MemberInfo info) where T : Attribute
        {
            Attribute[] attributes = Attribute.GetCustomAttributes(info, typeof(T));
            return attributes as T[];
        }
    }
}
