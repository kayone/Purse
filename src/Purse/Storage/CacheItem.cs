using System;

namespace Purse.Storage
{
    public class CacheItem<T>
    {
        public CacheItem(T obj, TimeSpan? lifetime = null)
        {
            Value = obj;
            if (lifetime.HasValue)
            {
                ExpiryTime = DateTime.UtcNow + lifetime.Value;
            }
        }

        public T Value { get; private set; }
        public DateTime? ExpiryTime { get; private set; }

        public bool IsExpired()
        {
            return ExpiryTime.HasValue && ExpiryTime.Value < DateTime.UtcNow;
        }
    }
}