using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// 排序集合
    /// </summary>
    public interface IOrderbyOptions: IEnumerable<IOrderby>
    {
        /// <summary>
        /// 元素个数
        /// </summary>
        int Count { get; }
        IOrderby this[int index] { get; }
        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="field"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        IOrderbyOptions Add(string field, bool isAscending = true);
        /// <summary>
        /// 清空
        /// </summary>
        void Clear();
        /// <summary>
        /// 获取排序项集合
        /// </summary>
        /// <returns></returns>
        List<IOrderby> GetOrderbies();
    }
}
