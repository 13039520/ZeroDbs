using NpgsqlTypes;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ZeroDbs
{
    /// <summary>
    /// 数据库数据类型名称映射到CRL类型
    /// <para>示例：PostgreSQL+int4=System.Int32</para>
    /// <para>备注：如果发现类型映射错误可以自行写入进行替换</para>
    /// </summary>
    public static class DbDataTypeMap
    {
        static ConcurrentDictionary<string, ConcurrentDictionary<string, Type>> _dict = new ConcurrentDictionary<string, ConcurrentDictionary<string, Type>>(StringComparer.OrdinalIgnoreCase);
        static DbDataTypeMap()
        {
            SetTypes("SqlServer", new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                ["tinyint"] = typeof(byte),
                ["smallint"] = typeof(short),
                ["int"] = typeof(int),
                ["bigint"] = typeof(long),
                ["real"] = typeof(float),
                ["float"] = typeof(double),
                ["decimal"] = typeof(decimal),
                ["numeric"] = typeof(decimal),
                ["money"] = typeof(decimal),
                ["smallmoney"] = typeof(decimal),
                ["bit"] = typeof(bool),
                ["char"] = typeof(string),
                ["nchar"] = typeof(string),
                ["varchar"] = typeof(string),
                ["nvarchar"] = typeof(string),
                ["text"] = typeof(string),
                ["ntext"] = typeof(string),
                ["xml"] = typeof(string),
                ["date"] = typeof(DateTime),
                ["datetime"] = typeof(DateTime),
                ["datetime2"] = typeof(DateTime),
                ["smalldatetime"] = typeof(DateTime),
                ["datetimeoffset"] = typeof(DateTimeOffset),
                ["time"] = typeof(TimeSpan),
                ["uniqueidentifier"] = typeof(Guid),

                ["binary"] = typeof(byte[]),
                ["varbinary"] = typeof(byte[]),
                ["image"] = typeof(byte[]),
                ["timestamp"] = typeof(byte[]),
                ["rowversion"] = typeof(byte[]),
                ["sql_variant"] = typeof(object),
                ["hierarchyid"] = typeof(byte[]),
                ["geometry"] = typeof(byte[]),
                ["geography"] = typeof(byte[])
            });
            SetTypes("PostgreSQL", new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                ["text"] = typeof(string),
                ["name"] = typeof(string),
                ["char"] = typeof(char),
                ["varchar"] = typeof(string),
                ["bpchar"] = typeof(string),
                ["citext"] = typeof(string),
                ["json"] = typeof(string),
                ["jsonb"] = typeof(string),
                ["xml"] = typeof(string),
                ["int2"] = typeof(short),
                ["int4"] = typeof(int),
                ["int8"] = typeof(long),
                ["float4"] = typeof(float),
                ["float8"] = typeof(double),
                ["numeric"] = typeof(decimal),
                ["money"] = typeof(decimal),
                ["oid"] = typeof(uint),
                ["xid"] = typeof(uint),
                ["xid8"] = typeof(ulong),
                ["cid"] = typeof(uint),
                ["date"] = typeof(DateOnly),
#if !NET45
                ["interval"] = typeof(NpgsqlInterval),
#endif
                ["timestamp"] = typeof(DateTime),
                ["timestamptz"] = typeof(DateTimeOffset),
                ["time"] = typeof(TimeSpan),
                ["timetz"] = typeof(DateTimeOffset),
                ["point"] = typeof(NpgsqlPoint),
                ["lseg"] = typeof(NpgsqlLSeg),
                ["path"] = typeof(NpgsqlPath),
                ["polygon"] = typeof(NpgsqlPolygon),
                ["circle"] = typeof(NpgsqlCircle),
                ["box"] = typeof(NpgsqlBox),
#if !NET45
                ["line"] = typeof(NpgsqlLine),
                ["tsvector"] = typeof(NpgsqlTsVector),
#else
    ["line"] = typeof(string),
    ["tsvector"] = typeof(string),
#endif

                ["oidvector"] = typeof(uint[]),
                ["uuid"] = typeof(Guid),
                ["bool"] = typeof(bool),
                ["varbit"] = typeof(BitArray),
                ["bytea"] = typeof(byte[]),
                ["hstore"] = typeof(Dictionary<string, string>),
                ["cidr"] = typeof(NpgsqlInet),
                ["inet"] = typeof(System.Net.IPAddress),
                ["macaddr"] = typeof(System.Net.NetworkInformation.PhysicalAddress),
#if !NET45
                ["tsquery"] = typeof(NpgsqlTsQuery),
#else
    ["tsquery"] = typeof(string),
#endif
                ["geometry"] = typeof(object),
                ["record"] = typeof(object[]),

                //range
                ["int2range"] = typeof(NpgsqlRange<short>),
                ["int4range"] = typeof(NpgsqlRange<int>),
                ["int8range"] = typeof(NpgsqlRange<long>),
                ["float4range"] = typeof(NpgsqlRange<float>),
                ["float8range"] = typeof(NpgsqlRange<double>),
                ["numericrange"] = typeof(NpgsqlRange<decimal>),
                ["moneyrange"] = typeof(NpgsqlRange<decimal>),
                ["oidrange"] = typeof(NpgsqlRange<uint>),
                ["xidrange"] = typeof(NpgsqlRange<uint>),
                ["xid8range"] = typeof(NpgsqlRange<ulong>),
                ["cidrange"] = typeof(NpgsqlRange<uint>),
                ["daterange"] = typeof(NpgsqlRange<DateOnly>),
#if !NET45
                ["intervalrange"] = typeof(NpgsqlRange<NpgsqlInterval>),
#endif
                ["timestamprange"] = typeof(NpgsqlRange<DateTime>),
                ["timestamptzrange"] = typeof(NpgsqlRange<DateTimeOffset>),
                ["timerange"] = typeof(NpgsqlRange<TimeSpan>),
                ["timetzrange"] = typeof(NpgsqlRange<DateTimeOffset>),
                //array
                ["_text"] = typeof(string[]),
                ["_name"] = typeof(string[]),
                ["_char"] = typeof(char[]),
                ["_varchar"] = typeof(string[]),
                ["_bpchar"] = typeof(string[]),
                ["_citext"] = typeof(string[]),
                ["_json"] = typeof(string[]),
                ["_jsonb"] = typeof(string[]),
                ["_xml"] = typeof(string[]),
                ["_int2"] = typeof(short[]),
                ["_int4"] = typeof(int[]),
                ["_int8"] = typeof(long[]),
                ["_float4"] = typeof(float[]),
                ["_float8"] = typeof(double[]),
                ["_numeric"] = typeof(decimal[]),
                ["_money"] = typeof(decimal[]),
                ["_oid"] = typeof(uint[]),
                ["_xid"] = typeof(uint[]),
                ["_xid8"] = typeof(ulong[]),
                ["_cid"] = typeof(uint[]),
                ["_date"] = typeof(DateOnly[]),
#if !NET45
                ["_interval"] = typeof(NpgsqlInterval[]),
#endif
                ["_timestamp"] = typeof(DateTime[]),
                ["_timestamptz"] = typeof(DateTimeOffset[]),
                ["_time"] = typeof(TimeSpan[]),
                ["_timetz"] = typeof(DateTimeOffset[]),
                ["_point"] = typeof(NpgsqlPoint[]),
                ["_lseg"] = typeof(NpgsqlLSeg[]),
                ["_path"] = typeof(NpgsqlPath[]),
                ["_polygon"] = typeof(NpgsqlPolygon[]),
                ["_circle"] = typeof(NpgsqlCircle[]),
                ["_box"] = typeof(NpgsqlBox[]),
#if !NET45
                ["_line"] = typeof(NpgsqlLine[]),
                ["_tsvector"] = typeof(NpgsqlTsVector[]),
#else
    ["_line"] = typeof(string[]),
    ["_tsvector"] = typeof(string[]),
#endif

                ["_oidvector"] = typeof(uint[][]),
                ["_uuid"] = typeof(Guid[]),
                ["_bool"] = typeof(bool[]),
                ["_varbit"] = typeof(BitArray[]),
                ["_bytea"] = typeof(byte[][]),
                ["_hstore"] = typeof(Dictionary<string, string>[]),
                ["_cidr"] = typeof((System.Net.IPAddress,int)[]),
                ["_inet"] = typeof(System.Net.IPAddress[]),
                ["_macaddr"] = typeof(System.Net.NetworkInformation.PhysicalAddress[]),
#if !NET45
                ["_tsquery"] = typeof(NpgsqlTsQuery[]),
#else
    ["_tsquery"] = typeof(string[]),
#endif
            });
            SetTypes("SQLite", new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                ["INTEGER"] = typeof(long),
                ["BIGINT"] = typeof(long),
                ["BLOB"] = typeof(byte[]),
                ["BOOLEAN"] = typeof(bool),
                ["CHAR"] = typeof(string),
                ["VARCHAR"] = typeof(string),
                ["STRING"] = typeof(string),
                ["TEXT"] = typeof(string),
                ["DATE"] = typeof(DateTime),
                ["DATETIME"] = typeof(DateTime),
                ["TIME"] = typeof(DateTime),
                ["DECIMAL"] = typeof(decimal),
                ["NUMERIC"] = typeof(decimal),
                ["DOUBLE"] = typeof(double),
                ["REAL"] = typeof(double),
                ["INT"] = typeof(int)
            });
            SetTypes("MySql", new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                // 整数
                ["TINYINT"] = typeof(byte),
                ["SMALLINT"] = typeof(short),
                ["MEDIUMINT"] = typeof(int),
                ["INT"] = typeof(int),
                ["INTEGER"] = typeof(int),
                ["BIGINT"] = typeof(long),

                // 浮点
                ["FLOAT"] = typeof(float),
                ["DOUBLE"] = typeof(double),
                ["REAL"] = typeof(double),

                // 定点
                ["DECIMAL"] = typeof(decimal),
                ["NUMERIC"] = typeof(decimal),

                // 字符串
                ["CHAR"] = typeof(string),
                ["VARCHAR"] = typeof(string),

                // 文本
                ["TINYTEXT"] = typeof(string),
                ["TEXT"] = typeof(string),
                ["MEDIUMTEXT"] = typeof(string),
                ["LONGTEXT"] = typeof(string),

                // 二进制
                ["BINARY"] = typeof(byte[]),
                ["VARBINARY"] = typeof(byte[]),

                // Blob
                ["TINYBLOB"] = typeof(byte[]),
                ["BLOB"] = typeof(byte[]),
                ["MEDIUMBLOB"] = typeof(byte[]),
                ["LONGBLOB"] = typeof(byte[]),

                // 日期时间
                ["DATE"] = typeof(DateTime),
                ["TIME"] = typeof(TimeSpan),
                ["DATETIME"] = typeof(DateTime),
                ["TIMESTAMP"] = typeof(DateTime),
                ["YEAR"] = typeof(int),

                // 布尔
                ["BIT"] = typeof(byte[]),
                ["BOOL"] = typeof(bool),
                ["BOOLEAN"] = typeof(bool),

                // 其它
                ["JSON"] = typeof(string),
                ["ENUM"] = typeof(string),
                ["SET"] = typeof(string),

                // 自定义约定（MySQL 本身没有 UUID 类型）
                ["UUID"] = typeof(Guid)
            });
        }
        public static bool GetType(string dbType, string dbDataTypeName, out Type type)
        {
            type = null;
            return _dict.TryGetValue(dbType, out var dict) && dict.TryGetValue(dbDataTypeName, out type);
        }
        public static bool SetType(string dbType, string dbDataTypeName, Type type)
        {
            if (string.IsNullOrEmpty(dbType)) { throw new ArgumentNullException(nameof(dbType)); }
            if (string.IsNullOrEmpty(dbDataTypeName)) { throw new ArgumentNullException(nameof(dbDataTypeName)); }
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            var dict = _dict.GetOrAdd(dbType, _ => new ConcurrentDictionary<string, Type>(StringComparer.OrdinalIgnoreCase));
            dict[dbDataTypeName] = type;
            return true;
        }
        public static void SetTypes(string dbType, IDictionary<string, Type> mappings)
        {
            if (mappings == null) { throw new ArgumentNullException(nameof(mappings)); }
            foreach (var item in mappings)
            {
                SetType(dbType, item.Key, item.Value);
            }
        }
    }
}
