//为 .NET6 以下的版本定义 DateOnly 类型
#if !NET6_0_OR_GREATER
using System;
using System.Globalization;

namespace System
{

    public struct DateOnly : IEquatable<DateOnly>, IComparable<DateOnly>
    {
        private readonly DateTime _value;

        public int Year => _value.Year;
        public int Month => _value.Month;
        public int Day => _value.Day;

        public DateOnly(int year, int month, int day)
        {
            _value = new DateTime(year, month, day);
        }

        private DateOnly(DateTime value)
        {
            _value = value.Date;
        }

        public DateTime ToDateTime()
        {
            return _value;
        }

        public override string ToString()
        {
            return _value.ToString("yyyy-MM-dd");
        }

        public string ToString(string format)
        {
            return _value.ToString(format);
        }

        public static DateOnly Parse(string s)
        {
            return new DateOnly(DateTime.Parse(s));
        }

        public static DateOnly ParseExact(string s, string format)
        {
            return new DateOnly(
                DateTime.ParseExact(
                    s,
                    format,
                    CultureInfo.InvariantCulture));
        }

        public static DateOnly FromDateTime(DateTime dt)
        {
            return new DateOnly(dt);
        }

        public bool Equals(DateOnly other)
        {
            return _value.Equals(other._value);
        }

        public override bool Equals(object obj)
        {
            return obj is DateOnly other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public int CompareTo(DateOnly other)
        {
            return _value.CompareTo(other._value);
        }

        public static bool operator ==(DateOnly left, DateOnly right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DateOnly left, DateOnly right)
        {
            return !left.Equals(right);
        }

        public static bool operator >(DateOnly left, DateOnly right)
        {
            return left._value > right._value;
        }

        public static bool operator <(DateOnly left, DateOnly right)
        {
            return left._value < right._value;
        }

        public static implicit operator DateTime(DateOnly d)
        {
            return d._value;
        }

        public static explicit operator DateOnly(DateTime d)
        {
            return new DateOnly(d);
        }
    }
    public readonly struct TimeOnly : IComparable<TimeOnly>, IEquatable<TimeOnly>
    {
        private readonly long _ticks;

        public const long TicksPerDay = TimeSpan.TicksPerDay;

        public TimeOnly(int hour, int minute)
        {
            _ticks = new TimeSpan(hour, minute, 0).Ticks;
        }

        public TimeOnly(int hour, int minute, int second)
        {
            _ticks = new TimeSpan(hour, minute, second).Ticks;
        }

        public TimeOnly(int hour, int minute, int second, int millisecond)
        {
            _ticks = new TimeSpan(0, hour, minute, second, millisecond).Ticks;
        }

        private TimeOnly(long ticks)
        {
            _ticks = ticks % TicksPerDay;
        }

        public int Hour => (int)TimeSpan.FromTicks(_ticks).Hours;
        public int Minute => (int)TimeSpan.FromTicks(_ticks).Minutes;
        public int Second => (int)TimeSpan.FromTicks(_ticks).Seconds;

        public TimeSpan ToTimeSpan() => TimeSpan.FromTicks(_ticks);

        public static TimeOnly FromTimeSpan(TimeSpan time)
            => new TimeOnly(time.Ticks % TicksPerDay);

        public static TimeOnly Parse(string s)
            => FromTimeSpan(TimeSpan.Parse(s));

        public override string ToString()
            => TimeSpan.FromTicks(_ticks).ToString(@"hh\:mm\:ss");

        // 比较
        public int CompareTo(TimeOnly other)
            => _ticks.CompareTo(other._ticks);

        public bool Equals(TimeOnly other)
            => _ticks == other._ticks;

        public override bool Equals(object obj)
            => obj is TimeOnly other && Equals(other);

        public override int GetHashCode()
            => _ticks.GetHashCode();

        // 运算符
        public static bool operator <(TimeOnly left, TimeOnly right)
            => left._ticks < right._ticks;

        public static bool operator >(TimeOnly left, TimeOnly right)
            => left._ticks > right._ticks;

        public static bool operator ==(TimeOnly left, TimeOnly right)
            => left._ticks == right._ticks;

        public static bool operator !=(TimeOnly left, TimeOnly right)
            => left._ticks != right._ticks;
    }
}
#endif