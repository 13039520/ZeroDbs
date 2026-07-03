using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class DataReaderWrapper : IDataReaderWrapper
    {
        readonly Dictionary<string, int> indexes;
        readonly int count;
        readonly IDataReader r;
        /// <summary>
        /// 字段索引字典
        /// </summary>
        public Dictionary<string, int> IndexDict {  get { return indexes; } }
        public DataReaderWrapper(IDataReader r)
        {
            this.r = r;
            count = r.FieldCount;
            indexes = new Dictionary<string, int>(count, StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < r.FieldCount; i++)
            {
                indexes[r.GetName(i)] = i;
            }
        }
        public T GetValue<T>(string field)
        {
            if (indexes.TryGetValue(field, out var n) && !r.IsDBNull(n))
            {
                return (T)r.GetValue(n);
            }
            return default;
        }
        public T GetValue<T>(string field, T noneValue)
        {
            if (indexes.TryGetValue(field, out var n) && !r.IsDBNull(n))
            {
                return (T)r.GetValue(n);
            }
            return noneValue;
        }
        public T GetValue<T>(int index)
        {
            if (index > -1 && index < count && !r.IsDBNull(index))
            {
                return (T)r.GetValue(index);
            }
            return default;
        }
        public T GetValue<T>(int index, T noneValue)
        {
            if (index > -1 && index < count && !r.IsDBNull(index))
            {
                return (T)r.GetValue(index);
            }
            return noneValue;
        }
        public object? GetValue(string field)
        {
            if (indexes.TryGetValue(field, out var n) && !r.IsDBNull(n))
            {
                return r.GetValue(n);
            }
            return null;
        }
        public object? GetValue(int index)
        {
            if (index > -1 && index < count && !r.IsDBNull(index))
            {
                return r.GetValue(index);
            }
            return null;
        }
        public string? GetDataTypeName(string field)
        {
            if (indexes.TryGetValue(field, out var n))
            {
                return r.GetDataTypeName(n);
            }
            return null;
        }
        public Type? GetFieldType(string field)
        {
            if (indexes.TryGetValue(field, out var n))
            {
                return r.GetFieldType(n);
            }
            return null;
        }
        public bool Exists(string field)
        {
            return indexes.ContainsKey(field);
        }
    }
}
