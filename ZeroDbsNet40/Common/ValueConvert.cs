using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    internal static class ValueConvert
    {
        public static string SqlValueStrByValue(object Value)
        {
            if (Value is null)
            {
                return "NULL";
            }
            string s = Value.ToString();
            Type t = Value.GetType();
            if (t.Name == "Nullable`1")
            {
                string nt = t.FullName;
                if (Value != null)
                {
                    if (nt.IndexOf("System.Boolean") > 0)
                    {
                        if ((bool)Value)
                        {
                            s = "1";
                        }
                        else
                        {
                            s = "0";
                        }
                    }
                    else if (nt.IndexOf("System.DateTime") > 0)
                    {
                        s = "'" + Value.ToString() + "'";
                    }
                    else if (nt.IndexOf("System.TimeSpan") > 0)
                    {
                        s = "'" + Value.ToString() + "'";
                    }
                    else if (nt.IndexOf("System.String") > 0)
                    {
                        //重复替换两次单引号以保证转义过的单引号被再次转义
                        s = "'" + Value.ToString().Replace("''", "'").Replace("'", "''") + "'";
                    }
                    else if (nt.IndexOf("System.Guid") > 0)
                    {
                        s = "'" + Value.ToString() + "'";
                    }
                    else if (nt.IndexOf("System.Byte") > 0)
                    {
                        byte[] tt = new byte[100];
                        tt.ToString();
                        s = "NULL";
                    }
                    else if (nt.IndexOf("System.SByte") > 0)
                    {
                        s = "NULL";
                    }
                    else if (nt.IndexOf("System.Char") > 0)
                    {
                        s = "'" + Value.ToString() + "'";
                    }
                }
                else
                {
                    s = "NULL";
                }
            }
            else
            {
                switch (t.Name)
                {
                    case "Boolean":
                        if ((bool)Value)
                        {
                            s = "1";
                        }
                        else
                        {
                            s = "0";
                        }
                        break;
                    case "Char":
                        s = "'" + (Value.ToString()) + "'";
                        break;
                    case "Byte":
                        s = "NULL";
                        break;
                    case "SByte":
                        s = "NULL";
                        break;
                    case "DateTime":
                        s = "'" + Value.ToString() + "'";
                        break;
                    case "TimeSpan":
                        s = "'" + Value.ToString() + "'";
                        break;
                    case "String":
                        //重复替换两次单引号以保证转义过的单引号被再次转义
                        s = "'" + Value.ToString().Replace("''", "'").Replace("'", "''") + "'";
                        break;
                    case "Guid":
                        s = "'" + Value.ToString() + "'";
                        break;
                }
            }
            return s;
        }
        public static object StrToTargetType(string OriginalStr, Type TargetType)
        {
            object obj = GetTypeDefaultValue(TargetType);
            switch (TargetType.Name)
            {
                case "Boolean":
                    OriginalStr = OriginalStr.ToLower().Trim();
                    if (OriginalStr.Length < 1 || OriginalStr == "0" || OriginalStr == "false")
                    {
                        obj = false;
                    }
                    else
                    {
                        obj = true;
                    }
                    break;
                case "Char":
                    char MyChar;
                    if (Char.TryParse(OriginalStr, out MyChar))
                    {
                        obj = MyChar;
                    }
                    break;
                case "Decimal":
                    Decimal MyDecimal;
                    if (Decimal.TryParse(OriginalStr, out MyDecimal))
                    {
                        obj = MyDecimal;
                    }
                    break;
                case "Double":
                    Double MyDouble;
                    if (Double.TryParse(OriginalStr, out MyDouble))
                    {
                        obj = MyDouble;
                    }
                    break;
                case "Single":
                    Single MySingle;
                    if (Single.TryParse(OriginalStr, out MySingle))
                    {
                        obj = MySingle;
                    }
                    break;
                case "Int32":
                    Int32 MyInt32;
                    if (Int32.TryParse(OriginalStr, out MyInt32))
                    {
                        obj = MyInt32;
                    }
                    break;
                case "Int64":
                    Int64 MyInt64;
                    if (Int64.TryParse(OriginalStr, out MyInt64))
                    {
                        obj = MyInt64;
                    }
                    break;
                case "Int16":
                    Int16 MyInt16;
                    if (Int16.TryParse(OriginalStr, out MyInt16))
                    {
                        obj = MyInt16;
                    }
                    break;
                case "UInt32":
                    UInt32 MyUInt32;
                    if (UInt32.TryParse(OriginalStr, out MyUInt32))
                    {
                        obj = MyUInt32;
                    }
                    break;
                case "UInt64":
                    UInt64 MyUInt64;
                    if (UInt64.TryParse(OriginalStr, out MyUInt64))
                    {
                        obj = MyUInt64;
                    }
                    break;
                case "UInt16":
                    UInt16 MyUInt16;
                    if (UInt16.TryParse(OriginalStr, out MyUInt16))
                    {
                        obj = MyUInt16;
                    }
                    break;
                case "Byte":
                    Byte MyByte;
                    if (Byte.TryParse(OriginalStr, out MyByte))
                    {
                        obj = MyByte;
                    }
                    break;
                case "SByte":
                    SByte MySByte;
                    if (SByte.TryParse(OriginalStr, out MySByte))
                    {
                        obj = MySByte;
                    }
                    break;
                case "DateTime":
                    DateTime MyDateTime;
                    if (DateTime.TryParse(OriginalStr, out MyDateTime))
                    {
                        obj = MyDateTime;
                    }
                    break;
                case "TimeSpan":
                    TimeSpan MyTimeSpan;
                    if (TimeSpan.TryParse(OriginalStr, out MyTimeSpan))
                    {
                        obj = MyTimeSpan;
                    }
                    break;
                case "String":
                    obj = "" + OriginalStr.ToString().Replace("''", "'").Replace("'", "''");
                    break;
                case "Guid":
                    Guid t = Guid.Empty;
                    Guid.TryParse(OriginalStr, out t);
                    obj = t;
                    break;
                case "Nullable`1":
                    obj = StrToTargetNullableType(OriginalStr, TargetType);
                    break;
            }
            return obj;
        }
        private static object StrToTargetNullableType(string OriginalStr, Type TargetType)
        {
            object obj = GetTypeDefaultValue(TargetType);
            string s = TargetType.FullName;

            if (s.IndexOf("System.Boolean") > 0)
            {
                OriginalStr = OriginalStr.ToLower().Trim();
                if (OriginalStr.Length < 1 || OriginalStr == "0" || OriginalStr == "false")
                {
                    obj = false;
                }
                else
                {
                    obj = true;
                }
            }
            else if (s.IndexOf("System.DateTime") > 0)
            {
                DateTime MyDateTime;
                if (DateTime.TryParse(OriginalStr, out MyDateTime))
                {
                    obj = MyDateTime;
                }
            }
            else if (s.IndexOf("System.TimeSpan") > 0)
            {
                TimeSpan MyTimeSpan;
                if (TimeSpan.TryParse(OriginalStr, out MyTimeSpan))
                {
                    obj = MyTimeSpan;
                }
            }
            else if (s.IndexOf("System.String") > 0)
            {
                obj = "" + OriginalStr.ToString();
            }
            else if (s.IndexOf("System.Guid") > 0)
            {
                Guid t = Guid.Empty;
                if (Guid.TryParse(OriginalStr, out t))
                {
                    obj = t;
                }
            }
            else if (s.IndexOf("System.Decimal") > 0)
            {
                Decimal MyDecimal;
                if (Decimal.TryParse(OriginalStr, out MyDecimal))
                {
                    obj = MyDecimal;
                }
            }
            else if (s.IndexOf("System.Double") > 0)
            {
                Double MyDouble;
                if (Double.TryParse(OriginalStr, out MyDouble))
                {
                    obj = MyDouble;
                }
            }
            else if (s.IndexOf("System.Single") > 0)
            {
                Single MySingle;
                if (Single.TryParse(OriginalStr, out MySingle))
                {
                    obj = MySingle;
                }
            }
            else if (s.IndexOf("System.Int32") > 0)
            {
                Int32 MyInt32;
                if (Int32.TryParse(OriginalStr, out MyInt32))
                {
                    obj = MyInt32;
                }
            }
            else if (s.IndexOf("System.Int64") > 0)
            {
                Int64 MyInt64;
                if (Int64.TryParse(OriginalStr, out MyInt64))
                {
                    obj = MyInt64;
                }
            }
            else if (s.IndexOf("System.Int16") > 0)
            {
                Int16 MyInt16;
                if (Int16.TryParse(OriginalStr, out MyInt16))
                {
                    obj = MyInt16;
                }
            }
            else if (s.IndexOf("System.UInt32") > 0)
            {
                UInt32 MyUInt32;
                if (UInt32.TryParse(OriginalStr, out MyUInt32))
                {
                    obj = MyUInt32;
                }
            }
            else if (s.IndexOf("System.UInt64") > 0)
            {
                UInt64 MyUInt64;
                if (UInt64.TryParse(OriginalStr, out MyUInt64))
                {
                    obj = MyUInt64;
                }
            }
            else if (s.IndexOf("System.UInt16") > 0)
            {
                UInt16 MyUInt16;
                if (UInt16.TryParse(OriginalStr, out MyUInt16))
                {
                    obj = MyUInt16;
                }
            }
            else if (s.IndexOf("System.Byte") > 0)
            {
                Byte MyByte;
                if (Byte.TryParse(OriginalStr, out MyByte))
                {
                    obj = MyByte;
                }
            }
            else if (s.IndexOf("System.SByte") > 0)
            {
                SByte MySByte;
                if (SByte.TryParse(OriginalStr, out MySByte))
                {
                    obj = MySByte;
                }
            }
            else if (s.IndexOf("System.Char") > 0)
            {
                char MyChar;
                if (Char.TryParse(OriginalStr, out MyChar))
                {
                    obj = MyChar;
                }
            }
            return obj;
        }
        private static object GetTypeDefaultValue(Type parameter)
        {
            return parameter.IsValueType ? Activator.CreateInstance(parameter) : null;
        }
    }
}
