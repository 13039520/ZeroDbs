using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// 执行结果
    /// </summary>
    public interface IExecuteResult
    {
        /// <summary>
        /// 查询命令返回结果集
        /// </summary>
        IReadOnlyList<DataTable> Tables { get; }
        /// <summary>
        /// 增删改影响行数
        /// </summary>
        int RecordsAffected { get; }
    }
}
