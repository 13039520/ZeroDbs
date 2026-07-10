using System;

namespace ZeroDbs
{
    /// <summary>
    /// 雪花算法ID生成器
    /// </summary>
    public interface ISnowflakeIdGenerator
    {
        /// <summary>
        /// 获取ID
        /// </summary>
        /// <returns></returns>
        long NextId();
    }
}
