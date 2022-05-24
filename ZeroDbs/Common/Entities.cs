using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ZeroDbs.Common
{
    public static class Entities
    {
        public static T TakeOneFromDataReader<T>(System.Data.IDataReader reader) where T :class,new ()
        {
            T obj = default(T);
            ListFromDataReader<T>(reader, new DataReadHandler<T>((r) => {
                obj = r.RowData;
                r.Next = false;
            }));
            return obj;
        }
        public static List<T> ListFromDataReader<T>(System.Data.IDataReader reader) where T : class, new()
        {
            List<T> reval = new List<T>();
            ListFromDataReader<T>(reader, (e) => {
                reval.Add(e.RowData);
            });
            return reval;
        }
        public static void ListFromDataReader<T>(System.Data.IDataReader reader, DataReadHandler<T> callback) where T : class, new()
        {
            var pis = PropertyInfoCache.GetPropertyInfoList<T>();
            var dic = new Dictionary<int, System.Reflection.PropertyInfo>(reader.FieldCount);
            int i = 0;
            while (i < reader.FieldCount)
            {
                var index = pis.FindIndex(o => string.Equals(o.Name, reader.GetName(i), StringComparison.OrdinalIgnoreCase));
                if (index > -1)
                {
                    dic.Add(i, pis[index]);
                }
                i++;
            }
            long rowNum = 0;
            while (reader.Read())
            {
                T obj = new T();
                foreach(int index in dic.Keys)
                {
                    var val = reader.GetValue(index);
                    if (ValueConvert.IsBoolType(dic[index].PropertyType))
                    {
                        dic[index].SetValue(obj, ValueConvert.GetBool(val), null);
                    }
                    else
                    {
                        if (DBNull.Value != val)
                        {
                            dic[index].SetValue(obj, val, null);
                        }
                    }
                }
                rowNum++;
                DataReadArgs<T> result = new DataReadArgs<T>(rowNum, obj);
                try
                {
                    callback(result);
                }
                catch
                {
                    break;
                }
                if (!result.Next)
                {
                    break;
                }
            }
            reader.Close();
        }

        public static T TakeOneFromDataReaderByEmit<T>(System.Data.IDataReader reader) where T : class, new()
        {
            T obj = default(T);
            ListFromDataReaderByEmit<T>(reader, new DataReadHandler<T>((r) => {
                obj = r.RowData;
                r.Next = false;
            }));
            return obj;
        }
        public static List<T> ListFromDataReaderByEmit<T>(System.Data.IDataReader reader) where T : class, new()
        {
            List<T> reval = new List<T>();
            ListFromDataReaderByEmit<T>(reader, (e) => {
                reval.Add(e.RowData);
            });
            return reval;
        }
        public static void ListFromDataReaderByEmit<T>(System.Data.IDataReader reader, DataReadHandler<T> callback) where T : class, new()
        {
            var type = typeof(T);
            List<PropertyEmitSetter> pis = PropertyEmitSetter.GetProperties(type).ToList();
            var dic = new Dictionary<int, PropertyEmitSetter>(reader.FieldCount);
            int i = 0;
            while (i < reader.FieldCount)
            {
                var index = pis.FindIndex(o => string.Equals(o.Info.Name, reader.GetName(i), StringComparison.OrdinalIgnoreCase));
                if (index > -1)
                {
                    dic.Add(i, pis[index]);
                }
                i++;
            }
            long rowNum = 0;
            while (reader.Read())
            {
                T obj = new T();
                foreach (var index in dic.Keys)
                {
                    if (reader.IsDBNull(index)) { continue; }
                    dic[index].Setter(obj, reader.GetValue(index));
                }
                rowNum++;
                DataReadArgs<T> result = new DataReadArgs<T>(rowNum, obj);
                try
                {
                    callback(result);
                }
                catch
                {
                    break;
                }
                if (!result.Next)
                {
                    break;
                }
            }
            reader.Close();
        }

        public static T TakeOneFromNameValueCollection<T>(System.Collections.Specialized.NameValueCollection source)
            where T : class, new()
        {
            T reval = new T();
            var ps = PropertyInfoCache.GetPropertyInfoList<T>();
            for (var i = 0; i < source.Keys.Count; i++)
            {
                var key = source.Keys[i];
                var p = ps.Find(o => string.Equals(o.Name, key, StringComparison.OrdinalIgnoreCase));
                if (p != null)
                {
                    object val;
                    if (!ValueConvert.StrToTargetType(source[key], p.PropertyType, out val))
                    {
                        val = null;
                    }
                    p.SetValue(reval, val, null);
                }
            }
            return reval;
        }
        public static void UpdateFromNameValueCollection<T>(T entity, System.Collections.Specialized.NameValueCollection source)
            where T : class, new()
        {
            if (entity == null) { return; }

            var ps = PropertyInfoCache.GetPropertyInfoList<T>();
            for (var i = 0; i < source.Keys.Count; i++)
            {
                var key = source.Keys[i];
                var p = ps.Find(o => string.Equals(o.Name, key, StringComparison.OrdinalIgnoreCase));
                if (p != null)
                {
                    object val;
                    if (!ValueConvert.StrToTargetType(source[key], p.PropertyType, out val))
                    {
                        val = null;
                    }
                    p.SetValue(entity, val, null);
                }
            }
        }


        


    }
}
