using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    internal static class ValueConvert
    {
        public static string SqlValueStrByValue(object Value, string datetimeFormat = "")
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
                        if (string.IsNullOrEmpty(datetimeFormat))
                        {
                            s = "'" + Value.ToString() + "'";
                        }
                        else
                        {
                            s = "'" + Convert.ToDateTime(Value).ToString(datetimeFormat) + "'";
                        }
                    }
                    else if (nt.IndexOf("System.TimeSpan") > 0)
                    {
                        s = "'" + Value.ToString() + "'";
                    }
                    else if (nt.IndexOf("System.Guid") > 0)
                    {
                        s = "'" + Value.ToString() + "'";
                    }
                    else if (nt.IndexOf("System.Char") > 0)
                    {
                        s = "'" + Value.ToString() + "'";
                    }
                    else if (nt.IndexOf("System.Single") > 0)
                    {
                        float f = Convert.ToSingle(Value);
                        if (Single.IsNaN(f)) { s = "0"; }
                        if (Single.IsNegativeInfinity(f)) { s = Single.MinValue.ToString(); }
                        if (Single.IsPositiveInfinity(f)) { s = Single.MaxValue.ToString(); }
                    }
                    else if (nt.IndexOf("System.Double") > 0)
                    {

                        double d = Convert.ToDouble(Value);
                        if (Double.IsNaN(d)) { s = "0"; }
                        if (Double.IsNegativeInfinity(d)) { s = Double.MinValue.ToString(); }
                        if (Double.IsPositiveInfinity(d)) { s = Double.MaxValue.ToString(); }
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
                    case "DateTime":
                        s = "'" + Value.ToString() + "'";
                        break;
                    case "TimeSpan":
                        s = "'" + Value.ToString() + "'";
                        break;
                    case "String":
                        //重复替换两次单引号以保证转义过的单引号被再次转义
                        s = null == Value ? "''" : "'" + Value.ToString().Replace("''", "'").Replace("'", "''") + "'";
                        break;
                    case "Guid":
                        s = "'" + Value.ToString() + "'";
                        break;
                    case "Single":
                        float f = Convert.ToSingle(Value);
                        if (Single.IsNaN(f)) { s = "0"; }
                        if (Single.IsNegativeInfinity(f)) { s = Single.MinValue.ToString(); }
                        if (Single.IsPositiveInfinity(f)) { s = Single.MaxValue.ToString(); }
                        break;
                    case "Double":
                        double d = Convert.ToDouble(Value);
                        if (Double.IsNaN(d)) { s = "0"; }
                        if (Double.IsNegativeInfinity(d)) { s = Double.MinValue.ToString(); }
                        if (Double.IsPositiveInfinity(d)) { s = Double.MaxValue.ToString(); }
                        break;
                }
            }
            return s;
        }
        public static object StrToTargetType(string str, Type type)
        {
            string typeName = type.Name;
            object val;
            if(StrToTargetType(str, typeName, out val))
            {
                return val;
            }
            return GetTypeDefaultValue(type);
        }
        public static bool StrToTargetType(string OriginalStr, string typeName, out object result)
        {
            result = null;
            string s1 = typeName.ToLower();
            string s2 = OriginalStr;
            if (s1 == "string")
            {
                result = s2;
                return true;
            }
            if (s1 == "int16" || s1 == "short")
            {
                short v1;
                if (short.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "uint16" || s1 == "ushort")
            {
                ushort v1;
                if (ushort.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "int" || s1 == "int32")
            {
                int v1;
                if (int.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "u" || s1 == "uint" || s1 == "uint32")
            {
                uint v1;
                if (uint.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "l" || s1 == "long" || s1 == "int64")
            {
                long v1;
                if (long.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "ul" || s1 == "ulong" || s1 == "uint64")
            {
                ulong v1;
                if (ulong.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "f" || s1 == "single" || s1 == "float")
            {
                float v1;
                if (float.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "d" || s1 == "double")
            {
                double v1;
                if (double.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "m" || s1 == "decimal")
            {
                decimal v1;
                if (decimal.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "dt" || s1 == "datetime")
            {
                DateTime v1;
                if (DateTime.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "timespan")
            {
                TimeSpan v1;
                if (TimeSpan.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "guid")
            {
                Guid v1;
                if (Guid.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "bool" || s1 == "boolean")
            {
                s2 = s2.ToLower();
                return s2 == "1" || s2 == "true";
            }
            if (s1 == "byte")
            {
                byte v1;
                if (byte.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "sbyte")
            {
                sbyte v1;
                if (sbyte.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            if (s1 == "char")
            {
                char v1;
                if (char.TryParse(s2, out v1))
                {
                    result = v1;
                    return true;
                }
            }
            return false;
        }
        private static object GetTypeDefaultValue(Type parameter)
        {
            return parameter.IsValueType ? Activator.CreateInstance(parameter) : null;
        }
    }
}
