using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ZeroDbs.Tools
{
    public static class DataEntity
    {
        public static T Get<T>(System.Collections.Specialized.NameValueCollection source)
            where T : class, new()
        {
            T reval = new T();
            var ps = Common.PropertyInfoCache.GetPropertyInfoList<T>();
            for (var i = 0; i < source.Keys.Count; i++)
            {
                var key = source.Keys[i];
                var p = ps.Find(o => string.Equals(o.Name, key, StringComparison.OrdinalIgnoreCase));
                if (p != null)
                {
                    p.SetValue(reval, Common.ValueConvert.StrToTargetType(source[key], p.PropertyType), null);
                }
            }
            return reval;
        }
        public static void Update<T>(T entity, System.Collections.Specialized.NameValueCollection source)
            where T : class, new()
        {
            if (entity == null) { return; }

            var ps = Common.PropertyInfoCache.GetPropertyInfoList<T>();
            for (var i = 0; i < source.Keys.Count; i++)
            {
                var key = source.Keys[i];
                var p = ps.Find(o => string.Equals(o.Name, key, StringComparison.OrdinalIgnoreCase));
                if (p != null)
                {
                    p.SetValue(entity, Common.ValueConvert.StrToTargetType(source[key], p.PropertyType), null);
                }
            }
        }

    }
}
