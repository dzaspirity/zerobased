using System.Collections.Generic;

namespace Zerobased
{
    /// <summary>
    ///     Object can be represented as dictionary
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary</typeparam>
    public interface IDictionaryable<TKey,TValue>
    {
        /// <summary>
        ///     Returns a Dictionary which represents the object instance
        /// </summary>
        /// <returns>Dictionary which represents the object instance</returns>
        IDictionary<TKey, TValue> ToDictionary();
    }
}
