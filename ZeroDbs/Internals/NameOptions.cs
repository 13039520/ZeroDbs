using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZeroDbs
{
    /// <summary>
    /// 字段项
    /// </summary>
    internal class NameOptions : INameOptions
    {
        readonly HashSet<string> _fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public int Count { get {  return _fields.Count; } }
        public NameOptions()
        {
            
        }
        public NameOptions(string name)
        {
            Add(name);
        }
        public NameOptions(IEnumerable<string> fields)
        {
            AddRange(fields);
        }
        public INameOptions Add(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _fields.Add(name);
            }
            return this;
        }
        public INameOptions AddRange(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    _fields.Add(name);
                }
            }
            return this;
        }
        public INameOptions Parse(string multiNameString)
        {
            string[] names = _FieldNameEx(multiNameString);
            foreach (var name in names)
            {
                _fields.Add(name);
            }
            return this;
        }
        public void Clear()
        {
            _fields.Clear();
        }
        public string[] GetNames() { return _fields.ToArray(); }
        public bool Contains(string name)
        {
            return _fields.Contains(name);
        }
        public bool ContainsAny(params string[] names)
        {
            foreach (var field in names) {
                if (this._fields.Contains(field)) {  return true; }
            }
            return false;
        }
        public bool ContainsAny(INameOptions names)
        {
            foreach (var field in names)
            {
                if (this._fields.Contains(field)) { return true; }
            }
            return false;
        }
        public IEnumerator<string> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        static Regex _CompiledRegex= new Regex(@"\b([a-zA-Z_][a-zA-Z0-9_]{0,63})\b", RegexOptions.Singleline | RegexOptions.Compiled);
        static string[] _FieldNameEx(string fieldNames)
        {
            if (string.IsNullOrEmpty(fieldNames)) { return new string[0]; }
            MatchCollection mc = _CompiledRegex.Matches(fieldNames);
            if (mc.Count < 1) { return new string[0]; }
            HashSet<string> result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (Match m in mc)
            {
                result.Add(m.Value);
            }
            return result.ToArray();
        }
    }
}
