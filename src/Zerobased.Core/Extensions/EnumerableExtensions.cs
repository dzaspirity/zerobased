using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Zerobased.Extensions
{
    /// <summary>
    ///     Extension method for <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Executes action for each element in sequence.
        /// </summary>
        /// <typeparam name="T">Type of sequence elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="action">Action to execute.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source"/> is null or
        ///     <paramref name="action"/> is null.
        /// </exception>
        public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
        {
            source = Check.NotNull(source, nameof(source));
            action = Check.NotNull(action, nameof(action));
            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        ///     Executes action for each element in sequence. It will silently ignore
        ///     any exception accrued during action call.
        /// </summary>
        /// <typeparam name="T">Type of sequence elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="action">Action to execute.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source"/> is null or
        ///     <paramref name="action"/> is null.
        /// </exception>
        public static void ForeachSafe<T>(this IEnumerable<T> source, Action<T> action)
        {
            source = Check.NotNull(source, nameof(source));
            action = Check.NotNull(action, nameof(action));
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
        ///     Executes action for each element in sequence.
        /// </summary>
        /// <typeparam name="T">Type of sequence elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="action">Action to execute.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source"/> is null or
        ///     <paramref name="action"/> is null.
        /// </exception>
        public static void Foreach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            source = Check.NotNull(source, nameof(source));
            action = Check.NotNull(action, nameof(action));
            int index = 0;
            foreach (T item in source)
            {
                action(item, index++);
            }
        }

        /// <summary>
        ///     Executes action for each element in sequence and returns original items.
        /// </summary>
        /// <typeparam name="T">Type of sequence elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="action">Action to execute.</param>
        /// <returns>Original sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source"/> is null or
        ///     <paramref name="action"/> is null.
        /// </exception>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            source = Check.NotNull(source, nameof(source));
            action = Check.NotNull(action, nameof(action));
            foreach (T item in source)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        ///     Executes action for those element in sequence which are satisfy <paramref name="checker"/>
        ///     and returns original sequence.
        /// </summary>
        /// <typeparam name="T">Type of sequence elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="checker"><see cref="Predicate{T}"/> to check if action should be applied for the current item.</param>
        /// <param name="action">Action to execute.</param>
        /// <returns>Original sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source"/> is null or
        ///     <paramref name="checker"/> is null or
        ///     <paramref name="action"/> is null.
        /// </exception>
        public static IEnumerable<T> DoIf<T>(this IEnumerable<T> source, Predicate<T> checker, Action<T> action)
        {
            source = Check.NotNull(source, nameof(source));
            action = Check.NotNull(action, nameof(action));
            checker = Check.NotNull(checker, nameof(checker));

            foreach (T item in source)
            {
                if (checker(item))
                {
                    action(item);
                }
                yield return item;
            }
        }

        /// <summary>
        ///     Filters sequence to return only non-null elements.
        /// </summary>
        /// <typeparam name="T">Type of sequence elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>
        ///     <see cref="IEnumerable{T}"/> that contains only non-null elements.
        /// </returns>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source) where T : class
        {
            return source.WhereNotNull(item => item);
        }

        /// <summary>
        ///     Filters sequence to return only elements with non-null value by specific selector.
        /// </summary>
        /// <typeparam name="TSource">Type of sequence elements.</typeparam>
        /// <typeparam name="TResult">Type of value to check.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Function to get value from element to check.</param>
        /// <returns>
        ///     <see cref="IEnumerable{TSource}"/> that contains only elements with non-null value by specific selector.
        /// </returns>
        public static IEnumerable<TSource> WhereNotNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
            where TResult : class
        {
            return source.Where(item => selector(item) != null);
        }

        /// <summary>
        ///     Filters sequence to return only strings which are not null and are not empty.
        /// </summary>
        /// <param name="source">Source sequence.</param>
        /// <returns>
        ///     <see cref="IEnumerable{String}"/> that contains only strings which are not null and are not empty.
        /// </returns>
        public static IEnumerable<string> WhereNotNullOrEmpty(this IEnumerable<string> source)
        {
            return source.Where(item => !item.IsNullOrEmpty());
        }

        /// <summary>
        ///     Filters sequence to return only elements with not null and not empty strings by specific selector.
        /// </summary>
        /// <typeparam name="TSource">Type of sequence elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Function to get string from element to check.</param>
        /// <returns>
        ///     <see cref="IEnumerable{TSource}"/> that contains only elements with non-null value by specific selector.
        /// </returns>
        public static IEnumerable<TSource> WhereNotNullOrEmpty<TSource>(this IEnumerable<TSource> source, Func<TSource, string> selector)
        {
            return source.Where(item => !selector(item).IsNullOrEmpty());
        }

        /// <summary>
        ///     Shortcut for source.OrderBy(item => item)
        /// </summary>
        public static IOrderedEnumerable<T> Sort<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(item => item);
        }

        /// <summary>
        ///     Shortcut for source.OrderByDescending(item => item)
        /// </summary>
        public static IOrderedEnumerable<T> SortDescending<T>(this IEnumerable<T> source)
        {
            return source.OrderByDescending(item => item);
        }

        /// <summary>
        ///     Groups the elements of a sequence according to a specified key selector function
        ///     and convert it into <see cref="Dictionary{TKey, TValue}"/>
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
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source"/> is null or
        ///     <paramref name="keySelector"/> is null.
        /// </exception>
        public static Dictionary<TKey, List<TSource>> ToGroupedDictionary<TKey, TSource>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            source = Check.NotNull(source, nameof(source));
            keySelector = Check.NotNull(keySelector, nameof(keySelector));
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
        ///     <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted according to a property path.
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
        ///     Performs a subsequent ordering of the elements in a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">An System.Linq.IOrderedEnumerable[T] that contains elements to sort.</param>
        /// <param name="propertyPath">Property path to sorting.</param>
        /// <param name="desc">Sort in descending order if <value>TRUE</value>, overwise in ascending order.</param>
        /// <returns>
        ///     <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted according to a property path.
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

        /// <summary>
        ///     Returns the element with minimum value by <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="source"/> is null or empty sequence.</exception>
        public static TSource MinBy<TSource, TProp>(this IEnumerable<TSource> source, Func<TSource, TProp> selector)
            where TProp : IComparable<TProp>
        {
            selector = Check.NotNull(selector, nameof(selector));
            IEnumerator<TSource> enumerator = Check.NotNullOrEmpty(source, nameof(source));
            TSource min = enumerator.Current;
            TProp minValue = selector(enumerator.Current);

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

        /// <summary>
        ///     Returns the element with maximum value by <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="source"/> is null or empty sequence.</exception>
        public static TSource MaxBy<TSource, TProp>(this IEnumerable<TSource> source, Func<TSource, TProp> selector)
            where TProp : IComparable<TProp>
        {
            selector = Check.NotNull(selector, nameof(selector));
            IEnumerator<TSource> enumerator = Check.NotNullOrEmpty(source, nameof(source));
            TSource max = enumerator.Current;
            TProp maxValue = selector(enumerator.Current);

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

        /// <summary>
        ///    Initializes a new instance of the <see cref="HashSet{T}"/> class
        ///    that uses the default equality comparer for the set type, contains elements copied
        ///    from the specified collection, and has sufficient capacity to accommodate the
        ///    number of elements copied.
        /// </summary>
        /// <typeparam name="T">The type of elements in the hash set.</typeparam>
        /// <param name="source">The collection whose elements are copied to the new set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
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
            .GetTypeInfo()
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(method => method.Name == "OrderByDescending"
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2);

        private static readonly MethodInfo OrderByMethod = typeof(Enumerable)
            .GetTypeInfo()
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(method => method.Name == "OrderBy"
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2);

        private static readonly MethodInfo ThenByDescendingMethod = typeof(Enumerable)
            .GetTypeInfo()
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(method => method.Name == "ThenByDescending"
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2);

        private static readonly MethodInfo ThenByMethod = typeof(Enumerable)
            .GetTypeInfo()
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(method => method.Name == "ThenBy"
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2);
    }
}
