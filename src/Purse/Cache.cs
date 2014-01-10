using System;
using System.Collections.Generic;
using System.Linq;
using Purse.Storage;

namespace Purse
{
    public class Cache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly ICacheStorage<TKey, CacheItem<TValue>> _store;

        public Cache()
            : this(new MemoryStorage<TKey, CacheItem<TValue>>())
        {
        }

        public Cache(ICacheStorage<TKey, CacheItem<TValue>> storage)
        {
            _store = storage;
        }

        public void Add(TKey key, TValue value, TimeSpan? lifetime = null)
        {
            if (key == null) throw new ArgumentNullException("key");

            _store.Set(key, new CacheItem<TValue>(value, lifetime));
        }

        public TValue Get(TKey key)
        {
            CacheItem<TValue> value;
            _store.TryGetValue(key, out value);

            if (value == null)
            {
                return default(TValue);
            }

            if (value.IsExpired())
            {
                _store.TryRemove(key, out value);
                return default(TValue);
            }

            return value.Value;
        }

        public void Remove(TKey key)
        {
            CacheItem<TValue> value;
            _store.TryRemove(key, out value);
        }

        public int Count
        {
            get { return _store.Count; }
        }

        public TValue Get(TKey key, Func<TValue> valueFunction, TimeSpan? lifeTime = null)
        {
            if (key == null) throw new ArgumentNullException("key");

            CacheItem<TValue> cacheItem;
            TValue value;

            if (!_store.TryGetValue(key, out cacheItem) || cacheItem.IsExpired())
            {
                value = valueFunction();
                Add(key, value, lifeTime);
            }
            else
            {
                value = cacheItem.Value;
            }

            return value;
        }

        public void Purge()
        {
            _store.Purge();
        }

        public IEnumerable<TValue> Values
        {
            get { return _store.Values.Select(c => c.Value); }
        }


    }
}