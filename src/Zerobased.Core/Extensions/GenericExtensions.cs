using JetBrains.Annotations;

namespace Zerobased.Extensions
{
    /// <summary>
    ///     Generic extension methods.
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// Returns empty string for NULL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        [NotNull]
        public static string ToStringSafe<T>(this T obj) where T : class
        {
            return obj == null
                ? string.Empty
                : obj.ToString();
        }

        /// <summary>
        /// Returns empty string for NULL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        [NotNull]
        public static string ToStringSafe<T>(this T? obj) where T : struct
        {
            return obj == null
                ? string.Empty
                : obj.ToString();
        }
    }
}
