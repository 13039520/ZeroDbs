using System;
using System.Collections.Generic;

namespace ZeroDbs
{
    /// <summary>
    /// 键值对集合(Key不区分大小写及不能重复)
    /// </summary>
    public interface IKeyValueOptions: IEnumerable<KeyValuePair<string,object>>
    {
        /// <summary>
        /// 元素数量
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[string key] { get;set; }
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetPairs();
        IKeyValueOptions Add(string key, object value);
        IKeyValueOptions Add(IDictionary<string, object> dictionary);
        IKeyValueOptions Remove(string key);
        bool TryGetValue(string key, out object value);
        bool ContainsKey(string key);
        void Clear();
    }
}
