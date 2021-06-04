using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface ISqlSelectBuilder
    {
        ISqlSelectBuilder Fields(params string[] fields);
        ISqlSelectBuilder Where(string where);
        ISqlSelectBuilder Orderby(string orderby);
        ISqlSelectBuilder Groupby(params string[] fields);
        ISqlSelectBuilder Top(int top);
        ISqlSelectBuilder Top(int top, Common.DatabaseType useDatabaseType);
    }
}
