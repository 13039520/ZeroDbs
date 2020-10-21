using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbSearcher
    {
        Dictionary<string, IDb> GetDbs();
        IDb GetDb(string dbKey);
        IDb GetDb<T>() where T : class, new();
        IDb GetDbByEntityFullName(string entityFullName);
    }
}
