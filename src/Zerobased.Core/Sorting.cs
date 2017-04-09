using System;
using System.Collections.Generic;
using Zerobased.Extensions;

namespace Zerobased
{
    /// <summary>
    ///     Presentation of sorting
    /// </summary>
    public class Sorting
    {
        private static char[] DefaultDelimiters = new[] { ',', ' ', ';' };

        /// <summary>
        ///     Property of sorting
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        ///     Direction of sorting
        /// </summary>
        public SortDirection Direction { get; set; } = SortDirection.Asc;

        /// <summary>
        ///     Shows if sorting direction is <see cref="SortDirection.Desc"/>
        /// </summary>
        public bool IsDescending
        {
            get => Direction == SortDirection.Desc;
            set => Direction = value ? SortDirection.Desc : SortDirection.Asc;
        }

        /// <summary>
        ///     Returns a string that represents the current sorting.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            char? prefix = IsDescending ? '-' : (char?)null;
            return $"{prefix}{PropertyName}";
        }

        /// <summary>
        ///     Converts the string representation of a sorting to its object equivalent.
        /// </summary>
        /// <param name="str">A string that contains a sorting to convert.</param>
        /// <returns>
        ///     <see cref="Sorting"/> object that is equivalent to the string value
        ///     specified in <paramref name="str"/>.
        /// </returns>
        public static Sorting Parse(string str)
        {
            var sd = new Sorting { PropertyName = str };

            if (str[0] == '+' || str[0] == '-')
            {
                sd.IsDescending = str[0] == '-';
                sd.PropertyName = str.Substring(1);
            }

            return sd;
        }

        /// <summary>
        ///     Converts the string representation of a list of sorting to <see cref="IEnumerable{Sorting}"/>.
        /// </summary>
        /// <param name="str">A string that contains a sorting to convert.</param>
        /// <param name="delimiter">
        ///     List of character is used as delimiters between sorting properties.
        /// </param>
        /// <returns>
        ///     <see cref="IEnumerable{Sorting}"/> object that is equivalent to the string value
        ///     specified in <paramref name="str"/>.
        /// </returns>
        public static IEnumerable<Sorting> ParseMany(string str, char delimiter)
        {
            return ParseMany(str, new[] { delimiter });
        }

        /// <summary>
        ///     Converts the string representation of a list of sorting to <see cref="IEnumerable{Sorting}"/>.
        ///     If <paramref name="delimiters"/> are no specified, comma (','), single space (' ')
        ///     and semicolon (';') will be used.
        /// </summary>
        /// <param name="str">A string that contains a sorting to convert.</param>
        /// <param name="delimiters">
        ///     List of characters are used as delimiters between sorting properties.
        ///     If not specified, then comma (','), single space (' ') and semicolon (';') will be used.
        /// </param>
        /// <returns>
        ///     <see cref="IEnumerable{Sorting}"/> object that is equivalent to the string value
        ///     specified in <paramref name="str"/>.
        /// </returns>
        public static IEnumerable<Sorting> ParseMany(string str, char[] delimiters = null)
        {
            if (!str.IsNullOrWhiteSpace())
            {
                foreach (string part in str.Split(delimiters ?? DefaultDelimiters, StringSplitOptions.RemoveEmptyEntries))
                {
                    var sorting = Parse(part);
                    yield return sorting;
                }
            }
        }

        /// <summary>
        ///     Sorting direction
        /// </summary>
        public enum SortDirection
        {
            /// <summary>
            ///     Ascending
            /// </summary>
            Asc = 0,

            /// <summary>
            ///     Descending
            /// </summary>
            Desc = 1
        }
    }
}
