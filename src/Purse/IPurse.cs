using System;
using System.Collections.Generic;

namespace Purse
{
    public interface IPurse
    {
        int Count { get; }
        void Purge();
        void Remove(string key);
    }

    public interface IPurse<T> : IPurse
    {
        IEnumerable<T> Values { get; }
        void Set(string key, T value, TimeSpan? lifetime = null);
        T Get(string key, Func<T> function, TimeSpan? lifeTime = null);
        T Find(string key);
    }
}