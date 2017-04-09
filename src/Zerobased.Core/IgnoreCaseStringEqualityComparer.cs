using System;
using System.Collections.Generic;

namespace Zerobased
{
    /// <summary>
    ///     Implementation of <see cref="IEqualityComparer{T}"/> to compare strings with ignoring case
    /// </summary>
    public class IgnoreCaseStringEqualityComparer : IEqualityComparer<string>
    {
        private static readonly Lazy<IgnoreCaseStringEqualityComparer> _instance = new Lazy<IgnoreCaseStringEqualityComparer>();

        /// <summary>
        ///     Singleton instance of <see cref="IgnoreCaseStringEqualityComparer"/>
        /// </summary>
        public static IgnoreCaseStringEqualityComparer Instance => _instance.Value;

        /// <summary>
        ///     Determines whether the specified strings are equal by <see cref="StringComparison.CurrentCultureIgnoreCase"/>
        /// </summary>
        /// <param name="x">The first string to compare</param>
        /// <param name="y">The second string to compare</param>
        /// <returns></returns>
        public bool Equals(string x, string y)
        {
            return x?.Equals(y, StringComparison.CurrentCultureIgnoreCase) ?? y == null;
        }

        /// <summary>
        ///     Returns a hash code for the specified string
        /// </summary>
        /// <param name="obj">The <see cref="String"/> for which a hash code is to be returned</param>
        /// <returns>A hash code for the specified string</returns>
        public int GetHashCode(string obj)
        {
            return obj?.ToUpper().GetHashCode() ?? int.MinValue;
        }
    }
}
