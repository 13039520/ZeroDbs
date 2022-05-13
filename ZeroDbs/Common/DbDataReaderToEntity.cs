using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ZeroDbs.Common
{
    public static class DbDataReaderToEntity<T> where T :class,new()
    {
        public static T Entity(System.Data.IDataReader reader)
        {
            T obj = default(T);
            EntityList(reader, new DbExecuteReadOnebyOneAction<T>((r) => {
                obj = r.RowData;
                r.Next = false;
            }));
            return obj;
        }
        public static List<T> EntityList(System.Data.IDataReader reader)
        {
            var type = typeof(T);
            var pis = type.GetProperties().ToList();
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
            List<T> reval = new List<T>();
            while (reader.Read())
            {
                T obj = new T();
                foreach (int index in dic.Keys)
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
                reval.Add(obj);
            }
            reader.Close();
            return reval;
        }
        public static void EntityList(System.Data.IDataReader reader, DbExecuteReadOnebyOneAction<T> callback)
        {
            var type = typeof(T);
            var pis = type.GetProperties().ToList();
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
                DbExecuteReadOnebyOneResult<T> result = new DbExecuteReadOnebyOneResult<T>(rowNum, obj);
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

        public static T EntityByEmit(System.Data.IDataReader reader)
        {
            T obj = default(T);
            EntityListByEmit(reader, new DbExecuteReadOnebyOneAction<T>((r) => {
                obj = r.RowData;
                r.Next = false;
            }));
            return obj;
        }
        public static List<T> EntityListByEmit(System.Data.IDataReader reader)
        {
            var type = typeof(T);
            List<EntityPropertyEmitSetter> pis = EntityPropertyEmitSetter.GetProperties(typeof(T)).ToList();
            var dic = new Dictionary<int, EntityPropertyEmitSetter>(reader.FieldCount);
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
            List<T> reval = new List<T>();
            while (reader.Read())
            {
                T obj = new T();
                foreach(var index in dic.Keys)
                {
                    if (reader.IsDBNull(index)) { continue; }
                    dic[index].Setter(obj, reader.GetValue(index));
                }
                reval.Add(obj);
            }
            reader.Close();
            return reval;
        }
        public static void EntityListByEmit(System.Data.IDataReader reader, DbExecuteReadOnebyOneAction<T> callback)
        {
            var type = typeof(T);
            List<EntityPropertyEmitSetter> pis = EntityPropertyEmitSetter.GetProperties(typeof(T)).ToList();
            var dic = new Dictionary<int, EntityPropertyEmitSetter>(reader.FieldCount);
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
                DbExecuteReadOnebyOneResult<T> result = new DbExecuteReadOnebyOneResult<T>(rowNum, obj);
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
