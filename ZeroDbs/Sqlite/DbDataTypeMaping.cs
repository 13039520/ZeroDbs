using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Sqlite
{
    internal class DbDataTypeMaping: IDbDataTypeMaping
    {
        public Type GetDotNetType(string dbDataTypeName, long maxLength)
        {
            Type type;
            switch (dbDataTypeName)
            {
                case "INTEGER":
                    type = typeof(long);
                    break;
                case "BIGINT":
                    type = typeof(long);
                    break;
                case "BLOB":
                    type = typeof(byte[]);
                    break;
                case "BOOLEAN":
                    type = typeof(bool);
                    break;
                case "CHAR":
                    if (maxLength == 36)
                    {
                        type = typeof(Guid);
                    }
                    else
                    {
                        type = typeof(string);
                    }
                    break;
                case "DATE":
                    type = typeof(DateTime);
                    break;
                case "DATETIME":
                    type = typeof(DateTime);
                    break;
                case "DECIMAL":
                    type = typeof(decimal);
                    break;
                case "DOUBLE":
                    type = typeof(double);
                    break;
                case "INT":
                    type = typeof(int);
                    break;
                case "NUMERIC":
                    type = typeof(decimal);
                    break;
                case "REAL":
                    type = typeof(double);
                    break;
                case "STRING":
                    type = typeof(string);
                    break;
                case "TEXT":
                    type = typeof(string);
                    break;
                case "TIME":
                    type = typeof(DateTime);
                    break;
                case "VARCHAR":
                    if (maxLength == 36)
                    {
                        type = typeof(Guid);
                    }
                    else
                    {
                        type = typeof(string);
                    }
                    break;
                default:
                    type = typeof(object);
                    break;
            }
            return type;
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
