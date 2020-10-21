using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class LocalMemCache: ICache
    {
        private static readonly Microsoft.Extensions.Caching.Memory.MemoryCache Cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());
        private ISerialization serialization = null;
        public ISerialization Serialization
        {
            get { return this.serialization; }
        }

        public LocalMemCache(ISerialization serialization)
        {
            this.serialization = serialization;
        }

        public void Set<T>(string key, T value) where T : class
        {
            Set<T>(key, value, DateTime.Now.AddMinutes(20));
        }
        public void Set<T>(string key, T value, DateTime expireDatetime) where T : class
        {
            this.Set(key, value, expireDatetime);
        }
        public object Get(string key)
        {
            return this._Get(key);
        }
        public T Get<T>(string key) where T : class
        {
            object obj = this._Get(key);
            if (obj != null)
            {
                return obj as T;
            }
            return null;
        }
        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            Cache.Remove(key);
        }
        public void Clear()
        {
            var l = GetCacheKeys();
            foreach (var s in l)
            {
                Remove(s);
            }
        }
        public List<string> GetCacheKeys()
        {
            const System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
            var entries = Cache.GetType().GetField("_entries", flags).GetValue(Cache);
            var cacheItems = entries as System.Collections.IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (System.Collections.DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }

        private void _Set(string key, object value, DateTime expireDatetime)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            if (value == null)
            {
                return;
            }
            using (var entry = Cache.CreateEntry(key)) {
                entry.Value = value;
                entry.AbsoluteExpiration = new DateTimeOffset(expireDatetime);
            };
        }
        private object _Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            object obj;
            if (!Cache.TryGetValue(key, out obj))
            {
                return null;
            }
            return obj;
        }
        
    }
}
