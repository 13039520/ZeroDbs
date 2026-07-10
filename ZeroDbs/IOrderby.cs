using System;

namespace ZeroDbs
{
    /// <summary>
    /// 排序字段
    /// </summary>
    public interface IOrderby
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Field { get; }
        /// <summary>
        /// ASC 或 DESC
        /// </summary>
        public bool IsAscending { get; }
    }
}
