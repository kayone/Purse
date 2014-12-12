using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Purse.Storage
{
    internal class MemoryStorage<TKey, TValue> : ICacheStorage<TKey, TValue>
    {
        ConcurrentDictionary<TKey, TValue> _dictionary = new ConcurrentDictionary<TKey, TValue>();

        public void Set(TKey key, TValue cacheItem)
        {
            _dictionary[key] = cacheItem;
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public void TryRemove(TKey key, out TValue value)
        {
            _dictionary.TryRemove(key, out value);
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public IEnumerable<TValue> Values
        {
            get { return _dictionary.Values; }
        }


        public Dictionary<TKey, TValue> Dictionary
        {
            get { return new Dictionary<TKey, TValue>(_dictionary); }
        }

        public void Purge()
        {
            var oldDictionary = _dictionary;
            _dictionary = new ConcurrentDictionary<TKey, TValue>();
            oldDictionary.Clear();
        }
    }
}