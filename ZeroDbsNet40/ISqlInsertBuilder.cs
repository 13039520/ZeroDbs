using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface ISqlInsertBuilder
    {
        ISqlInsertBuilder Fields(params string[] fields);
        ISqlInsertBuilder Values(params object[] values);
        ISqlInsertBuilder Where(string where);
        ISqlInsertBuilder DateTimeFormat(string format);
    }
}
