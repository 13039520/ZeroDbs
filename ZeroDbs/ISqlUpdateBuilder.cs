using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface ISqlUpdateBuilder
    {
        ISqlUpdateBuilder SetFields(params string[] fields);
        ISqlUpdateBuilder SetValues(params object[] values);
        ISqlUpdateBuilder WhereFields(params string[] fields);
        ISqlUpdateBuilder WhereValues(params object[] values);
        ISqlUpdateBuilder Where(string where);
        ISqlUpdateBuilder DateTimeFormat(string format);
    }
}
