using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Purse.Storage
{
    public class MemoryStorage<TKey, TValue> : IPurseStorage<TKey, TValue>
    {
        ConcurrentDictionary<TKey, TValue> _dictionary = new ConcurrentDictionary<TKey, TValue>();

        public void Set(TKey key, TValue cacheItem)
        {
            _dictionary[key] = cacheItem;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return TryGetValue(key, out value);
        }

        public void TryRemove(TKey key, out TValue value)
        {
            TryRemove(key, out value);
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public IEnumerable<TValue> Values
        {
            get { return _dictionary.Values; }
        }

        public void Purge()
        {
            var oldDictionary = _dictionary;
            _dictionary = new ConcurrentDictionary<TKey, TValue>();
            oldDictionary.Clear();
        }
    }
}