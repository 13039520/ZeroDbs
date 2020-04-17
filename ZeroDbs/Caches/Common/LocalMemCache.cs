using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Caches.Common
{
    public class LocalMemCache
    {
        private static readonly Microsoft.Extensions.Caching.Memory.MemoryCache Cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());
        
        public void Set(string key, object value, DateTime expireDatetime)
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

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            Cache.Remove(key);
        }
        public object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            object obj;
            if(!Cache.TryGetValue(key,out obj))
            {
                return null;
            }
            return obj;
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
    }
}
