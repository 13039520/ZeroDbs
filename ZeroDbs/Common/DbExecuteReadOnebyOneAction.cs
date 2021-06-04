using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbExecuteReadOnebyOneResult<T> where T : class, new()
    {
        public readonly long RowNum;
        public readonly T RowData;
        public bool Next = true;
        public DbExecuteReadOnebyOneResult(long RowNum, T RowData)
        {
            this.RowNum = RowNum;
            this.RowData = RowData;
        }

    }
    public delegate void DbExecuteReadOnebyOneAction<T>(DbExecuteReadOnebyOneResult<T> result) where T : class, new();

}
