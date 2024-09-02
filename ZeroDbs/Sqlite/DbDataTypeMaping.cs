using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Sqlite
{
    internal class DbDataTypeMaping: IDataTypeMaping
    {
        public string GetDotNetTypeString(int dbDataTypeIntValue, long maxLength)
        {
            throw new Exception("不受支持");
        }
        public string GetDotNetTypeString(string dbDataTypeName, long maxLength)
        {
            string s = "";
            switch (dbDataTypeName)
            {
                case "INTEGER":
                    s = "Int64";
                    break;
                case "BIGINT":
                    s = "Int64";
                    break;
                case "BLOB":
                    s = "Byte[]";
                    break;
                case "BOOLEAN":
                    s = "Boolean";
                    break;
                case "CHAR":
                    if (maxLength == 36)
                    {
                        s = "Guid";
                    }
                    else
                    {
                        s = "String";
                    }
                    break;
                case "DATE":
                    s = "DateTime";
                    break;
                case "DATETIME":
                    s = "DateTime";
                    break;
                case "DECIMAL":
                    s = "Decimal";
                    break;
                case "DOUBLE":
                    s = "Double";
                    break;
                case "INT":
                    s = "Int32";
                    break;
                case "NUMERIC":
                    s = "Decimal";
                    break;
                case "REAL":
                    s = "Double";
                    break;
                case "STRING":
                    s = "String";
                    break;
                case "TEXT":
                    s = "String";
                    break;
                case "TIME":
                    s = "DateTime";
                    break;
                case "VARCHAR":
                    if (maxLength == 36)
                    {
                        s = "Guid";
                    }
                    else
                    {
                        s = "String";
                    }
                    break;
                default:
                    s = "Object";
                    break;
            }
            return string.Format("System.{0}", s);
        }
        public string GetDotNetDefaultValue(string defaultVal, string dbDataTypeName, long maxLength)
        {
            return string.Empty;
        }

    }
}
