using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    internal static class DbDataviewStructCache
    {
        class StructCache
        {
            public DateTime CacheTime { get; set; }
            public Common.DbDataTableInfo CacheData { get; set; }
        }
        static readonly int CacheMinutes = 60 * 24;
        static Dictionary<string, StructCache> CacheDic = new Dictionary<string, StructCache>();
        public static Common.DbDataTableInfo Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("key不能为空");
            }
            key = key.ToLower();
            if (CacheDic.ContainsKey(key))
            {
                var obj = CacheDic[key];
                if ((DateTime.Now - obj.CacheTime).TotalMinutes < CacheMinutes)
                {
                    return obj.CacheData;
                }
                CacheDic.Remove(key);
                return obj.CacheData;
            }
            return null;
        }
        public static void Set(string key, Common.DbDataTableInfo value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("key不能为空");
            }
            key = key.ToLower();
            if (CacheDic.ContainsKey(key))
            {
                CacheDic[key] = new StructCache { CacheTime = DateTime.Now, CacheData = value };
            }
            else
            {
                CacheDic.Add(key, new StructCache { CacheTime = DateTime.Now, CacheData = value });
            }
        }

    }
}
