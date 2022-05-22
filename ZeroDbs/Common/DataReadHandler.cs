using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroDbs.Common
{
    public class DataReadArgs<T> where T : class, new()
    {
        public readonly long RowNum;
        public readonly T RowData;
        public bool Next = true;
        public DataReadArgs(long RowNum, T RowData)
        {
            this.RowNum = RowNum;
            this.RowData = RowData;
        }

    }
    public delegate void DataReadHandler<T>(DataReadArgs<T> result) where T : class, new();

}
