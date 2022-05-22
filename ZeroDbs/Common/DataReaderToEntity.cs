using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ZeroDbs.Common
{
    public static class DataReaderToEntity
    {
        public static T TakeOne<T>(System.Data.IDataReader reader) where T :class,new ()
        {
            T obj = default(T);
            List<T>(reader, new DataReadHandler<T>((r) => {
                obj = r.RowData;
                r.Next = false;
            }));
            return obj;
        }
        public static List<T> List<T>(System.Data.IDataReader reader) where T : class, new()
        {
            List<T> reval = new List<T>();
            List<T>(reader, (e) => {
                reval.Add(e.RowData);
            });
            return reval;
        }
        public static void List<T>(System.Data.IDataReader reader, DataReadHandler<T> callback) where T : class, new()
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
                    if (TargetTypeIsBool(dic[index].PropertyType))
                    {
                        dic[index].SetValue(obj, ConverToBool(val), null);
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

        public static T TakeOneByEmit<T>(System.Data.IDataReader reader) where T : class, new()
        {
            T obj = default(T);
            ListByEmit<T>(reader, new DataReadHandler<T>((r) => {
                obj = r.RowData;
                r.Next = false;
            }));
            return obj;
        }
        public static List<T> ListByEmit<T>(System.Data.IDataReader reader) where T : class, new()
        {
            List<T> reval = new List<T>();
            ListByEmit<T>(reader, (e) => {
                reval.Add(e.RowData);
            });
            return reval;
        }
        public static void ListByEmit<T>(System.Data.IDataReader reader, DataReadHandler<T> callback) where T : class, new()
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


        private static Type boolType = typeof(bool);
        private static Type boolNullableType = typeof(Nullable<bool>);
        public static bool TargetTypeIsBool(Type type)
        {
            return type == boolType || type == boolNullableType;
        }
        public static bool ConverToBool(object value)
        {
            if(value == null) { return false; }
            if(value is bool)
            {
                return (bool)value;
            }
            if(value is string)
            {
                string s = value.ToString().ToLower();
                return s == "true" || s == "1";
            }
            if (value is decimal)
            {
                return 0M != (decimal)value;
            }
            if (value is float || value is double)
            {
                return 0D != (double)value;
            }
            if (value is long || value is int || value is short || value is byte)
            {
                return 0L != (long)value;
            }
            if(value is Guid)
            {
                return Guid.Empty != (Guid)value;
            }
            return false;
        }
        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static object GetDefaultValue(Type parameter)
        {
            return parameter.IsValueType ? Activator.CreateInstance(parameter) : null;
        }

    }
}
