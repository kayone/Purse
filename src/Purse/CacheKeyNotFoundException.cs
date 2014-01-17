using System;

namespace Purse
{
    public class CacheKeyNotFoundException : Exception
    {
        public object Key { get; private set; }

        internal CacheKeyNotFoundException(object key)
            : base("Key not found " + key)
        {
            Key = key;
        }
    }
}