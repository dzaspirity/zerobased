using System;
using System.Collections.Generic;

namespace Zerobased.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="IDictionary{TKey, TValue}"/> interface.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        ///     Returns stored value if <paramref name="key"/> exists in dictionary.
        ///     Otherwise adds <paramref name="valueToAdd"/> to dictionary and returns it.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of values in the dictionary.
        /// </typeparam>
        /// <param name="dictionary">
        ///     Dictionary instance from with getting value.
        /// </param>
        /// <param name="key">
        ///     The key whose value to get.
        /// </param>
        /// <param name="valueToAdd">
        ///     Value to add and return in case if <paramref name="key"/> is missing.
        ///     If not specified then default value for the <typeparamref name="TValue"/> will be used.
        /// </param>
        /// <returns>
        ///     The element with the specified <paramref name="key"/> or <paramref name="valueToAdd"/>
        ///     if key does not exists in dictionary.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key"/> is null or
        ///     <paramref name="dictionary"/> is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     <paramref name="dictionary"/> is read-only.
        /// </exception>
        public static TValue GetValueOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue valueToAdd = default(TValue))
        {
            return dictionary.GetValueOrAdd(key, k => valueToAdd);
        }

        /// <summary>
        ///     Returns stored value if <paramref name="key"/> exists in dictionary.
        ///     Otherwise adds value provided by <paramref name="valueToAddProvider"/> to dictionary and returns it.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of values in the dictionary.
        /// </typeparam>
        /// <param name="dictionary">
        ///     Dictionary instance from with getting value.
        /// </param>
        /// <param name="key">
        ///     The key whose value to get.
        /// </param>
        /// <param name="valueToAddProvider">
        ///     Function to provide value to add and return in case if <paramref name="key"/> is missing.
        /// </param>
        /// <returns>
        ///     The element with the specified <paramref name="key"/> or <paramref name="valueToAddProvider"/>
        ///     if key does not exists in dictionary.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key"/> is null or
        ///     <paramref name="dictionary"/> is null or
        ///     <paramref name="valueToAddProvider"/> is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     <paramref name="dictionary"/> is read-only.
        /// </exception>
        public static TValue GetValueOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueToAddProvider)
        {
            valueToAddProvider = Check.NotNull(valueToAddProvider, nameof(valueToAddProvider));

            return dictionary.GetValueOrAdd(key, k => valueToAddProvider());
        }

        /// <summary>
        ///     Returns stored value if <paramref name="key"/> exists in dictionary.
        ///     Otherwise adds value provided by <paramref name="valueToAddProvider"/> to dictionary and returns it.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of values in the dictionary.
        /// </typeparam>
        /// <param name="dictionary">
        ///     Dictionary instance from with getting value.
        /// </param>
        /// <param name="key">
        ///     The key whose value to get.
        /// </param>
        /// <param name="valueToAddProvider">
        ///     Function to provide value to add and return in case if <paramref name="key"/> is missing.
        /// </param>
        /// <returns>
        ///     The element with the specified <paramref name="key"/> or <paramref name="valueToAddProvider"/>
        ///     if key does not exists in dictionary.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key"/> is null or
        ///     <paramref name="dictionary"/> is null or
        ///     <paramref name="valueToAddProvider"/> is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     <paramref name="dictionary"/> is read-only.
        /// </exception>
        public static TValue GetValueOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueToAddProvider)
        {
            dictionary = Check.NotNull(dictionary, nameof(dictionary));
            valueToAddProvider = Check.NotNull(valueToAddProvider, nameof(valueToAddProvider));

            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, valueToAddProvider(key));
            }
            return dictionary[key];
        }

        /// <summary>
        ///     Returns stored value if <paramref name="key"/> exists in dictionary.
        ///     Otherwise returns <paramref name="fallbackValue"/>.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of values in the dictionary.
        /// </typeparam>
        /// <param name="dictionary">
        ///     Dictionary instance from with getting value.
        /// </param>
        /// <param name="key">
        ///     The key whose value to get.
        /// </param>
        /// <param name="fallbackValue">
        ///     Value to return in case if <paramref name="key"/> is missing.
        ///     If not specified then default value for the <typeparamref name="TValue"/> will be used.
        /// </param>
        /// <returns>
        ///     The element with the specified <paramref name="key"/> or <paramref name="fallbackValue"/>
        ///     if key does not exists in dictionary.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key"/> is null or <paramref name="dictionary"/> is null.
        /// </exception>
        public static TValue GetValueSafe<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue fallbackValue = default(TValue))
        {
            return dictionary.GetValueSafe(key, k => fallbackValue);
        }

        /// <summary>
        ///     Returns stored value if <paramref name="key"/> exists in dictionary.
        ///     Otherwise returns value provided by <paramref name="fallbackValueProvider"/>.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of values in the dictionary.
        /// </typeparam>
        /// <param name="dictionary">
        ///     Dictionary instance to get value.
        /// </param>
        /// <param name="key">
        ///     The key whose value to get.
        /// </param>
        /// <param name="fallbackValueProvider">
        ///     Function to provide value to return in case if <paramref name="key"/> is missing.
        /// </param>
        /// <returns>
        ///     The element with the specified <paramref name="key"/> or value
        ///     provided by <paramref name="fallbackValueProvider"/>
        ///     if <paramref name="key"/> does not exists in dictionary.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key"/> is null or
        ///     <paramref name="dictionary"/> is null or
        ///     <paramref name="fallbackValueProvider"/> is null.
        /// </exception>
        public static TValue GetValueSafe<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> fallbackValueProvider)
        {
            fallbackValueProvider = Check.NotNull(fallbackValueProvider, nameof(fallbackValueProvider));
            return dictionary.GetValueSafe(key, k => fallbackValueProvider());
        }

        /// <summary>
        ///     Returns stored value if <paramref name="key"/> exists in dictionary.
        ///     Otherwise returns value provided by <paramref name="fallbackValueProvider"/>.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of values in the dictionary.
        /// </typeparam>
        /// <param name="dictionary">
        ///     Dictionary instance to get value.
        /// </param>
        /// <param name="key">
        ///     The key whose value to get.
        /// </param>
        /// <param name="fallbackValueProvider">
        ///     Function to provide value to return in case if <paramref name="key"/> is missing.
        /// </param>
        /// <returns>
        ///     The element with the specified <paramref name="key"/> or value
        ///     provided by <paramref name="fallbackValueProvider"/>
        ///     if <paramref name="key"/> does not exists in dictionary.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="key"/> is null or
        ///     <paramref name="dictionary"/> is null or
        ///     <paramref name="fallbackValueProvider"/> is null.
        /// </exception>
        public static TValue GetValueSafe<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> fallbackValueProvider)
        {
            dictionary = Check.NotNull(dictionary, nameof(dictionary));
            fallbackValueProvider = Check.NotNull(fallbackValueProvider, nameof(fallbackValueProvider));
            if (!dictionary.TryGetValue(key, out TValue value))
            {
                value = fallbackValueProvider(key);
            }

            return value;
        }
    }
}
