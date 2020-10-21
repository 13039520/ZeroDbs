using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public enum DbExecuteSqlType
    {
        NONQUERY,
        QUERY,
        TRANSACTION,
        ERROR
    }
}
