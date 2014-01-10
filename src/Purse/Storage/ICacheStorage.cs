using System.Collections.Generic;

namespace Purse.Storage
{
    public interface ICacheStorage<TKey, TValue>
    {
        void Set(TKey key, TValue cacheItem);
        bool TryGetValue(TKey key, out TValue value);
        void TryRemove(TKey key, out TValue value);
        int Count { get; }
        IEnumerable<TValue> Values { get; }
        void Purge();
    }
}