using System;
using System.Collections.Generic;

namespace ZeroDbs
{
    /// <summary>
    /// 名称集合项(单词+自动去重复)
    /// </summary>
    public interface INameOptions: IEnumerable<string>
    {
        /// <summary>
        /// 元素个数
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 添加名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        INameOptions Add(string name);
        /// <summary>
        /// 批量添加名称
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        INameOptions AddRange(IEnumerable<string> names);
        /// <summary>
        /// 批量添加名称(从多名称字符串中提取名称：单词提取)
        /// </summary>
        /// <param name="multiNameString"></param>
        /// <returns></returns>
        INameOptions Parse(string multiNameString);
        /// <summary>
        /// 清空
        /// </summary>
        void Clear();
        /// <summary>
        /// 获取字段数组集合
        /// </summary>
        /// <returns></returns>
        string[] GetNames();
        /// <summary>
        /// 是否包含字段
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Contains(string name);
        /// <summary>
        /// 是否包含字段(多字段检测)
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        bool ContainsAny(params string[] names);
        /// <summary>
        /// 是否包含字段(多字段检测，主要用于判断两个对象是否有交集的字段名称)
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        bool ContainsAny(INameOptions names);
    }
}
