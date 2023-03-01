using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Sqlite
{
    internal class DbDataTypeMaping: IDataTypeMaping
    {
        List<string> rawTypeNames = new List<string>();
        List<string> dotnetTypeNames = new List<string>();
        public DbDataTypeMaping()
        {
            rawTypeNames.AddRange("int,tinyint,smallint,mediumint,bigint,integer,unsigned big int,int2,int8,character,varchar,varying character,char,nchar,native character,nvarchar,text,clob,string,real,double,double precision,float,numeric,decimal,boolean,date,time,datetime,blob,none".Split(','));
            dotnetTypeNames.AddRange("Int32,Int32,Int32,Int32,Int64,Int64,Int64,Int32,Int32,String,String,String,String,String,String,String,String,String,String,Double,Double,Double,Single,Decimal,Decimal,Boolean,DateTime,DateTime,DateTime,Byte[],Object".Split(','));
        }
        public string GetDotNetTypeString(int dbDataTypeIntValue, long maxLength)
        {
            string s = Enum.GetName(typeof(System.Data.SqlDbType), dbDataTypeIntValue);
            return GetDotNetTypeString(s, maxLength);
        }
        public string GetDotNetTypeString(string dbDataTypeName, long maxLength)
        {
            int index = rawTypeNames.IndexOf(dbDataTypeName);
            if (index < 0) { return "System.Object"; }
            string s = rawTypeNames[index];
            if (s.IndexOf('.') < 0)
            {
                s = String.Format("System.{0}", s);
            }
            return s;
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
