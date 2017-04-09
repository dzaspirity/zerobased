using System;
using System.Collections.Generic;

namespace Zerobased.Extensions
{
    /// <summary>
    ///     Extension methods for the <see cref="ICollection{T}"/>
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        ///     Determines the index of a item that matches with <paramref name="predicate"/> in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="collection">Source collection for search in.</param>
        /// <param name="predicate">Condition of matching.</param>
        /// <returns>Index of a item that matchs with <paramref name="predicate"/></returns>
        public static int IndexOf<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            int index = -1;

            foreach (var item in collection)
            {
                index++;
                if (predicate(item))
                {
                    return index;
                }
            }
            return -1;
        }

        /// <summary>
        ///     Determines whether the specified collection is NULL or contains no elements.
        /// </summary>
        /// <typeparam name="T">Collection items type.</typeparam>
        /// <param name="collection">Collection to test.</param>
        /// <returns><value>TRUE</value> if the value parameter is null or an empty collection; otherwise, <value>FALSE</value>.</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        /// <summary>
        ///     Adds the elements of the <paramref name="items"/> to the end of the <paramref name="collection"/>
        /// </summary>
        /// <typeparam name="TCollection">Type of collection.</typeparam>
        /// <typeparam name="TItem">Type of item.</typeparam>
        ///     <param name="collection">
        ///     Collection to add items. This collection will be modified. New items will be added to the end of the collection.
        /// </param>
        /// <param name="items">
        ///     The collection whose elements should be added to the end of the <paramref name="collection"/>.
        ///     The collection itself cannot be null, but it can contain elements that are null,
        ///     if type <typeparamref name="TItem"/> is a reference type.
        /// </param>
        /// <returns>Instance of source collection.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="collection"/> is null or <paramref name="items"/> is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     <paramref name="collection"/> is read-only.
        /// </exception>
        public static TCollection AddRange<TCollection, TItem>(this TCollection collection, IEnumerable<TItem> items)
            where TCollection : class, ICollection<TItem>
        {
            collection = Check.NotNull(collection, nameof(collection));
            items = Check.NotNull(items, nameof(items));
            List<TItem> list = collection as List<TItem>;

            if (list != null)
            {
                list.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    collection.Add(item);
                }
            }

            return collection;
        }

        /// <summary>
        ///     Removes the first occurrence of a every object of <paramref name="items"/> from the <paramref name="collection"/>.
        /// </summary>
        /// <typeparam name="TCollection">Type of collection.</typeparam>
        /// <typeparam name="TItem">Type of item.</typeparam>
        /// <param name="collection">
        ///     Collection to remove items. This collection will be modified. Items will be removed from the collection.
        /// </param>
        /// <param name="items">
        ///     The objects to remove from the <paramref name="collection"/>.
        /// </param>
        /// <returns>Instance of source collection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is null or <paramref name="items"/> is null.</exception>
        /// <exception cref="NotSupportedException"><paramref name="collection"/> is read-only.</exception>
        public static TCollection RemoveRange<TCollection, TItem>(this TCollection collection, IEnumerable<TItem> items)
            where TCollection : class, ICollection<TItem>
        {
            collection = Check.NotNull(collection, nameof(collection));
            items = Check.NotNull(items, nameof(items));
            foreach (var item in items)
            {
                collection.Remove(item);
            }

            return collection;
        }

        /// <summary>
        ///     Adds string value to the end of the <paramref name="collection"/> by processing a composite format string.
        /// </summary>
        /// <typeparam name="TCollection">Type of collection.</typeparam>
        /// <param name="collection">
        ///     Collection to add items. This collection will be modified. New items will be added to the end of the collection.
        /// </param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to format.</param>
        /// <returns>Instance of source collection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is null.</exception>
        /// <exception cref="NotSupportedException"><paramref name="collection"/> is read-only.</exception>
        /// <exception cref="ArgumentException"><paramref name="format"/> is null or empty string.</exception>
        /// <exception cref="FormatException">
        ///     <paramref name="format"/> is invalid.-or- The index of a format item is less than zero, or greater
        ///     than or equal to the length of the <paramref name="args"/> array.
        /// </exception>
        [StringFormatMethod("format")]
        public static TCollection AddFormat<TCollection>(this TCollection collection, string format, params object[] args)
            where TCollection : class, ICollection<string>
        {
            collection = Check.NotNull(collection, nameof(collection));
            format = Check.NotNullOrEmpty(format, nameof(format));

            collection.Add(format.FormatWith(args));
            return collection;
        }
    }
}
