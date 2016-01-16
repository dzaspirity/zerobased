using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Zerobased.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Execute action for each element in sequence.
        /// </summary>
        /// <typeparam name="T">Type of sequence elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="action">Action for executing.</param>
        public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Execute action for each element in sequence.
        /// </summary>
        /// <typeparam name="T">Type of sequence elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="action">Action for executing.</param>
        public static void ForeachSafe<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (T item in source)
            {
                try
                {
                    action(item);
                }
                catch
                {
                    // do nothing
                }
            }
        }

        /// <summary>
        /// Execute action for each element in sequence.
        /// </summary>
        /// <typeparam name="T">Type of sequence elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="action">
        ///     Action for executing. The second parameter
        ///     of the function represents the index of the source element.
        /// </param>
        public static void Foreach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            int index = 0;
            foreach (T item in source)
            {
                action(item, index++);
            }
        }

        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (T item in source)
            {
                action(item);
                yield return item;
            }
        }

        public static IEnumerable<T> DoIf<T>(this IEnumerable<T> source, Predicate<T> checker, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (T item in source)
            {
                if (checker(item))
                {
                    action(item);
                }

                yield return item;
            }
        }

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source)
        {
            return source.WhereNotNull(item => item);
        }

        public static IEnumerable<TSource> WhereNotNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Where(item => selector(item) != null);
        }

        public static IEnumerable<string> WhereNotNullOrEmpty(this IEnumerable<string> source)
        {
            return source.Where(item => !item.IsNullOrEmpty());
        }

        public static IEnumerable<TSource> WhereNotNullOrEmpty<TSource>(this IEnumerable<TSource> source, Func<TSource, string> selector)
        {
            return source.Where(item => !selector(item).IsNullOrEmpty());
        }

        public static IOrderedEnumerable<T> Sort<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(item => item);
        }

        public static IOrderedEnumerable<T> SortDescending<T>(this IEnumerable<T> source)
        {
            return source.OrderByDescending(item => item);
        }

        /// <summary>
        ///     Groups the elements of a sequence according to a specified key selector function 
        ///     and convert it into System.Collections.Generic.Dictionary[TKey, T]
        ///     with key got from <paramref name="keySelector"/> function.
        /// </summary>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable[T] whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns>
        ///     System.Collections.Generic.Dictionary[TKey, T] where keys are results
        ///     of <paramref name="keySelector"/> functions and values are lists of
        ///     grouped by <paramref name="keySelector"/> elements.
        /// </returns>
        public static Dictionary<TKey, List<TSource>> ToGroupedDictionary<TKey, TSource>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var dict = source
                .GroupBy(keySelector)
                .ToDictionary(group => group.Key, group => group.ToList());
            return dict;
        }

        /// <summary>
        ///     Sorts the elements of a sequence in ascending order according to a property path.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="propertyPath">Property path to sorting.</param>
        /// <param name="desc">Sort in descending order if <value>TRUE</value>, overwise in ascending order.</param>
        /// <returns>
        ///     An System.Linq.IOrderedEnumerable[T] whose elements are sorted according to a property path.
        /// </returns>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyPath, bool desc = false)
        {
            ParameterExpression arg = Expression.Parameter(typeof(T), "x");
            Expression expr = ZExpression.PropertyPath(arg, propertyPath);
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), expr.Type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            object result = (desc ? OrderByDescendingMethod : OrderByMethod)
                    .MakeGenericMethod(typeof(T), expr.Type)
                    .Invoke(null, new object[] { source, lambda.Compile() });
            return (IOrderedEnumerable<T>)result;
        }

        /// <summary>
        ///     Performs a subsequent ordering of the elements in a sequence in ascending
        ///     order according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">An System.Linq.IOrderedEnumerable[T] that contains elements to sort.</param>
        /// <param name="propertyPath">Property path to sorting.</param>
        /// <param name="desc">Sort in descending order if <value>TRUE</value>, overwise in ascending order.</param>
        /// <returns>
        ///     An System.Linq.IOrderedEnumerable[T] whose elements are sorted according to a property path.
        /// </returns>
        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source, string propertyPath, bool desc = false)
        {
            ParameterExpression arg = Expression.Parameter(typeof(T), "x");
            Expression expr = ZExpression.PropertyPath(arg, propertyPath);
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), expr.Type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            object result = (desc ? ThenByDescendingMethod : ThenByMethod)
                    .MakeGenericMethod(typeof(T), expr.Type)
                    .Invoke(null, new object[] { source, lambda.Compile() });
            return (IOrderedEnumerable<T>)result;
        }

        public static TSource MinBy<TSource, TProp>(this IEnumerable<TSource> source, Func<TSource, TProp> selector)
            where TProp : IComparable<TProp>
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(selector, nameof(selector));

            var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                throw new InvalidOperationException("The source sequence is empty");
            }

            var min = enumerator.Current;
            var minValue = selector(enumerator.Current);

            while (enumerator.MoveNext())
            {
                var value = selector(enumerator.Current);

                if (minValue.CompareTo(value) < 0)
                {
                    minValue = value;
                    min = enumerator.Current;
                }
            }


            return min;
        }

        public static TSource MaxBy<TSource, TProp>(this IEnumerable<TSource> source, Func<TSource, TProp> selector)
            where TProp : IComparable<TProp>
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(selector, nameof(selector));

            var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                throw new InvalidOperationException("The source sequence is empty");
            }

            var max = enumerator.Current;
            var maxValue = selector(enumerator.Current);

            while (enumerator.MoveNext())
            {
                var value = selector(enumerator.Current);

                if (maxValue.CompareTo(value) > 0)
                {
                    maxValue = value;
                    max = enumerator.Current;
                }
            }

            return max;
        }

        public static bool MinMax<T>(this IEnumerable<T> source, out T min, out T max)
        {
            Comparer<T> comparer = Comparer<T>.Default;
            var enumerator = source.GetEnumerator();

            if (enumerator.MoveNext())
            {
                min = max = enumerator.Current;
            }
            else
            {
                min = max = default(T);
                return false;
            }

            while (enumerator.MoveNext())
            {
                if (comparer.Compare(enumerator.Current, min) < 0)
                {
                    min = enumerator.Current;
                }
                else if (comparer.Compare(enumerator.Current, max) > 0)
                {
                    max = enumerator.Current;
                }
            }

            return true;
        }

        /// <summary>
        ///     Concatenates the members of a collection, using the specified separator between each member.
        /// </summary>
        /// <typeparam name="T">The type of the members of <paramref name="values"/>.</typeparam>
        /// <param name="values">A sequence that contains the objects to concatenate.</param>
        /// <param name="separator">The string to use as a separator.</param>
        /// <returns>
        ///     A string that consists of the members of values delimited by the separator
        ///     string. If values has no members, the method returns System.String.Empty.
        /// </returns>
        public static string Join<T>(this IEnumerable<T> values, string separator)
        {
            return string.Join(separator, values);
        }

        /// <summary>
        ///     Concatenates the members of a collection, using the specified separator between each member.
        ///     <paramref name="format"/> applied for each element before concatenating.
        /// </summary>
        /// <typeparam name="T">The type of the members of <paramref name="values"/>.</typeparam>
        /// <param name="values">A sequence that contains the objects to concatenate.</param>
        /// <param name="separator">The string to use as a separator.</param>
        /// <param name="format">Format applied for each element before concatenating.</param>
        /// <returns>
        ///     A string that consists of the members of values delimited by the separator
        ///     string. If values has no members, the method returns System.String.Empty.
        /// </returns>
        public static string Join<T>(this IEnumerable<T> values, string separator, string format)
        {
            return string.Join(separator, values.Select(i => format.FormatWith(i)));
        }

        /// <summary>
        ///     Concatenates the members of a collection, using the specified separator between each member.
        ///     <paramref name="toString"/> converts values to string before concatenating.
        /// </summary>
        /// <typeparam name="T">The type of the members of <paramref name="values"/>.</typeparam>
        /// <param name="values">A sequence that contains the objects to concatenate.</param>
        /// <param name="separator">The string to use as a separator.</param>
        /// <param name="toString">Converts values to string before concatenating. </param>
        /// <returns>
        ///     A string that consists of the members of values delimited by the separator
        ///     string. If values has no members, the method returns System.String.Empty.
        /// </returns>
        public static string Join<T>(this IEnumerable<T> values, string separator, Func<T, string> toString)
        {
            return string.Join(separator, values.Select(toString));
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        public static HashSet<TResult> ToHashSet<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return new HashSet<TResult>(source.Select(selector));
        }

        public static IEnumerable<T> Expand<T>(this IEnumerable<IEnumerable<T>> values)
        {
            return values.SelectMany(items => items);
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> values, int count)
        {
            List<T> buffer = new List<T>(count);
            var enumerator = values.GetEnumerator();

            while (enumerator.MoveNext())
            {
                buffer.Add(enumerator.Current);

                if (buffer.Count == count)
                {
                    yield return buffer;
                    buffer = new List<T>(count);
                }
            }

            if (buffer.Count > 0)
            {
                yield return buffer;
            }
        }


        private static readonly MethodInfo OrderByDescendingMethod = typeof(Enumerable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(method => method.Name == "OrderByDescending"
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2);

        private static readonly MethodInfo OrderByMethod = typeof(Enumerable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(method => method.Name == "OrderBy"
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2);

        private static readonly MethodInfo ThenByDescendingMethod = typeof(Enumerable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(method => method.Name == "ThenByDescending"
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2);

        private static readonly MethodInfo ThenByMethod = typeof(Enumerable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(method => method.Name == "ThenBy"
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2);
    }
}
