using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// 数据库数据写入参数校正器
    /// </summary>
    public interface IDbParameterCreator
    {
        string DbType {  get; }
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <returns></returns>
        System.Data.Common.DbParameter Create();
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="pName">参数名</param>
        /// <param name="pValue">参数值</param>
        /// <returns></returns>
        System.Data.Common.DbParameter Create(string pName, object pValue);
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="pName">参数名</param>
        /// <param name="dbType">参数数据类型</param>
        /// <param name="size">最大字节数</param>
        /// <param name="pValue">参数值</param>
        /// <returns></returns>
        System.Data.Common.DbParameter Create(string pName, System.Data.DbType dbType, int size, object pValue);
    }
}
