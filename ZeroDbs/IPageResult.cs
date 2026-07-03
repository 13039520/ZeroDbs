using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// 分页查询结果
    /// </summary>
    public interface IPageResult<T>
    {
        long Total {  get; }
        int Page {  get; }
        int Size {  get; }
        List<T> Rows { get; }
    }
}
