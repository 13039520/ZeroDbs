using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class KeyValueOptions: IKeyValueOptions
    {
        private readonly Dictionary<string, object> _dict;
        public int Count {  get { return _dict.Count; } }
        public KeyValueOptions()
        {
            _dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
        public KeyValueOptions(IDictionary<string, object> source)
        {
            _dict = new Dictionary<string, object>(source, StringComparer.OrdinalIgnoreCase);
        }
        public object this[string key]
        {
            get => _dict[key];
            set => _dict[key] = value;
        }
        public Dictionary<string, object> GetPairs()
        {
            return _dict;
        }
        public IKeyValueOptions Add(string key, object value)
        {
            _dict[key] = value;
            return this;
        }
        public IKeyValueOptions Add(IDictionary<string, object> dictionary)
        {
            if (dictionary == null) { return this; }
            foreach (var key in dictionary.Keys)
            {
                _dict[key] = dictionary[key];
            }
            return this;
        }
        public IKeyValueOptions Remove(string key)
        {
            _dict.Remove(key);
            return this;
        }
        public bool TryGetValue(string key, out object value)
        {
            return _dict.TryGetValue(key, out value);
        }
        public bool ContainsKey(string key)
        {
            return _dict.ContainsKey(key);
        }
        public void Clear()
        {
            _dict.Clear();
        }
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
