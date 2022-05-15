using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs.Common
{
    internal static class EntityPropertyInfoCache
    {
        private static Dictionary<Type, List<System.Reflection.PropertyInfo>> _Properties = new Dictionary<Type, List<System.Reflection.PropertyInfo>>();
        private static object _lock = new object();
        public static List<System.Reflection.PropertyInfo> GetPropertyInfoList<T>()
        {
            var type = typeof(T);
            List<System.Reflection.PropertyInfo> properties = null;
            if (_Properties.TryGetValue(type, out properties))
            {
                return properties;
            }
            lock (_lock)
            {
                if (_Properties.TryGetValue(type, out properties))
                {
                    return properties;
                }
                properties = type.GetProperties().ToList();
                _Properties.Add(type, properties);
            }
            return properties;
        }
        public static List<System.Reflection.PropertyInfo> GetPropertyInfoList(Type type)
        {
            List<System.Reflection.PropertyInfo> properties = null;
            if (_Properties.TryGetValue(type, out properties))
            {
                return properties;
            }
            lock (_lock)
            {
                if (_Properties.TryGetValue(type, out properties))
                {
                    return properties;
                }
                properties = type.GetProperties().ToList();
                _Properties.Add(type, properties);
            }
            return properties;
        }
    }
}
