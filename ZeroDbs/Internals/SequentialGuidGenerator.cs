using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    internal static class SequentialGuidGenerator
    {
        static object _lock=new object();
        static long startN = DateTime.Now.Ticks;
        const long utc1970Ticks = 621355968000000000L;
        public static Guid Next()
        {
            var part1 = BitConverter.GetBytes(DateTime.Now.Ticks - utc1970Ticks);
            byte[] part2;
            lock (_lock)
            {
                startN++;
                part2 = BitConverter.GetBytes(startN);
            }
            if (BitConverter.IsLittleEndian)
            {
                //反转为大端模式(使字符串化的时候具有顺序性)
                Array.Reverse(part1);
                Array.Reverse(part2);
            }
            var bytes = new byte[] {
                part1[3],part1[2],part1[1],part1[0],//p1
                part1[5],part1[4],//p2
                part1[7],part1[6],//p3
                part2[0],part2[1],//p4
                part2[2],part2[3],part2[4],part2[5],part2[6],part2[7],//p5
            };
            return new Guid(bytes);
        }
    }
}
