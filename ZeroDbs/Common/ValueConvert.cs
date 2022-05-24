using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    internal static class ValueConvert
    {
        public static string SqlValueStrByValue(object value, string datetimeFormat = "")
        {
            if (value is null)
            {
                return "null";
            }
            string s = value.ToString();
            Type t = value.GetType();
            if (t.Name== "Nullable`1")
            {
                t = t.UnderlyingSystemType;
            }
            switch (t.Name)
            {
                case "Boolean":
                    if ((bool)value)
                    {
                        s = "1";
                    }
                    else
                    {
                        s = "0";
                    }
                    break;
                case "Char":
                    s = string.Format("'{0}'", s);
                    break;
                case "DateTime":
                    if (string.IsNullOrEmpty(datetimeFormat))
                    {
                        s = string.Format("'{0}'", s);
                    }
                    else
                    {
                        s = string.Format("'{0}'",Convert.ToDateTime(s).ToString(datetimeFormat));
                    }
                    break;
                case "TimeSpan":
                    s = string.Format("'{0}'", s);
                    break;
                case "String":
                    //重复替换两次单引号以保证转义过的单引号被再次转义
                    s = String.Format("'{0}'", value.ToString().Replace("''", "'").Replace("'", "''"));
                    break;
                case "Guid":
                    s = string.Format("'{0}'", s);
                    break;
                case "Single":
                    float f = Convert.ToSingle(value);
                    if (Single.IsNaN(f)) { s = "0"; }
                    if (Single.IsNegativeInfinity(f)) { s = Single.MinValue.ToString(); }
                    if (Single.IsPositiveInfinity(f)) { s = Single.MaxValue.ToString(); }
                    break;
                case "Double":
                    double d = Convert.ToDouble(value);
                    if (Double.IsNaN(d)) { s = "0"; }
                    if (Double.IsNegativeInfinity(d)) { s = Double.MinValue.ToString(); }
                    if (Double.IsPositiveInfinity(d)) { s = Double.MaxValue.ToString(); }
                    break;
            }
            return s;
        }
        public static bool StrToTargetType(string str, Type type, out object result)
        {
            if(type == null)
            {
                throw new ArgumentNullException("type");
            }
            if(type.Equals(typeof(string)))
            {
                result = str;
                return true;
            }
            if(str is null) { 
                result = GetDefaultValue(type);
                return false;
            }
            if (type.Equals(typeof(DateTime)))
            {
                DateTime v;
                if (DateTime.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(DateTime);
                return false;
            }
            if (type.Equals(typeof(byte)))
            {
                byte v;
                if(byte.TryParse(str,out v))
                {
                    result = v;
                    return true;
                }
                result = default(byte);
                return false;
            }
            if (type.Equals(typeof(short)))
            {
                short v;
                if (short.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(short);
                return false;
            }
            if (type.Equals(typeof(int)))
            {
                int v;
                if (int.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(int);
                return false;
            }
            if (type.Equals(typeof(long)))
            {
                long v;
                if (long.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(long);
                return false;
            }
            if (type.Equals(typeof(float)))
            {
                float v;
                if (float.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(float);
                return false;
            }
            if (type.Equals(typeof(double)))
            {
                double v;
                if (double.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(double);
                return false;
            }
            if (type.Equals(typeof(decimal)))
            {
                decimal v;
                if (decimal.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(decimal);
                return false;
            }
            if (type.Equals(typeof(ushort)))
            {
                ushort v;
                if (ushort.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(ushort);
                return false;
            }
            if (type.Equals(typeof(uint)))
            {
                uint v;
                if (uint.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(uint);
                return false;
            }
            if (type.Equals(typeof(ulong)))
            {
                ulong v;
                if (ulong.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(ulong);
                return false;
            }
            if (type.Equals(typeof(bool)))
            {
                bool v;
                if (bool.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(bool);
                return false;
            }
            if (type.Equals(typeof(Guid)))
            {
                Guid v;
                if (Guid.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(Guid);
                return false;
            }
            if (type.Equals(typeof(char)))
            {
                char v;
                if (char.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(char);
                return false;
            }
            if (type.Equals(typeof(TimeSpan)))
            {
                TimeSpan v;
                if (TimeSpan.TryParse(str, out v))
                {
                    result = v;
                    return true;
                }
                result = default(TimeSpan);
                return false;
            }
            if (type.IsEnum)
            {
                string[] s = Enum.GetNames(type);
                int i = 0;
                while (i < s.Length)
                {
                    if (string.Equals(s[i], str, StringComparison.OrdinalIgnoreCase))
                    {
                        result = Enum.Parse(type, str);
                        return true;
                    }
                    i++;
                }
            }
            result = GetDefaultValue(type);
            return false;
        }
        public static bool StrToTargetType(string str, string typeName, out object result)
        {
            string s1 = typeName.ToLower();
            if (s1 == "string")
            {
                return StrToTargetType(str, typeof(string), out result);
            }
            if (s1 == "int16" || s1 == "short")
            {
                return StrToTargetType(str, typeof(short), out result);
            }
            if (s1 == "uint16" || s1 == "ushort")
            {
                return StrToTargetType(str, typeof(ushort), out result);
            }
            if (s1 == "int" || s1 == "int32")
            {
                return StrToTargetType(str, typeof(int), out result);
            }
            if (s1 == "u" || s1 == "uint" || s1 == "uint32")
            {
                return StrToTargetType(str, typeof(uint), out result);
            }
            if (s1 == "l" || s1 == "long" || s1 == "int64")
            {
                return StrToTargetType(str, typeof(long), out result);
            }
            if (s1 == "ul" || s1 == "ulong" || s1 == "uint64")
            {
                return StrToTargetType(str, typeof(ulong), out result);
            }
            if (s1 == "f" || s1 == "single" || s1 == "float")
            {
                return StrToTargetType(str, typeof(float), out result);
            }
            if (s1 == "d" || s1 == "double")
            {
                return StrToTargetType(str, typeof(double), out result);
            }
            if (s1 == "m" || s1 == "decimal")
            {
                return StrToTargetType(str, typeof(decimal), out result);
            }
            if (s1 == "dt" || s1 == "datetime")
            {
                return StrToTargetType(str, typeof(DateTime), out result);
            }
            if (s1 == "timespan")
            {
                return StrToTargetType(str, typeof(TimeSpan), out result);
            }
            if (s1 == "guid")
            {
                return StrToTargetType(str, typeof(Guid), out result);
            }
            if (s1 == "bool" || s1 == "boolean")
            {
                str = str.ToLower();
                result = str == "1" || str == "true";
                return true;
            }
            if (s1 == "byte")
            {
                return StrToTargetType(str, typeof(byte), out result);
            }
            if (s1 == "sbyte")
            {
                return StrToTargetType(str, typeof(sbyte), out result);
            }
            if (s1 == "char")
            {
                return StrToTargetType(str, typeof(char), out result);
            }
            Type type = Type.GetType("System." + typeName);
            if(type != null)
            {
                return StrToTargetType(str, type, out result);
            }
            result = null;
            return false;
        }
        public static bool IsBoolType(Type type)
        {
            return type == boolType || type == boolNullableType;
        }
        public static bool GetBool(object value)
        {
            if (value == null) { return false; }
            if (value is bool)
            {
                return (bool)value;
            }
            if (value is string)
            {
                string s = value.ToString().ToLower();
                return s == "true" || s == "1";
            }
            if (value is decimal)
            {
                return 0M != (decimal)value;
            }
            if (value is float || value is double)
            {
                return 0D != (double)value;
            }
            if (value is long || value is int || value is short || value is byte)
            {
                return 0L != (long)value;
            }
            if (value is Guid)
            {
                return Guid.Empty != (Guid)value;
            }
            return false;
        }
        public static object GetDefaultValue(Type parameter)
        {
            return parameter.IsValueType ? Activator.CreateInstance(parameter) : null;
        }

        private static Type boolType = typeof(bool);
        private static Type boolNullableType = typeof(Nullable<bool>);
        
    }
}
