using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// 类型别名映射(常见的基本数据类型已经存在)
    /// <para>示例：System.Int32=int System.Nullable<System.Int32>=int? ......</para>
    /// </summary>
    public static class TypeAliasMap
    {
        static readonly System.Collections.Concurrent.ConcurrentDictionary<Type, string> _dict = new System.Collections.Concurrent.ConcurrentDictionary<Type, string>();
        static TypeAliasMap()
        {
            _dict[typeof(bool)] = "bool";
            _dict[typeof(bool?)] = "bool?";
            _dict[typeof(byte)] = "byte";
            _dict[typeof(byte?)] = "byte?";
            _dict[typeof(short)] = "short";
            _dict[typeof(short?)] = "short?";
            _dict[typeof(int)] = "int";
            _dict[typeof(int?)] = "int?";
            _dict[typeof(long)] = "long";
            _dict[typeof(long?)] = "long?";
            _dict[typeof(ushort)] = "ushort";
            _dict[typeof(ushort?)] = "ushort?";
            _dict[typeof(uint)] = "uint";
            _dict[typeof(uint?)] = "uint?";
            _dict[typeof(ulong)] = "ulong";
            _dict[typeof(ulong?)] = "ulong?";
            _dict[typeof(float)] = "float";
            _dict[typeof(float?)] = "float?";
            _dict[typeof(double)] = "double";
            _dict[typeof(double?)] = "double?";
            _dict[typeof(decimal)] = "decimal";
            _dict[typeof(decimal?)] = "decimal?";

            _dict[typeof(DateOnly)] = "DateOnly";
            _dict[typeof(DateOnly?)] = "DateOnly?";
            _dict[typeof(DateTime)] = "DateTime";
            _dict[typeof(DateTime?)] = "DateTime?";
            _dict[typeof(DateTimeOffset)] = "DateTimeOffset";
            _dict[typeof(DateTimeOffset?)] = "DateTimeOffset?";
            _dict[typeof(TimeOnly)] = "TimeOnly";
            _dict[typeof(TimeOnly?)] = "TimeOnly?";
            _dict[typeof(TimeSpan)] = "TimeSpan";
            _dict[typeof(TimeSpan?)] = "TimeSpan?";

            _dict[typeof(Guid)] = "Guid";
            _dict[typeof(Guid?)] = "Guid?";
            _dict[typeof(char)] = "char";
            _dict[typeof(char?)] = "char?";
            _dict[typeof(string)] = "string";
            _dict[typeof(object)] = "object";

            //常见 PostgreSQL 的 Range 类型
            _dict[typeof(NpgsqlTypes.NpgsqlRange<byte>)] = "NpgsqlRange<byte>";
            _dict[typeof(NpgsqlTypes.NpgsqlRange<short>)] = "NpgsqlRange<short>";
            _dict[typeof(NpgsqlTypes.NpgsqlRange<int>)] = "NpgsqlRange<int>";
            _dict[typeof(NpgsqlTypes.NpgsqlRange<long>)] = "NpgsqlRange<long>";
            _dict[typeof(NpgsqlTypes.NpgsqlRange<decimal>)] = "NpgsqlRange<decimal>";
            _dict[typeof(NpgsqlTypes.NpgsqlRange<float>)] = "NpgsqlRange<float>";
            _dict[typeof(NpgsqlTypes.NpgsqlRange<double>)] = "NpgsqlRange<double>";
            _dict[typeof(NpgsqlTypes.NpgsqlRange<DateOnly>)] = "NpgsqlRange<DateOnly>";
            _dict[typeof(NpgsqlTypes.NpgsqlRange<DateTime>)] = "NpgsqlRange<DateTime>";
            _dict[typeof(NpgsqlTypes.NpgsqlRange<TimeSpan>)] = "NpgsqlRange<TimeSpan>";

        }
        /// <summary>
        /// 获取类型别名(如果没有配置则返回类型的全名)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetAlias(Type type)
        {
            if (_dict.TryGetValue(type, out var result)) { return result; }
            //优先返回全名：因为如果不是常见类型可能缺少命名空间引用
            return type.FullName ?? type.Name;
        }
        /// <summary>
        /// 设置类型别名(常见的基本数据类型已经存在)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="alias"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetAlias(Type type, string alias)
        {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (string.IsNullOrWhiteSpace(alias)) { throw new ArgumentNullException(nameof(alias)); }
            _dict[type] = alias;
        }
    }
}
