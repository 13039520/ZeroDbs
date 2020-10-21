using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Sqlite
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
            string t = System.Text.RegularExpressions.Regex.Replace(dbDataTypeName, @"\s\(\d+\)$", "").ToLower();
            if (mapDic.ContainsKey(t)) { return mapDic[t]; }
            return "object";
        }
        public string GetDotNetDefaultValue(string defaultVal, string dbDataTypeName, long maxLength)
        {
            string s = string.Empty;
            if (!string.IsNullOrEmpty(defaultVal))
            {
                string t = System.Text.RegularExpressions.Regex.Replace(dbDataTypeName, @"\s\(\d+\)$", "");
                switch (t.ToLower())
                {
                    case "int":
                        s = GetNumberDefaultValue(defaultVal, "");
                        break;
                    case "tinyint":
                        s = GetNumberDefaultValue(defaultVal, "");
                        break;
                    case "mediumint":
                        s = GetNumberDefaultValue(defaultVal, "");
                        break;
                    case "bigint":
                        s = GetNumberDefaultValue(defaultVal, "L");
                        break;
                    case "integer":
                        s = GetNumberDefaultValue(defaultVal, "L");
                        break;
                    case "unsigned big int":
                        s = GetNumberDefaultValue(defaultVal, "L");
                        break;
                    case "int2":
                        s = GetNumberDefaultValue(defaultVal, "");
                        break;
                    case "int8":
                        s = GetNumberDefaultValue(defaultVal, "");
                        break;
                    case "character":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "varchar":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "varying character":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "nchar":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "native character":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "nvarchar":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "string":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "text":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "clob":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "char":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "real":
                        s = GetNumberDefaultValue(defaultVal, "D");
                        break;
                    case "double":
                        s = GetNumberDefaultValue(defaultVal, "D");
                        break;
                    case "double precision":
                        s = GetNumberDefaultValue(defaultVal, "D");
                        break;
                    case "float":
                        s = GetNumberDefaultValue(defaultVal, "F");
                        break;
                    case "numeric":
                        s = GetNumberDefaultValue(defaultVal, "M");
                        break;
                    case "decimal":
                        s = GetNumberDefaultValue(defaultVal, "M");
                        break;
                    case "boolean":
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
                    case "date":
                        s = GetDateTimeDefaultValue(defaultVal);
                        break;
                    case "time":
                        s = GetDateTimeDefaultValue(defaultVal);
                        break;
                    case "datetime":
                        s = GetDateTimeDefaultValue(defaultVal);
                        break;
                    case "timestamp":
                        s = GetDateTimeDefaultValue(defaultVal);
                        break;
                    case "blob":
                        s = "null";
                        break;
                    case "none"://object

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

                typeMapDic.Add("int", "int");
                typeMapDic.Add("tinyint", "int");
                typeMapDic.Add("smallint", "int");
                typeMapDic.Add("mediumint", "int");
                typeMapDic.Add("bigint", "long");
                typeMapDic.Add("integer", "long");
                typeMapDic.Add("unsigned big int", "long");
                typeMapDic.Add("int2", "int");
                typeMapDic.Add("int8", "int");

                typeMapDic.Add("character", "string");
                typeMapDic.Add("varchar", "string");
                typeMapDic.Add("varying character", "string");
                typeMapDic.Add("char", "string");
                typeMapDic.Add("nchar", "string");
                typeMapDic.Add("native character", "string");
                typeMapDic.Add("nvarchar", "string");
                typeMapDic.Add("text", "string");
                typeMapDic.Add("clob", "string");
                typeMapDic.Add("string", "string");

                typeMapDic.Add("real", "double");
                typeMapDic.Add("double", "double");
                typeMapDic.Add("double precision", "double");
                typeMapDic.Add("float", "float");

                typeMapDic.Add("numeric", "decimal");
                typeMapDic.Add("decimal", "decimal");
                typeMapDic.Add("boolean", "bool");
                typeMapDic.Add("date", "DateTime");
                typeMapDic.Add("time", "DateTime");
                typeMapDic.Add("datetime", "DateTime");

                typeMapDic.Add("blob", "byte[]");
                typeMapDic.Add("none", "object");


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
            string DatePattern = @"\d{2,4}[/-]\d{1,2}[/-]\d{1,2}(\s\d{1,2}:\d{1,2}:\d{1,2}(:\d{1,3})(\.\d{1,3})?)?";
            if (System.Text.RegularExpressions.Regex.IsMatch(val, @"datetime\('now'\s*,\s*'localtime'\)"))
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
