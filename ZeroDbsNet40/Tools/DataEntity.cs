using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Tools
{
    public static class DataEntity
    {
        public static T Get<T>(System.Collections.Specialized.NameValueCollection NameValueCollection)
            where T : class, new()
        {
            T Result = (T)Activator.CreateInstance(typeof(T));
            System.Reflection.PropertyInfo[] Properties = Result.GetType().GetProperties();
            System.Collections.Hashtable MyHashtable = new System.Collections.Hashtable();
            for (var i = 0; i < NameValueCollection.Keys.Count; i++)
            {
                var key = NameValueCollection.Keys[i].ToLower();
                if (!MyHashtable.ContainsKey(key))
                {
                    MyHashtable.Add(key, NameValueCollection[NameValueCollection.Keys[i]]);
                }
            }
            int HasPropertyCount = 0;
            for (int j = 0; j < Properties.Length; j++)
            {
                string PropertyName = Properties[j].Name;
                if (MyHashtable.Contains(PropertyName.ToLower()))
                {
                    HasPropertyCount++;
                    Properties[j].SetValue(Result, Common.ValueConvert.StrToTargetType(MyHashtable[PropertyName.ToLower()].ToString(), Properties[j].PropertyType), null);
                }
            }
            if (HasPropertyCount < 1)
            {
                Result = null;
            }
            return Result;
        }
        public static T Update<T>(T SourceEntity, System.Collections.Specialized.NameValueCollection NameValueCollection)
            where T : class, new()
        {
            if (SourceEntity == null) { return SourceEntity; }

            T Result = (T)Activator.CreateInstance(typeof(T));
            System.Reflection.PropertyInfo[] Properties = Result.GetType().GetProperties();
            System.Collections.Hashtable MyHashtable = new System.Collections.Hashtable();
            for (var i = 0; i < NameValueCollection.Keys.Count; i++)
            {
                var key = NameValueCollection.Keys[i].ToLower();
                if (!MyHashtable.ContainsKey(key))
                {
                    MyHashtable.Add(key, NameValueCollection[NameValueCollection.Keys[i]]);
                }
            }
            for (int j = 0; j < Properties.Length; j++)
            {
                string PropertyName = Properties[j].Name;
                if (MyHashtable.Contains(PropertyName.ToLower()))
                {
                    Properties[j].SetValue(Result, Common.ValueConvert.StrToTargetType(MyHashtable[PropertyName.ToLower()].ToString(), Properties[j].PropertyType), null);
                }
                else
                {
                    Properties[j].SetValue(Result, Properties[j].GetValue(SourceEntity, null), null);
                }
            }
            return Result;
        }

    }
}
