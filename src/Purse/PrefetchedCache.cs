using System;
using System.Collections.Generic;

namespace Purse
{
    /// <typeparam name="TKey">Type used for key. It's highly recommanded that you use value types eg. int, decimal and strings</typeparam>
    /// <typeparam name="TValue">Type of value stored in cache.</typeparam>
    /// <summary>Use this cache doesn't user lazy loading. All values are loaded and expired at the same time. use this for smaller set of items</summary>
    public class PrefetchedCache<TKey, TValue>
    {
        private readonly Func<Dictionary<TKey, TValue>> _fetchFunc;
        private readonly TimeSpan? _lifeTime;
        private DateTime _expiry = DateTime.MaxValue;
        private Dictionary<TKey, TValue> _items;

        public PrefetchedCache(Func<Dictionary<TKey, TValue>> fetchFunc, TimeSpan? lifeTime = null)
        {
            _fetchFunc = fetchFunc;
            _lifeTime = lifeTime;
            Refresh();
        }

        /// <summary>
        /// Return list of all values stored in cache
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                return _items.Values;
            }
        }

        /// <summary>
        /// Retrieve an item from cache
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <returns>Cached value for the given key</returns>
        public TValue Get(TKey key)
        {
            try
            {
                ValidateExpiry();
                return _items[key];
            }
            catch (KeyNotFoundException)
            {
                throw new CacheKeyNotFoundException(key);
            }
        }

        /// <summary>
        /// Current number of items stored in cache
        /// </summary>

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        /// <summary>
        /// Repopulate cache from source
        /// </summary>
        public void Refresh()
        {
            var items = _fetchFunc();
            CalculateExpiry();
            _items = items;
        }

        /// <summary>
        /// Check if item exist in cache
        /// </summary>
        public bool ContainsKey(TKey key)
        {
            return _items.ContainsKey(key);
        }

        private void ValidateExpiry()
        {
            if (_expiry < DateTime.Now)
            {
                CalculateExpiry();
                Refresh();
            }
        }

        private void CalculateExpiry()
        {
            if (_lifeTime != null)
            {
                _expiry = DateTime.Now.Add(_lifeTime.Value);
            }
        }
    }

}