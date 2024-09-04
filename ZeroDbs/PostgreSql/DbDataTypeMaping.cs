using NpgsqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.PostgreSql
{
    internal class DbDataTypeMaping: IDbDataTypeMaping
    {
        public Type GetDotNetType(string dbDataTypeName, long maxLength)
        {
            //参考： https://www.npgsql.org/doc/types/basic.html
            switch (dbDataTypeName)
            {
                
                #region -- number --
                case "int2":
                    return typeof(short);
                case "int4":
                    return typeof(int);
                case "int8":
                    return typeof(long);
                case "float4":
                    return typeof(float);
                case "float8":
                    return typeof(double);
                case "numeric":
                    return typeof(decimal);
                case "money":
                    return typeof(decimal);
                case "oid":
                    return typeof(uint);
                case "xid":
                    return typeof(uint);
                case "xid8":
                    return typeof(ulong);
                case "cid":
                    return typeof(uint);
                #endregion

                #region -- string --
                case "text":
                    return typeof(string);
                case "name":
                    return typeof(string);
                case "char":
                    return typeof(char);
                case "varchar":
                    return typeof(string);
                case "bpchar":
                    return typeof(string);
                case "citext":
                    return typeof(string);
                case "json":
                    return typeof(string);
                case "jsonb":
                    return typeof(string);
                case "xml":
                    return typeof(string);
                #endregion

                #region -- datetime --
                case "date":
                    return typeof(DateTime);
                case "interval":
                    return typeof(NpgsqlInterval);
                case "timestamp":
                    return typeof(DateTime);
                case "timestamptz":
                    return typeof(DateTime);
                case "time":
                    return typeof(TimeSpan);
                case "timetz":
                    return typeof(DateTimeOffset);
                #endregion

                #region -- geometry --
                case "point":
                    return typeof(NpgsqlPoint);
                case "lseg":
                    return typeof(NpgsqlLSeg);
                case "path":
                    return typeof(NpgsqlPath);
                case "polygon":
                    return typeof(NpgsqlPolygon);
                case "line":
#if NET40
                    return typeof(string);
#else
                    return typeof(NpgsqlLine);
#endif
                case "circle":
                    return typeof(NpgsqlCircle);
                case "box":
                    return typeof(NpgsqlBox);
                #endregion

                #region -- vector --
                case "oidvector":
                    return typeof(uint[]);
                case "tsvector":
#if NET40
                    return typeof(string);
#else
                    return typeof(NpgsqlTsVector);
#endif
                #endregion

                #region -- other --
                case "uuid":
                    return typeof(Guid);
                case "bool":
                    return typeof(bool);
                case "varbit":
                    return typeof(BitArray);
                case "bytea":
                    return typeof(byte[]);
                case "hstore":
                    return typeof(Dictionary<string, string>);
                case "cidr":
                    return typeof(NpgsqlInet);
                case "inet":
                    return typeof(System.Net.IPAddress);
                case "macaddr":
                    return typeof(System.Net.NetworkInformation.PhysicalAddress);
                case "tsquery":
#if NET40
                    return typeof(string);
#else
                    return typeof(NpgsqlTsQuery);
#endif
                case "geometry":
                    return typeof(object);// PostgisGeometry
                case "record":
                    return typeof(object[]);
                #endregion

                default:
                    var m = System.Text.RegularExpressions.Regex.Match(dbDataTypeName, @"^bit\(?<n>\d+\)$");
                    if (m.Success)
                    {
                        int n = Convert.ToInt32(m.Groups["n"].Value);
                        if (n != 1) { return typeof(BitArray); }
                        else { return typeof(bool); }
                    }
                    //range types
                    m = System.Text.RegularExpressions.Regex.Match(dbDataTypeName, @"^_(?<type>[a-z0-9]+)range$");
                    if (m.Success)
                    {
                        string t=m.Groups["type"].Value;
                        return GetRangeType(t);
                    }
                    //array types
                    m = System.Text.RegularExpressions.Regex.Match(dbDataTypeName, @"^_(?<type>[a-z0-9]+)$");
                    if (m.Success)
                    {
                        string t = m.Groups["type"].Value;
                        return GetArrayType(t);
                    }
                    return typeof(object);
            }
        }
        private Type GetRangeType(string dbTypeName)
        {
#if NET40
            return typeof(object);
#else
            switch (dbTypeName)
            {
                case "int2":
                    return typeof(NpgsqlRange<short>);
                case "int4":
                    return typeof(NpgsqlRange<int>);
                case "int8":
                    return typeof(NpgsqlRange<long>);
                case "float4":
                    return typeof(NpgsqlRange<float>);
                case "float8":
                    return typeof(NpgsqlRange<double>);
                case "numeric":
                    return typeof(NpgsqlRange<decimal>);
                case "money":
                    return typeof(NpgsqlRange<decimal>);
                case "date":
                    return typeof(NpgsqlRange<DateTime>);
                case "interval":
                    return typeof(NpgsqlRange<NpgsqlInterval>);
                case "timestamp":
                    return typeof(NpgsqlRange<DateTime>);
                case "timestamptz":
                    return typeof(NpgsqlRange<DateTime>);
                case "time":
                    return typeof(NpgsqlRange<TimeSpan>);
                case "timetz":
                    return typeof(NpgsqlRange<DateTimeOffset>);
            }
            return typeof(NpgsqlRange<int>);
#endif

        }
        private Type GetArrayType(string dbTypeName)
        {
            switch (dbTypeName)
            {
                #region -- number --
                case "int2":
                    return typeof(short[]);
                case "int4":
                    return typeof(int[]);
                case "int8":
                    return typeof(long[]);
                case "float4":
                    return typeof(float[]);
                case "float8":
                    return typeof(double[]);
                case "numeric":
                    return typeof(decimal[]);
                case "money":
                    return typeof(decimal[]);
                case "oid":
                    return typeof(uint[]);
                case "xid":
                    return typeof(uint[]);
                case "xid8":
                    return typeof(ulong[]);
                case "cid":
                    return typeof(uint[]);
                #endregion

                #region -- string --
                case "text":
                    return typeof(string[]);
                case "name":
                    return typeof(string[]);
                case "char":
                    return typeof(char[]);
                case "varchar":
                    return typeof(string[]);
                case "bpchar":
                    return typeof(string[]);
                case "citext":
                    return typeof(string[]);
                case "json":
                    return typeof(string[]);
                case "jsonb":
                    return typeof(string[]);
                case "xml":
                    return typeof(string[]);
                #endregion

                #region -- datetime --
                case "date":
                    return typeof(DateTime[]);
                case "interval":
                    return typeof(NpgsqlInterval[]);
                case "timestamp":
                    return typeof(DateTime[]);
                case "timestamptz":
                    return typeof(DateTime[]);
                case "time":
                    return typeof(TimeSpan[]);
                case "timetz":
                    return typeof(DateTimeOffset[]);
                #endregion

                #region -- geometry --
                case "point":
                    return typeof(NpgsqlPoint[]);
                case "lseg":
                    return typeof(NpgsqlLSeg[]);
                case "path":
                    return typeof(NpgsqlPath[]);
                case "polygon":
                    return typeof(NpgsqlPolygon[]);
                case "line":
#if NET40
                    return typeof(string[]);
#else
                    return typeof(NpgsqlLine[]);
#endif
                case "circle":
                    return typeof(NpgsqlCircle[]);
                case "box":
                    return typeof(NpgsqlBox[]);
                #endregion

                #region -- vector --
                case "oidvector":
                    return typeof(uint[][]);
                case "tsvector":
#if NET40
                    return typeof(string[]);
#else
                    return typeof(NpgsqlTsVector[]);
#endif
                #endregion

                #region -- other --
                case "uuid":
                    return typeof(Guid[]);
                case "bool":
                    return typeof(bool[]);
                case "varbit":
                    return typeof(BitArray[]);
                case "bytea":
                    return typeof(byte[][]);
                case "hstore":
                    return typeof(Dictionary<string, string>[]);
                case "cidr":
                    return typeof(NpgsqlInet[]);
                case "inet":
                    return typeof(System.Net.IPAddress[]);
                case "macaddr":
                    return typeof(System.Net.NetworkInformation.PhysicalAddress[]);
                case "tsquery":
#if NET40
                    return typeof(string[]);
#else
                    return typeof(NpgsqlTsQuery[]);
#endif
                case "geometry":
                    return typeof(object[]);// PostgisGeometry
                case "record":
                    return typeof(object[]);
                    #endregion
            }
            return typeof(object[]);
        }
        public string GetDotNetTypeFullName(string dbDataTypeName, long maxLength)
        {
            return GetDotNetType(dbDataTypeName, maxLength).FullName;
        }
        public string GetDotNetDefaultValueText(string defaultVal, string dbDataTypeName, long maxLength)
        {
            return string.Empty;
        }

    }
}
