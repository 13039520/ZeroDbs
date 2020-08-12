using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces
{
    public interface ICache
    {
        ISerialization Serialization { get; }
        void Set<T>(string key, T value) where T : class;
        void Set<T>(string key, T value, DateTime ExpireDatetime) where T : class;
        object Get(string key);
        T Get<T>(string key) where T : class;
        void Remove(string key);
        void Clear();

    }
}
