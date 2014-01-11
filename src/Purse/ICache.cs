using System;
using System.Collections.Generic;

namespace Purse
{
    /// <typeparam name="TKey">Type used for key. It's highly recommanded that you use value types eg. int, decimal and strings</typeparam>
    /// <typeparam name="TValue">Type of value stored in cache.</typeparam>
    public interface ICache<TKey, TValue>
    {
        /// <summary>
        /// Return list of all values stored in cache
        /// </summary>
        IEnumerable<TValue> Values { get; }

        /// <summary>
        /// Add an item with a specific key to the cache
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Value associated with the cache</param>
        /// <param name="lifetime">Duration for which the item is considered valid</param>
        void Set(TKey key, TValue value, TimeSpan? lifetime = null);

        /// <summary>
        /// Retrieve an item from cache. Adding it to cache if it doesn't already exist or has expired
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="valueFunction">Callback used to get the proper value if the item doesn't
        ///  exist in cache or the existing value has expired</param>
        /// <param name="lifeTime">Duration for which the item is considered valid</param>
        /// <returns>Value stored in cache</returns>
        /// <remarks>Please note the default value of TValue eg. null or 0 for numeric types is considered
        /// a valid value and stored in cache</remarks>
        TValue Get(TKey key, Func<TValue> valueFunction, TimeSpan? lifeTime = null);

        /// <summary>
        /// Retrieve an item from cache
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <returns>Cached value for the given key</returns>
        /// <remarks>Please note the default value of TValue eg. null or 0 for numeric types is considered
        /// a valid value and stored in cache</remarks>
        TValue Get(TKey key);

        /// <summary>
        /// Current number of items stored in cache
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Remove all existing items from cache
        /// </summary>
        void Purge();

        /// <summary>
        /// Remove an item with a specific key from cache
        /// </summary>
        /// <param name="key">Key of the item to remove</param>
        void Remove(TKey key);
    }

}