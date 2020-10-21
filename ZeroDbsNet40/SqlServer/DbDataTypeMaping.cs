using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.SqlServer
{
    internal class DbDataTypeMaping: IDataTypeMaping
    {
        public string GetDotNetTypeString(int dbDataTypeIntValue, long maxLength)
        {
            string s = Enum.GetName(typeof(System.Data.SqlDbType), dbDataTypeIntValue);
            return GetDotNetTypeString(s, maxLength);
        }
        public string GetDotNetTypeString(string dbDataTypeName, long maxLength)
        {
            Dictionary<string, string> mapDic = GetTypeMapDic();
            string t = dbDataTypeName.ToLower();
            //如果不存在于哈希表中则多半是用户自定义的数据类型，默认用string代替
            return mapDic.ContainsKey(t) ? mapDic[t].ToString() : "object";
        }
        public string GetDotNetDefaultValue(string defaultVal, string dbDataTypeName, long maxLength)
        {
            string s = string.Empty;
            if (!string.IsNullOrEmpty(defaultVal))
            {
                switch (dbDataTypeName.ToLower())
                {
                    case "bigint"://Int64 long
                        s = GetNumberDefaultValue(defaultVal, "L");
                        break;
                    case "binary"://byte[]

                        break;
                    case "bit"://bool
                        if (!string.IsNullOrEmpty(defaultVal))
                        {
                            defaultVal = defaultVal.ToLower().Replace("(", "").Replace(")", "");
                            if (defaultVal == "1" || defaultVal == "true")
                            {
                                s = "true";
                            }
                            else if (defaultVal == "0" || defaultVal == "false")
                            {
                                s = "false";
                            }
                        }
                        break;
                    case "char"://string
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "date"://DateTime
                        s = GetDateTimeDefaultValue(defaultVal);
                        break;
                    case "datetime"://DateTime
                        s = GetDateTimeDefaultValue(defaultVal);
                        break;
                    case "datetime2"://DateTime
                        s = GetDateTimeDefaultValue(defaultVal);
                        break;
                    case "datetimeoffset"://DateTimeOffset

                        break;
                    case "decimal"://decimal
                        s = GetNumberDefaultValue(defaultVal, "M");
                        break;
                    case "float"://double
                        s = GetNumberDefaultValue(defaultVal, "D");
                        break;
                    case "image"://byte[]

                        break;
                    case "int"://Int32
                        s = GetNumberDefaultValue(defaultVal, "");
                        break;
                    case "money"://decimal
                        s = GetNumberDefaultValue(defaultVal, "M");
                        break;
                    case "nchar"://string
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "ntext"://string
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "numeric"://decimal
                        s = GetNumberDefaultValue(defaultVal, "M");
                        break;
                    case "nvarchar"://string
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "real"://Single float
                        s = GetNumberDefaultValue(defaultVal, "F");
                        break;
                    case "rowversion"://Byte[]

                        break;
                    case "smalldatetime"://DateTime
                        s = GetDateTimeDefaultValue(defaultVal);
                        break;
                    case "smallint"://Int16 short
                        s = GetNumberDefaultValue(defaultVal, "");
                        break;
                    case "smallmoney"://decimal
                        s = GetNumberDefaultValue(defaultVal, "M");
                        break;
                    case "sql_variant"://Object*

                        break;
                    case "text"://string
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "time"://TimeSpan

                        break;
                    case "timestamp"://byte[]

                        break;
                    case "tinyint"://byte

                        break;
                    case "uniqueidentifier"://Guid
                        if (System.Text.RegularExpressions.Regex.IsMatch(defaultVal, @"newid\(\)", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                        {
                            s = "System.Guid.NewGuid()";
                        }
                        else
                        {
                            s = "System.Guid.Empty";
                        }
                        break;
                    case "varbinary"://byte[]

                        break;
                    case "varchar"://string
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "xml"://Xml

                        break;
                    default:

                        break;
                }
            }
            return s;
        }

        private static Dictionary<string, string> typeMapDic = null;
        private static object _lock = new object();
        private Dictionary<string,string> GetTypeMapDic()
        {
            if (typeMapDic != null)
            {
                return typeMapDic;
            }
            lock (_lock)
            {
                if (typeMapDic != null)
                {
                    return typeMapDic;
                }
                typeMapDic = new Dictionary<string, string>();
                //System.Collections.Specialized ht = new System.Collections.Hashtable();
                typeMapDic.Add("bigint", "long");//Int64 long
                typeMapDic.Add("binary", "byte[]");//Byte[]
                typeMapDic.Add("bit", "bool");//Boolean
                typeMapDic.Add("char", "string");//String
                typeMapDic.Add("date", "DateTime");
                typeMapDic.Add("datetime", "DateTime");
                typeMapDic.Add("datetime2", "DateTime");
                typeMapDic.Add("datetimeoffset", "DateTimeOffset");
                typeMapDic.Add("decimal", "decimal");//Decimal
                typeMapDic.Add("float", "double");//Double
                typeMapDic.Add("image", "byte[]");//Byte[]
                typeMapDic.Add("int", "int");//Int32
                typeMapDic.Add("money", "decimal");//Decimal
                typeMapDic.Add("nchar", "string");//String
                typeMapDic.Add("ntext", "string");//String
                typeMapDic.Add("numeric", "decimal");//Decimal
                typeMapDic.Add("nvarchar", "string");
                typeMapDic.Add("real", "float");//Single
                typeMapDic.Add("rowversion", "byte[]");
                typeMapDic.Add("smalldatetime", "DateTime");
                typeMapDic.Add("smallint", "short");//Int16
                typeMapDic.Add("smallmoney", "decimal");//Decimal
                typeMapDic.Add("sql_variant", "object*");
                typeMapDic.Add("text", "string");//String
                typeMapDic.Add("time", "TimeSpan");
                typeMapDic.Add("timestamp", "byte[]");//Byte[]
                typeMapDic.Add("tinyint", "byte");//Byte
                typeMapDic.Add("uniqueidentifier", "Guid");
                typeMapDic.Add("varbinary", "byte[]");//Byte[]
                typeMapDic.Add("varchar", "string");//String
                typeMapDic.Add("xml", "Xml");
            }
            return typeMapDic;
        }
        private string GetStringDefaultValue(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                val = val.TrimStart('(').TrimEnd(')');
                if (val.ToLower() != "null")
                {
                    val = val.Trim('\'');
                    val = "\"" + val.Replace("\"", "\\\"") + "\"";
                }
                else
                {
                    val = "\"\"";
                }
            }
            return val;
        }
        private string GetDateTimeDefaultValue(string val)
        {
            string DatePattern = @"\d{2,4}[/-]\d{1,2}[/-]\d{1,2}(\s\d{1,2}:\d{1,2}:\d{1,2}(:\d{1,3}))?";
            if (System.Text.RegularExpressions.Regex.IsMatch(val, @"getdate\(\)"))
            {
                val = "DateTime.Now";
            }
            else
            {
                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(val, DatePattern);
                if (m.Success)
                {
                    val = "Convert.ToDateTime(\"" + m.Value + "\")";
                }
                else
                {
                    val = "";
                }
            }
            return val;
        }
        private string GetNumberDefaultValue(string val, string suffix)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(val, @"-?\d+(\.\d+)?");
            if (m.Success)
            {
                val = m.Value + suffix;
            }
            else
            {
                val = "";
            }
            return val;
        }
    }
}
