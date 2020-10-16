using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces.Common
{
    internal static class DbDataReaderToEntity<T> where T :class,new()
    {
        public static T Entity(System.Data.IDataReader DataReader)
        {
            Dictionary<string, DataReaderInfo> FieldDic = new Dictionary<string, DataReaderInfo>();
            int i = 0;
            while (i < DataReader.FieldCount)
            {
                string Name = DataReader.GetName(i);
                FieldDic.Add(Name.ToLower(), new DataReaderInfo { Index = i, Name = Name, FieldType = DataReader.GetFieldType(i) });
                i++;
            }

            List<T> Li = new List<T>();
            while (Li.Count < 1 && DataReader.Read())
            {
                T obj = (T)Activator.CreateInstance(typeof(T));
                System.Reflection.PropertyInfo[] pis = obj.GetType().GetProperties();
                int j = 0;
                while (j < pis.Length)
                {
                    string fieldName = pis[j].Name;
                    string name = fieldName.ToLower();
                    if (FieldDic.ContainsKey(name))
                    {
                        DataReaderInfo info = FieldDic[name];
                        string key = info.Name;
                        int index = info.Index;
                        if (TargetTypeIsBool(pis[j].PropertyType))
                        {
                            pis[j].SetValue(obj, ConverToBool(DataReader[key]), null);
                        }
                        else
                        {
                            object o;
                            try
                            {
                                o = DataReader.GetValue(index);
                            }
                            catch
                            {
                                o = DBNull.Value;
                            }
                            if (DBNull.Equals(DBNull.Value, o))
                            {
                                o = GetDefaultValue(pis[j].PropertyType);
                            }
                            pis[j].SetValue(obj, o, null);
                        }
                    }
                    j++;
                }
                Li.Add(obj);
            }
            DataReader.Close();
            return Li.Count > 0 ? Li[0] : default(T);
        }
        public static List<T> EntityList(System.Data.IDataReader DataReader)
        {
            Dictionary<string, DataReaderInfo> FieldDic = new Dictionary<string, DataReaderInfo>();
            int i = 0;
            while (i < DataReader.FieldCount)
            {
                string Name = DataReader.GetName(i);
                FieldDic.Add(Name.ToLower(), new DataReaderInfo { Index=i, Name = Name, FieldType= DataReader.GetFieldType(i) });
                i++;
            }

            List<T> Li = new List<T>();

            while (DataReader.Read())
            {
                T obj = (T)Activator.CreateInstance(typeof(T));
                System.Reflection.PropertyInfo[] pis = obj.GetType().GetProperties();
                int j = 0;
                while (j < pis.Length)
                {
                    string fieldName = pis[j].Name;
                    string name = fieldName.ToLower();
                    if (FieldDic.ContainsKey(name))
                    {
                        DataReaderInfo info = FieldDic[name];
                        string key = info.Name;
                        int index = info.Index;
                        if (TargetTypeIsBool(pis[j].PropertyType))
                        {
                            pis[j].SetValue(obj, ConverToBool(DataReader[key]), null);
                        }
                        else
                        {
                            object o;
                            try
                            {
                                o = DataReader.GetValue(index);
                            }
                            catch
                            {
                                o = DBNull.Value;
                            }
                            if (DBNull.Equals(DBNull.Value, o))
                            {
                                o = GetDefaultValue(pis[j].PropertyType);
                            }
                            pis[j].SetValue(obj, o, null);
                        }
                    }
                    j++;
                }
                Li.Add(obj);
            }
            DataReader.Close();
            return Li;
        }

        public static T EntityByEmit(System.Data.IDataReader DataReader)
        {
            Dictionary<string, string> FieldDic = new Dictionary<string, string>();
            int i = 0;
            while (i < DataReader.FieldCount)
            {
                string Name = DataReader.GetName(i);
                if (!FieldDic.ContainsKey(Name.ToLower()))
                {
                    FieldDic.Add(Name.ToLower(), Name);
                }
                i++;
            }
            List<T> Li = new List<T>();
            while (Li.Count < 1 && DataReader.Read())
            {
                T obj = new T();
                EntityPropertyEmitSetter[] ps = EntityPropertyEmitSetter.GetProperties(typeof(T));
                int j = 0;
                while (j < ps.Length)
                {
                    string fieldName = ps[j].Info.Name;
                    string name = fieldName.ToLower();
                    if (FieldDic.ContainsKey(name))
                    {
                        ps[j].Setter(obj, DataReader[FieldDic[name]]);
                    }
                    j++;
                }
                Li.Add(obj);
            }
            DataReader.Close();
            return Li.Count > 0 ? Li[0] : default(T);
        }

        public static List<T> EntityListByEmit(System.Data.IDataReader DataReader)
        {
            Dictionary<string, string> FieldDic = new Dictionary<string, string>();
            int i = 0;
            while (i < DataReader.FieldCount)
            {
                string Name = DataReader.GetName(i);
                if (!FieldDic.ContainsKey(Name.ToLower()))
                {
                    FieldDic.Add(Name.ToLower(), Name);
                }
                i++;
            }

            List<T> Li = new List<T>();
            EntityPropertyEmitSetter[] ps = EntityPropertyEmitSetter.GetProperties(typeof(T));
            while (DataReader.Read())
            {
                T obj = new T();
                int num = 0;
                while (num < ps.Length)
                {
                    EntityPropertyEmitSetter p = ps[num];
                    string fieldName = p.Info.Name;
                    string name = fieldName.ToLower();
                    if (!FieldDic.ContainsKey(name))
                    {
                        num++;
                        continue;
                    }
                    p.Setter(obj, DataReader[FieldDic[name]]);
                    num++;
                }
                Li.Add(obj);
            }
            DataReader.Close();
            return Li;
        }

        public static bool TargetTypeIsBool(Type TargetType)
        {
            bool TargetTypeIsBool = false;
            string s = TargetType.Name;
            if (s == "Boolean")
            {
                TargetTypeIsBool = true;
            }
            else if (s == "Nullable`1")
            {
                s = TargetType.FullName;
                if (s.IndexOf("System.Boolean") > 0)
                {
                    TargetTypeIsBool = true;
                }
            }
            return TargetTypeIsBool;
        }
        public static bool ConverToBool(object TargetValue)
        {
            bool Val = false;
            string s = s = TargetValue != null ? TargetValue.ToString().ToLower() : "";
            if (s.Length < 1 || s == "false" || s == "0")
            {
                Val = false;
            }
            else
            {
                Val = true;
            }
            return Val;
        }
        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static object GetDefaultValue(Type parameter)
        {
            return parameter.IsValueType ? Activator.CreateInstance(parameter) : null;
        }

        class DataReaderInfo
        {
            public int Index { get; set; }
            public string Name { get; set; }
            public Type FieldType { get; set; }
        }
    }
}
