using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// 代码生成上下文
    /// </summary>
    public interface ICodeGenerateContext
    {
        /// <summary>
        /// 引用命名空间
        /// </summary>
        string UsingNamespaces {  get; }
        /// <summary>
        /// 类的命名空间
        /// </summary>
        string ClassNamespace { get; }
        /// <summary>
        /// 是不是分部类
        /// </summary>
        bool IsPartial { get; }
        /// <summary>
        /// 包含静态方法
        /// </summary>
        bool IncludeStaticMethod { get; }
        /// <summary>
        /// 当前(表)实体类代码
        /// </summary>
        string CurrentTableModelCode {  get; }
        /// <summary>
        /// 当前数据表
        /// </summary>
        ITable CurrentTable {  get; }
    }
}
