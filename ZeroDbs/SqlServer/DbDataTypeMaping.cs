using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.SqlServer
{
    internal class DbDataTypeMaping: IDbDataTypeMaping
    {
        public Type GetDotNetType(string dbDataTypeName, long maxLength)
        {
            Type type;
            switch (dbDataTypeName)
            {
                case "bigint":
                    type = typeof(long);
                    break;
                case "binary":
                    type = typeof(byte[]);
                    break;
                case "bit":
                    type = typeof(bool);
                    break;
                case "char":
                    type = typeof(string);
                    break;
                case "date":
                    type = typeof(DateTime);
                    break;
                case "datetime":
                    type = typeof(DateTime);
                    break;
                case "datetime2":
                    type = typeof(DateTime);
                    break;
                case "datetimeoffset":
                    type = typeof(DateTimeOffset);
                    break;
                case "decimal":
                    type = typeof(decimal);
                    break;
                case "float":
                    type = typeof(double);
                    break;
                case "geography":
                    type = typeof(byte[]);
                    break;
                case "geometry":
                    type = typeof(byte[]);
                    break;
                case "hierarchyid":
                    type = typeof(byte[]);
                    break;
                case "image":
                    type = typeof(byte[]);
                    break;
                case "int":
                    type = typeof(int);
                    break;
                case "money":
                    type = typeof(decimal);
                    break;
                case "nchar":
                    type = typeof(string);
                    break;
                case "ntext":
                    type = typeof(string);
                    break;
                case "numeric":
                    type = typeof(decimal);
                    break;
                case "nvarchar":
                    type = typeof(string);
                    break;
                case "real":
                    type = typeof(float);
                    break;
                case "smalldatetime":
                    type = typeof(DateTime);
                    break;
                case "smallint":
                    type = typeof(short);
                    break;
                case "smallmoney":
                    type = typeof(decimal);
                    break;
                case "sql_variant":
                    type = typeof(object);
                    break;
                case "text":
                    type = typeof(string);
                    break;
                case "time":
                    type = typeof(TimeSpan);
                    break;
                case "timestamp":
                    type = typeof(byte[]);
                    break;
                case "tinyint":
                    type = typeof(byte);
                    break;
                case "uniqueidentifier":
                    type = typeof(Guid);
                    break;
                case "varbinary":
                    type = typeof(byte[]);
                    break;
                case "varchar":
                    type = typeof(string);
                    break;
                case "xml":
                    type = typeof(string);
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
            string s = string.Empty;
            if (!string.IsNullOrEmpty(defaultVal))
            {
                switch (dbDataTypeName.ToLower())
                {
                    case "bigint"://Int64 long
                        s = GetNumberDefaultValue(defaultVal, "L");
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
                    case "decimal"://decimal
                        s = GetNumberDefaultValue(defaultVal, "M");
                        break;
                    case "float"://double
                        s = GetNumberDefaultValue(defaultVal, "D");
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
                    case "smalldatetime"://DateTime
                        s = GetDateTimeDefaultValue(defaultVal);
                        break;
                    case "smallint"://Int16 short
                        s = GetNumberDefaultValue(defaultVal, "");
                        break;
                    case "smallmoney"://decimal
                        s = GetNumberDefaultValue(defaultVal, "M");
                        break;
                    case "text"://string
                        s = GetStringDefaultValue(defaultVal);
                        break;
                    case "tinyint"://byte
                        s = GetNumberDefaultValue(defaultVal, "");
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
                    case "varchar"://string
                        s = GetStringDefaultValue(defaultVal);
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
