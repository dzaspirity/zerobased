using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zerobased.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="Exception"/>
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     Aggregates <paramref name="exception"/> and inner exceptions to <typeparamref name="TAccumulate"/> object.
        /// </summary>
        /// <typeparam name="TAccumulate"></typeparam>
        /// <param name="exception"></param>
        /// <param name="seed"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TAccumulate Aggregate<TAccumulate>(this Exception exception, TAccumulate seed, Func<TAccumulate, Exception, TAccumulate> func)
        {
            return exception.Expand().Aggregate(seed, func);
        }

        /// <summary>
        ///     Expands <paramref name="exception"/> and inner exceptions to enumerable.
        /// </summary>
        /// <param name="exception">Exception to expand</param>
        /// <returns></returns>
        public static IEnumerable<Exception> Expand(this Exception exception)
        {
            Exception curExc = exception;

            while (curExc != null)
            {
                yield return curExc;
                curExc = curExc.InnerException;
            }
        }

        /// <summary>
        ///     Expands <paramref name="exception"/> and inner exceptions to string.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="format">
        ///     {0} - GetType().FullName, {1} - Message, {2} - StackTrace
        /// </param>
        /// <returns></returns>
        public static string ExpandToString(this Exception exception, string format = null)
        {
            if (format.IsNullOrWhiteSpace())
            {
                format = $"{{0}}: {{1}}{Environment.NewLine}Stack trace:{Environment.NewLine}{{2}}";
            }
            StringBuilder builder = exception
                .Aggregate(new StringBuilder(), (sb, e) => sb.AppendFormat(format, e.GetType().FullName, e.Message, e.StackTrace));
            return builder.ToString();
        }
    }
}
