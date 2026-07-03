using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal class SnowflakeIdGenerator: ISnowflakeIdGenerator
    {
        // 起始时间戳（2024-01-01）
        private const long Epoch = 1704067200000L;

        // 位数定义
        private const int WorkerIdBits = 5;
        private const int DatacenterIdBits = 5;
        private const int SequenceBits = 12;

        // 最大值
        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
        private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);

        // 左移位数
        private const int WorkerIdShift = SequenceBits;
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
        private const int TimestampLeftShift =
            SequenceBits + WorkerIdBits + DatacenterIdBits;

        // 序列掩码
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);

        private long _workerId;
        private long _datacenterId;
        private long _sequence = 0L;

        private long _lastTimestamp = -1L;

        private readonly object _lock = new object();

        public SnowflakeIdGenerator(long workerId, long datacenterId)
        {
            if (workerId > MaxWorkerId || workerId < 0)
                throw new ArgumentException($"workerId 必须在 0~{MaxWorkerId}");

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
                throw new ArgumentException($"datacenterId 必须在 0~{MaxDatacenterId}");

            _workerId = workerId;
            _datacenterId = datacenterId;
        }

        public long NextId()
        {
            lock (_lock)
            {
                long timestamp = CurrentTimeMillis();

                // 时钟回拨
                if (timestamp < _lastTimestamp)
                {
                    throw new Exception("时钟回拨，拒绝生成ID");
                }

                if (timestamp == _lastTimestamp)
                {
                    // 同一毫秒内自增
                    _sequence = (_sequence + 1) & SequenceMask;

                    // 序列溢出
                    if (_sequence == 0)
                    {
                        timestamp = WaitNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = 0L;
                }

                _lastTimestamp = timestamp;

                return ((timestamp - Epoch) << TimestampLeftShift)
                       | (_datacenterId << DatacenterIdShift)
                       | (_workerId << WorkerIdShift)
                       | _sequence;
            }
        }

        private long WaitNextMillis(long lastTimestamp)
        {
            long timestamp = CurrentTimeMillis();

            while (timestamp <= lastTimestamp)
            {
                timestamp = CurrentTimeMillis();
            }

            return timestamp;
        }

        private long CurrentTimeMillis()
        {
#if !NET45
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
#else
            DateTime utc = DateTimeOffset.UtcNow.UtcDateTime;
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(utc - epoch).TotalMilliseconds;
#endif
        }
    }
}
