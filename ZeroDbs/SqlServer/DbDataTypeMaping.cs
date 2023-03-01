using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.SqlServer
{
    internal class DbDataTypeMaping: IDataTypeMaping
    {
        List<string> rawTypeNames = new List<string>();
        List<string> dotnetTypeNames = new List<string>();
        public DbDataTypeMaping()
        {
            rawTypeNames.AddRange("bigint,binary,bit,char,date,datetime,datetime2,datetimeoffset,decimal,float,geography,geometry,hierarchyid,imageint,money,nchar,ntext,numeric,nvarchar,real,smalldatetime,smallint,smallmoney,sql_variant,text,time,timestamp,tinyint,uniqueidentifier,varbinary,varchar,xml".Split(','));
            dotnetTypeNames.AddRange("Int64,Byte[],Boolean,String,DateTime,DateTime,DateTime,DateTimeOffset,Decimal,Double,Byte[],Byte[],Byte[],Byte[],Int32,Decimal,String,String,Decimal,String,Single,DateTime,Int16,Decimal,Object,String,TimeSpan,Byte[],Byte,Guid,Byte[],String,String".Split(','));
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
            string s = dotnetTypeNames[index];
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
