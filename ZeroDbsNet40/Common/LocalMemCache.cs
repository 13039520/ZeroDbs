using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class MyLocalMemCache : System.Runtime.Caching.MemoryCache
    {
        public MyLocalMemCache(string name, System.Collections.Specialized.NameValueCollection config = null) : base(name, config)
        {
        }
        public List<string> GetAllKeys()
        {
            IEnumerator<KeyValuePair<string, object>> enumerator = base.GetEnumerator();
            List<string> reval = new List<string>();
            while (enumerator.MoveNext())
            {
                reval.Add(enumerator.Current.Key);
            }
            return reval;
        }
    }
    public class LocalMemCache: ICache
    {
        private static readonly MyLocalMemCache Cache = new MyLocalMemCache("ZeroDbs");
        private ISerialization serialization = null;
        public ISerialization Serialization
        {
            get { return this.serialization; }
        }
        public LocalMemCache(ISerialization serialization)
        {
            this.serialization = serialization;
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
            Cache.Set(key, value, new DateTimeOffset(expireDatetime));
        }
        private object _Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return Cache.Get(key);
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
            return Cache.GetAllKeys();
        }

        public void Set<T>(string key, T value) where T : class
        {
            Set<T>(key, value, DateTime.Now.AddMinutes(20));
        }
        public void Set<T>(string key, T value, DateTime expireDatetime) where T : class
        {
            this._Set(key, value, expireDatetime);
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



    }
}
