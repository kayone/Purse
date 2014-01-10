using System;
using System.Collections.Generic;

namespace Purse
{
    public interface IPurse<TKey, TValue>
    {
        IEnumerable<TValue> Values { get; }
        void Set(TKey key, TValue value, TimeSpan? lifetime = null);
        TValue Get(TKey key, Func<TValue> function, TimeSpan? lifeTime = null);
        TValue Find(TKey key);

        int Count { get; }
        void Purge();
        void Remove(TKey key);
    }

}