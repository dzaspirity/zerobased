using System.Collections.Generic;

namespace Zerobased
{
    public interface IDictionaryable<TKey,TValue>
    {
        IDictionary<TKey, TValue> ToDictionary();
    }
}
