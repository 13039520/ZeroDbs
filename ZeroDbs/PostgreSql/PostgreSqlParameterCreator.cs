using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs.PostgreSql
{
    internal class PostgreSqlParameterCreator : IDbParameterCreator
    {
        public string DbType { get; }
        public PostgreSqlParameterCreator(string dbType)
        {
            this.DbType = dbType;
        }
        public System.Data.Common.DbParameter Create()
        {
            return new NpgsqlParameter();
        }
        public System.Data.Common.DbParameter Create(string pName, object pValue)
        {
            NpgsqlParameter p = new NpgsqlParameter();
            p.ParameterName = pName;
            p.NpgsqlValue = pValue is null ? DBNull.Value : pValue;
            NpgsqlDbType? a = TryGetNpgsqlDbTypeByDictConfig(pValue.GetType());
            if (a.HasValue)
            {
                p.NpgsqlDbType = a.Value;
            }
            return p;
        }
        public System.Data.Common.DbParameter Create(string pName, System.Data.DbType dbType, int size, object pValue)
        {
            NpgsqlParameter p = new NpgsqlParameter();
            p.ParameterName = pName;
            p.NpgsqlValue = pValue is null ? DBNull.Value : pValue;
            NpgsqlDbType? a = TryGetNpgsqlDbTypeByDictConfig(pValue.GetType());
            if (a.HasValue)
            {
                p.NpgsqlDbType = a.Value;
            }
            p.DbType = dbType;
            p.Size = size;
            return p;
        }

        #region -- private --
        private static Dictionary<Type, NpgsqlDbType> dict = null;
        private static bool dictInitialized = false;
        private static object _lock = new object();
        private static Dictionary<Type, NpgsqlDbType> CreateMappingDictionary()
        {
            Dictionary<Type, NpgsqlDbType> map = new Dictionary<Type, NpgsqlDbType>();
            map.Add(typeof(string), NpgsqlDbType.Text);
            //map.Add(typeof(byte), NpgsqlDbType.Unknown);
            //map.Add(typeof(uint), NpgsqlDbType.Oid);
            //map.Add(typeof(ulong), NpgsqlDbType.Xid8);
            map.Add(typeof(short), NpgsqlDbType.Smallint);
            map.Add(typeof(int), NpgsqlDbType.Integer);
            map.Add(typeof(long), NpgsqlDbType.Bigint);
            map.Add(typeof(float), NpgsqlDbType.Real);
            map.Add(typeof(double), NpgsqlDbType.Double);
            map.Add(typeof(decimal), NpgsqlDbType.Numeric);
            map.Add(typeof(bool), NpgsqlDbType.Boolean);
            map.Add(typeof(DateTime), NpgsqlDbType.Timestamp);
            map.Add(typeof(NpgsqlInterval), NpgsqlDbType.Interval);
#if NET40
            map.Add(typeof(DateTimeOffset), NpgsqlDbType.TimeTZ);
#else
            map.Add(typeof(DateTimeOffset), NpgsqlDbType.TimeTz);
#endif
            map.Add(typeof(TimeSpan), NpgsqlDbType.Time);

            map.Add(typeof(NpgsqlPoint), NpgsqlDbType.Point);
            map.Add(typeof(NpgsqlLSeg), NpgsqlDbType.LSeg);
            map.Add(typeof(NpgsqlPath), NpgsqlDbType.Path);
            map.Add(typeof(NpgsqlPolygon), NpgsqlDbType.Polygon);
            map.Add(typeof(NpgsqlCircle), NpgsqlDbType.Circle);
            map.Add(typeof(NpgsqlBox), NpgsqlDbType.Box);
#if NET40

#else
            map.Add(typeof(NpgsqlLine), NpgsqlDbType.Line);

            map.Add(typeof(NpgsqlRange<short>), NpgsqlDbType.Range | NpgsqlDbType.Smallint);
            map.Add(typeof(NpgsqlRange<int>), NpgsqlDbType.IntegerRange);
            map.Add(typeof(NpgsqlRange<long>), NpgsqlDbType.BigIntRange);
            map.Add(typeof(NpgsqlRange<decimal>), NpgsqlDbType.NumericRange);
            map.Add(typeof(NpgsqlRange<float>), NpgsqlDbType.Range | NpgsqlDbType.Real);
            map.Add(typeof(NpgsqlRange<double>), NpgsqlDbType.Range | NpgsqlDbType.Double);
            map.Add(typeof(NpgsqlRange<DateTime>), NpgsqlDbType.DateRange);
            map.Add(typeof(NpgsqlRange<TimeSpan>), NpgsqlDbType.Range | NpgsqlDbType.Time);
#endif
            map.Add(typeof(byte[]), NpgsqlDbType.Bytea);
            map.Add(typeof(short[]), NpgsqlDbType.Array | NpgsqlDbType.Smallint);
            map.Add(typeof(int[]), NpgsqlDbType.Array | NpgsqlDbType.Integer);
            map.Add(typeof(long[]), NpgsqlDbType.Array | NpgsqlDbType.Bigint);
            map.Add(typeof(decimal[]), NpgsqlDbType.Array | NpgsqlDbType.Numeric);
            map.Add(typeof(float[]), NpgsqlDbType.Array | NpgsqlDbType.Real);
            map.Add(typeof(double[]), NpgsqlDbType.Array | NpgsqlDbType.Double);
            map.Add(typeof(DateTime[]), NpgsqlDbType.Array | NpgsqlDbType.Timestamp);
            map.Add(typeof(TimeSpan[]), NpgsqlDbType.Array | NpgsqlDbType.Time);
#if NET40
            map.Add(typeof(DateTimeOffset[]), NpgsqlDbType.Array | NpgsqlDbType.TimeTZ);
#else
            map.Add(typeof(DateTimeOffset[]), NpgsqlDbType.Array | NpgsqlDbType.TimeTz);
            map.Add(typeof(NpgsqlLine[]), NpgsqlDbType.Array | NpgsqlDbType.Line);
#endif
            map.Add(typeof(NpgsqlPoint[]), NpgsqlDbType.Array | NpgsqlDbType.Point);
            map.Add(typeof(NpgsqlLSeg[]), NpgsqlDbType.Array | NpgsqlDbType.LSeg);
            map.Add(typeof(NpgsqlPath[]), NpgsqlDbType.Array | NpgsqlDbType.Path);
            map.Add(typeof(NpgsqlPolygon[]), NpgsqlDbType.Array | NpgsqlDbType.Polygon);
            map.Add(typeof(NpgsqlCircle[]), NpgsqlDbType.Array | NpgsqlDbType.Circle);
            map.Add(typeof(NpgsqlBox[]), NpgsqlDbType.Array | NpgsqlDbType.Box);
            map.Add(typeof(System.Net.IPAddress[]), NpgsqlDbType.Array | NpgsqlDbType.Inet);
            map.Add(typeof(System.Net.NetworkInformation.PhysicalAddress[]), NpgsqlDbType.Array | NpgsqlDbType.MacAddr);

            map.Add(typeof(System.Net.IPAddress), NpgsqlDbType.Inet);
            map.Add(typeof(System.Net.NetworkInformation.PhysicalAddress), NpgsqlDbType.MacAddr);

#if NET40
            map.Add(typeof(BitArray), NpgsqlDbType.Array | NpgsqlDbType.Bit);
#else
            map.Add(typeof(BitArray), NpgsqlDbType.Varbit);
#endif
            return map;
        }
        private static void DictionaryInit()
        {
            if (dictInitialized) { return; }
            lock (_lock)
            {
                if (dictInitialized) { return; }
                dict = CreateMappingDictionary();
                dictInitialized = true;
            }
        }
        private static Dictionary<Type, NpgsqlDbType> GetMappingDictionary()
        {
            if (dictInitialized) { return dict; }
            DictionaryInit();
            return dict;
        }
        private static NpgsqlDbType? TryGetNpgsqlDbTypeByDictConfig(Type type)
        {
            Dictionary<Type, NpgsqlDbType> dic = GetMappingDictionary();
            if (dic.ContainsKey(type))
            {
                return dic[type];
            }
            return null;
        }
        #endregion

    }
}
