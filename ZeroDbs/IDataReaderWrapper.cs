using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface IDataReaderWrapper
    {
        /// <summary>
        /// 读取字段泛型值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">字段名(不区分大小写)</param>
        /// <returns></returns>
        T GetValue<T>(string field);
        /// <summary>
        /// 读取字段泛型值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">字段索引</param>
        /// <returns></returns>
        T GetValue<T>(int index);
        /// <summary>
        /// 读取字段泛型值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">字段名(不区分大小写)</param>
        /// <param name="noneValue">自定义默认值(字段不存在或值为null时)</param>
        /// <returns></returns>
        T GetValue<T>(string field, T noneValue);
        /// <summary>
        /// 读取字段泛型值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">字段索引</param>
        /// <param name="noneValue">自定义默认值(字段不存在或值为null时)</param>
        /// <returns></returns>
        T GetValue<T>(int index, T noneValue);
        /// <summary>
        /// 获取字段值(字段不存在时返回null)
        /// </summary>
        /// <param name="field">字段名(不区分大小写)</param>
        /// <returns></returns>
        object? GetValue(string field);
        /// <summary>
        /// 获取字段值(字段不存在时返回null)
        /// </summary>
        /// <param name="index">字段索引</param>
        /// <returns></returns>
        object? GetValue(int index);
        /// <summary>
        /// 获取数据库数据类型(字段不存在时返回null)
        /// </summary>
        /// <param name="field">字段名(不区分大小写)</param>
        /// <returns></returns>
        string? GetDataTypeName(string field);
        /// <summary>
        /// 获取NET运行时数据类型(字段不存在时返回null)
        /// </summary>
        /// <param name="field">字段名(不区分大小写)</param>
        /// <returns></returns>
        Type? GetFieldType(string field);
        /// <summary>
        /// 字段是否存在
        /// </summary>
        /// <param name="field">字段名(不区分大小写)</param>
        /// <returns></returns>
        bool Exists(string field);
    }
}
