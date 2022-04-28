using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface ITableInfo: ICloneable
    {
        string DbName { get; }
        /// <summary>
        /// 数据表（或视图）名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 数据表（或视图）描述
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 是不是视图
        /// </summary>
        bool IsView { get; }
        /// <summary>
        /// 字段集合
        /// </summary>
        List<IColumnInfo> Colunms { get; }
    }
}
