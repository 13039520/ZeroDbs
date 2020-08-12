using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Caches.Common
{
    public class MyLocalMemCache: System.Runtime.Caching.MemoryCache
    {
        public MyLocalMemCache(string name, System.Collections.Specialized.NameValueCollection config = null):base(name,config)
        {
        }
        public List<string> GetAllKeys()
        {
            IEnumerator<KeyValuePair<string, object>> enumerator= base.GetEnumerator();
            List<string> reval = new List<string>();
            while (enumerator.MoveNext())
            {
                reval.Add(enumerator.Current.Key);
            }
            return reval;
        }
    }
    public class LocalMemCache
    {
        private static readonly MyLocalMemCache Cache = new MyLocalMemCache("ZeroDbs");
        
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
            Cache.Set(key, value, new DateTimeOffset(expireDatetime));
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
            return Cache.Get(key);
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
            return Cache.GetAllKeys();
        }
    }
}
