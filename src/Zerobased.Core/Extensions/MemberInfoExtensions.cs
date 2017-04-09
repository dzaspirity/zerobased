using System;
using System.Reflection;

namespace Zerobased.Extensions
{
    /// <summary>
    ///     Additional extensions for <see cref="MemberInfo"/>
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        ///     Generic wrapper over <see cref="CustomAttributeExtensions.IsDefined(MemberInfo, Type, bool)"/>
        /// </summary>
        /// <typeparam name="TAttr">The type of the attribute to search for</typeparam>
        /// <param name="info">The member to inspect</param>
        /// <param name="inherit">true to inspect the ancestors of element; otherwise, false</param>
        /// <returns>true if an attribute of the specified type is applied to element; otherwise, false</returns>
        /// <exception cref="ArgumentNullException"><paramref name="info"/> is null</exception>
        /// <exception cref="NotSupportedException"><paramref name="info"/> is not a constructor, method, property, event, type, or field</exception>
        public static bool IsDefined<TAttr>(this MemberInfo info, bool inherit = false)
            where TAttr : Attribute
        {
            var attributes = ZAttribute.GetCustomAttributes<TAttr>(info);
            return attributes.Length > 0;
        }
    }
}
