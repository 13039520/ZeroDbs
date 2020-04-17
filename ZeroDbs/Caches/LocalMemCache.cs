using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Caches
{
    public class LocalMemCache : ZeroDbs.Interfaces.ICache
    {
        private ZeroDbs.Interfaces.ISerialization serialization = null;
        private Caches.Common.LocalMemCache localMemCache = null;
        public ZeroDbs.Interfaces.ISerialization Serialization
        {
            get { return this.serialization; }
        }
        public LocalMemCache(ZeroDbs.Interfaces.ISerialization serialization)
        {
            this.serialization = serialization;
            this.localMemCache = new Common.LocalMemCache();
        }
        public void Set<T>(string key, T value) where T : class
        {
            Set<T>(key, value, DateTime.Now.AddMinutes(20));
        }
        public void Set<T>(string key, T value, DateTime expireDatetime) where T : class
        {
            localMemCache.Set(key, value, expireDatetime);
        }
        public object Get(string key)
        {
            object obj = localMemCache.Get(key);
            if (serialization != null)
            {
                return serialization.Serialization<object>(obj).ToString();
            }
            return obj.ToString();
        }
        public T Get<T>(string key) where T: class
        {
            object obj = localMemCache.Get(key);
            if (obj != null)
            {
                return obj as T;
            }
            return null;
        }
        public void Remove(string key)
        {
            localMemCache.Remove(key);
        }
        public void Clear()
        {
            localMemCache.Clear();
        }
    }
}
