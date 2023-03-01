using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public static class DbDataviewStructCache
    {
        class StructCache
        {
            public DateTime CacheTime { get; set; }
            public ITableInfo CacheData { get; set; }
        }
        static object _lock = new object();
        static Dictionary<string, StructCache> CacheDic = new Dictionary<string, StructCache>();
        public static ITableInfo Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("key is null or empty");
            }
            key = key.ToLower();
            if (CacheDic.ContainsKey(key))
            {
                return CacheDic[key].CacheData;
            }
            return null;
        }
        public static void Set(string key, ITableInfo value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("key is null or empty");
            }
            if (value == null)
            {
                throw new Exception("value is null");
            }
            key = key.ToLower();
            lock (_lock)
            {
                if (CacheDic.ContainsKey(key))
                {
                    CacheDic[key].CacheData = value;
                    CacheDic[key].CacheTime = DateTime.Now;
                }
                else
                {
                    CacheDic.Add(key, new StructCache { CacheTime = DateTime.Now, CacheData = value });
                }
            }
        }

    }
}
