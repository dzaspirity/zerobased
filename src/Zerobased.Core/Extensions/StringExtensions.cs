using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Zerobased.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Parses int from string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fallbackValue"></param>
        /// <returns>Integer value, if NaN returns <paramref name="fallbackValue"/></returns>
        public static int ToInt(this string str, int fallbackValue) => int.TryParse(str, out int @int) ? @int : fallbackValue;

        /// <summary>
        /// Parse int from string
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Integer value, if NaN returns 0</returns>
        public static int ToInt(this string str) => str.ToInt(default(int));

        /// <summary>
        /// Parse int from string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fallbackValue"></param>
        /// <returns>Integer value, if NaN returns <paramref name="fallbackValue"/></returns>
        public static long ToLong(this string str, long fallbackValue = default(long)) => long.TryParse(str, out long @long) ? @long : fallbackValue;

        public static double ToDouble(this string str, double fallbackValue, bool tryParseFraction = true)
        {
            double d;
            if (!double.TryParse(str, out d) && tryParseFraction && !Fraction.TryParse(str, out d))
                return fallbackValue;
            return d;
        }

        public static double ToDouble(this string str, bool tryParseFraction = true) => str.ToDouble(default(double), tryParseFraction);

        public static string Wrap(this string str, string startWrapper, string endWrapper = null)
        {
            Check.NotNullOrEmpty(startWrapper, nameof(startWrapper));

            return string.Concat(startWrapper, str, (endWrapper ?? startWrapper));
        }

        public static string WrapByTag(this string source, string tag) => "<{0}>{1}</{0}>".FormatWith(tag, source);

        public static string WrapByTag(this string source, string tag, IDictionary<string, string> attrs)
        {
            var sb = new StringBuilder();
            sb.Append("<" + tag);
            foreach (var attr in attrs)
            {
                sb.AppendFormat(" {0}='{1}'", attr.Key, attr.Value);
            }
            sb.AppendFormat(">{0}</{1}>", source, tag);
            return sb.ToString();
        }

        public static bool Contains(this string source, string value, bool ignoreCase)
        {
            if (ignoreCase)
            {
                source = source.ToUpper();
                value = value.ToUpper();
            }
            return source.Contains(value);
        }

        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        public static string[] Split(this string str, string delimeter, StringSplitOptions options = StringSplitOptions.None) => str.Split(new[] { delimeter }, options);

        public static string[] Split(this string str, char delimeter, StringSplitOptions options = StringSplitOptions.None) => str.Split(new[] { delimeter }, options);

        [StringFormatMethod("format")]
        public static string FormatWith(this string format, params object[] args) => string.Format(format, args);

        [StringFormatMethod("format")]
        public static string FormatWith(this string format, object arg0) => string.Format(format, arg0);

        [StringFormatMethod("format")]
        public static string FormatWith(this string format, object arg0, object arg1) => string.Format(format, arg0, arg1);

        [StringFormatMethod("format")]
        public static string FormatWith(this string format, object arg0, object arg1, object arg2) => string.Format(format, arg0, arg1, arg2);

        public static string TrimSafe(this string str) => str?.Trim() ?? string.Empty;

        /// <summary>
        ///     Replace <paramref name="value"/> with empty string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Remove(this string str, string value) => str.Replace(value, string.Empty);

        /// <summary>
        /// Converts string to bytes array without using Encoding.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <returns>Bytes array representation of string.</returns>
        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string FirstLetterToLower(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }
            if (str.Length > 1)
            {
                return char.ToLower(str[0]) + str.Substring(1);
            }
            return str.ToLower();
        }

        public static bool StartsWithLetter(this string str) => !str.IsNullOrEmpty() && Char.IsLetter(str[0]);

        public static string SubstringSafe(this string str, int startIndex, int length)
        {
            if (str.IsNullOrEmpty() || startIndex >= str.Length || length <= 0)
            {
                return string.Empty;
            }
            length = Math.Min(length, str.Length - startIndex);
            return str.Substring(startIndex, length);
        }
    }
}
