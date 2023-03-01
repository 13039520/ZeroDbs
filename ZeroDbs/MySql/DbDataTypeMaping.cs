using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace ZeroDbs.MySql
{
    internal class DbDataTypeMaping : IDataTypeMaping
    {
        public string GetDotNetTypeString(int dbDataTypeIntValue, long maxLength)
        {
            string s = Enum.GetName(typeof(MySqlDbType), dbDataTypeIntValue);
            return GetDotNetTypeString(s, maxLength);
        }
        public string GetDotNetTypeString(string dbDataTypeName, long maxLength)
        {
            string s = "";
            switch (dbDataTypeName)
            {
                case "int":
                    s = "Int32";
                    break;
                case "tinyint":
                    if (maxLength != 1)
                    {
                        s = "Byte";
                    }
                    else
                    {
                        s = "Boolean";
                    }
                    break;
                case "smallint":
                    s = "Int16";
                    break;
                case "mediumint":
                    s = "Int32";
                    break;
                case "bigint":
                    s = "Int64";
                    break;
                case "decimal":
                    s = "Decimal";
                    break;
                case "float":
                    s = "Single";
                    break;
                case "double":
                    s = "Double";
                    break;
                case "bit":
                    s = "Boolean";
                    break;
                case "boolean":
                    s = "Boolean";
                    break;
                case "bool":
                    s = "Boolean";
                    break;
                case "date":
                    s = "DateTime";
                    break;
                case "datetime":
                    s = "DateTime";
                    break;
                case "timestamp":
                    s = "DateTime"; //"TimeSpan";
                    break;
                case "time":
                    s = "DateTime";
                    break;
                case "year":
                    s = "String";//有效数字范围：1901-2155
                    break;
                case "char":
                    if (maxLength == 36)
                    {
                        s = "Guid";
                    }
                    else
                    {
                        s = "String";
                    }
                    break;
                case "varchar":
                    s = "String";
                    break;
                case "tinytext":
                    s = "String";
                    break;
                case "text":
                    s = "String";
                    break;
                case "mediumtext":
                    s = "String";
                    break;
                case "longtext":
                    s = "String";
                    break;
                case "binary":
                    s = "Byte[]";
                    break;
                case "varbinary":
                    s = "Byte[]";
                    break;
                default:
                    s = "Object";
                    break;
            }
            return string.Format("System.{0}",s);
        }
        public string GetDotNetDefaultValue(string defaultVal, string dbDataTypeName, long maxLength)
        {
            string s = string.Empty;
            if (!string.IsNullOrEmpty(defaultVal))
            {
                switch (dbDataTypeName)
                {
                    case "int":
                        s = GetNumberDefaultValue(defaultVal, "");
                        break;
                    case "tinyint":
                        //s = GetNumberDefaultValue(DefaultVal, "");//"byte";
                        //一律映射为bool型
                        defaultVal = defaultVal.ToLower().Replace("(", "").Replace(")", "");
                        if (defaultVal.Length == 0 || defaultVal == "0" || defaultVal == "false" || defaultVal == "b'false'" || defaultVal == "b'0'")
                        {
                            s = "false";
                        }
                        else
                        {
                            s = "true";
                        }
                        break;
                    case "smallint":
                        s = GetNumberDefaultValue(defaultVal, "");
                        break;
                    case "mediumint":
                        s = GetNumberDefaultValue(defaultVal, "");
                        break;
                    case "bigint":
                        s = GetNumberDefaultValue(defaultVal, "L");
                        break;
                    case "decimal":
                        s = GetNumberDefaultValue(defaultVal, "M");
                        break;
                    case "float":
                        s = GetNumberDefaultValue(defaultVal, "F");
                        break;
                    case "double":
                        s = GetNumberDefaultValue(defaultVal, "D");
                        break;
                    case "bit":
                        defaultVal = defaultVal.ToLower().Replace("(", "").Replace(")", "");
                        if (defaultVal.Length == 0 || defaultVal == "0" || defaultVal == "false" || defaultVal == "b'false'" || defaultVal == "b'0'")
                        {
                            s = "false";
                        }
                        else
                        {
                            s = "true";
                        }
                        break;
                    case "date":
                        s = GetDateTimeDefaultValue(defaultVal);
                        break;
                    case "datetime":
                        s = GetDateTimeDefaultValue(defaultVal);
                        break;
                    case "timestamp":
                        s = GetDateTimeDefaultValue(defaultVal);
                        break;
                    case "time":
                        s = GetDateTimeDefaultValue(defaultVal);
                        break;
                    case "year":
                        s = GetStringDefaultValue(defaultVal);//有效数字范围：1901-2155
                        break;
                    case "char":
                        if (maxLength != 36)
                        {
                            s = GetStringDefaultValue(defaultVal);
                        }
                        else
                        {
                            s = GetGuidDefaultValue(defaultVal);
                        }
                        break;
                    case "varchar":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "tinytext":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "text":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "mediumtext":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "longtext":
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "binary":

                        break;
                    case "varbinary":

                        break;
                    default:
                        
                        break;
                }
            }
            return s;
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
                    val = "";
                }
            }
            return val;
        }
        private string GetGuidDefaultValue(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                val = System.Text.RegularExpressions.Regex.Replace(val, @"(^"")|(""$)", "");
                val = System.Text.RegularExpressions.Regex.Replace(val, @"(^\(')|('\)$)", "");
                if (string.Equals(val.ToLower(), "uuid()") || string.Equals(val.ToLower(), "newid()"))
                {
                    val = "System.Guid.NewGuid()";
                }
                else
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(val, @"^\{?([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}?$"))
                    {
                        val = "new System.Guid(\"" + val + "\")";
                    }
                    else
                    {
                        val = "";
                    }
                }
            }
            return val;
        }
        private string GetDateTimeDefaultValue(string val)
        {
            string DatePattern = @"(\d{2,4}[/-]\d{1,2}[/-]\d{1,2}(\s\d{1,2}:\d{1,2}:\d{1,2}(:\d{1,3}))?)|(#[\d/:]+#)";
            if (System.Text.RegularExpressions.Regex.IsMatch(val, @"CURRENT_TIMESTAMP"))
            {
                val = "DateTime.Now";
            }
            else
            {
                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(val, DatePattern);
                if (m.Success)
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(m.Value, @"^[0\-\:]+$"))
                    {
                        val = "Convert.ToDateTime(\"" + m.Value + "\")";
                    }
                    else
                    {
                        val = "";
                    }
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
