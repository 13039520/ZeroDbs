using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface ISqlDeleteBuilder
    {
        ISqlDeleteBuilder WhereFields(params string[] fields);
        ISqlDeleteBuilder WhereValues(params object[] values);
        ISqlDeleteBuilder Where(string where);
        ISqlDeleteBuilder DateTimeFormat(string format);
    }
}
