using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface IColumnInfo
    {
        /// <summary>
        /// 列名
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 数据类型
        /// </summary>
        string Type { get; }
        /// 是否允许为空
        /// </summary>
        bool IsNullable { get; }
        /// <summary>
        /// 是否主键
        /// </summary>
        bool IsPrimaryKey { get; }
        /// <summary>
        /// 是否自动增长
        /// </summary>
        bool IsIdentity { get; }
        /// <summary>
        /// 占用字节
        /// </summary>
        long Byte { get; }
        /// <summary>
        /// 长度限制
        /// </summary>
        long MaxLength { get; }
        /// <summary>
        /// 小数位数（0表示无）
        /// </summary>
        int DecimalDigits { get; }
        /// <summary>
        /// 默认值
        /// </summary>
        string DefaultValue { get; }
        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; }
    }
}
