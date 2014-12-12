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

        public void Set(TKey key, TValue value, TimeSpan? lifetime = null)
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
                throw new CacheKeyNotFoundException(key.ToString());
            }

            if (value.IsExpired())
            {
                _store.TryRemove(key, out value);
                throw new CacheKeyNotFoundException(key.ToString());
            }

            return value.Value;
        }


        public bool ContainsKey(TKey key)
        {
            return _store.ContainsKey(key);
        }

        public void PurgeExpired()
        {
            var expiredKeys = _store.Dictionary.Where(c => c.Value.IsExpired()).Select(c => c.Key);

            foreach (var expiredKey in expiredKeys)
            {
                Remove(expiredKey);
            }
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
                Set(key, value, lifeTime);
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