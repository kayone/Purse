using System;
using System.Collections.Generic;
using System.Linq;
using Purse.Storage;

namespace Purse
{
    public class Purse<TKey, TValue> : IPurse<TKey, TValue>
    {
        private readonly IPurseStorage<TKey, CacheItem<TValue>> _store;

        public Purse()
            : this(new MemoryStorage<TKey, CacheItem<TValue>>())
        {
        }

        public Purse(IPurseStorage<TKey, CacheItem<TValue>> storage)
        {
            _store = storage;
        }

        public void Set(TKey key, TValue value, TimeSpan? lifetime = null)
        {
            if (key == null) throw new ArgumentNullException("key");

            _store.Set(key, new CacheItem<TValue>(value, lifetime));
        }

        public TValue Find(TKey key)
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

            return value.Object;
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

        public TValue Get(TKey key, Func<TValue> function, TimeSpan? lifeTime = null)
        {
            if (key == null) throw new ArgumentNullException("key");

            CacheItem<TValue> cacheItem;
            TValue value;

            if (!_store.TryGetValue(key, out cacheItem) || cacheItem.IsExpired())
            {
                value = function();
                Set(key, value, lifeTime);
            }
            else
            {
                value = cacheItem.Object;
            }

            return value;
        }

        public void Purge()
        {
            _store.Purge();
        }

        public IEnumerable<TValue> Values
        {
            get { return _store.Values.Select(c => c.Object); }
        }


    }
}