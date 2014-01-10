using System;
using System.Collections.Generic;
using System.Linq;
using Purse.Storage;

namespace Purse
{
    public class Purse<T> : IPurse<T>
    {
        private readonly IPurseStorage<string, CacheItem<T>> _store;

        public Purse()
            : this(new MemoryStorage<string, CacheItem<T>>())
        {
        }

        public Purse(IPurseStorage<string, CacheItem<T>> storage)
        {
            _store = storage;
        }

        public void Set(string key, T value, TimeSpan? lifetime = null)
        {
            if (key == null) throw new ArgumentNullException("key");

            _store.Set(key, new CacheItem<T>(value, lifetime));
        }

        public T Find(string key)
        {
            CacheItem<T> value;
            _store.TryGetValue(key, out value);

            if (value == null)
            {
                return default(T);
            }

            if (value.IsExpired())
            {
                _store.TryRemove(key, out value);
                return default(T);
            }

            return value.Object;
        }

        public void Remove(string key)
        {
            CacheItem<T> value;
            _store.TryRemove(key, out value);
        }

        public int Count
        {
            get { return _store.Count; }
        }

        public T Get(string key, Func<T> function, TimeSpan? lifeTime = null)
        {
            if (key == null) throw new ArgumentNullException("key");

            CacheItem<T> cacheItem;
            T value;

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

        public IEnumerable<T> Values
        {
            get { return _store.Values.Select(c => c.Object); }
        }


    }
}