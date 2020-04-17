using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces
{
    public interface IDbSearcher
    {
        Dictionary<string, ZeroDbs.Interfaces.IDb> GetDbs();
        ZeroDbs.Interfaces.IDb GetDb(string dbKey);
        ZeroDbs.Interfaces.IDb GetDb<T>() where T : class, new();
        ZeroDbs.Interfaces.IDb GetDbByEntityFullName(string entityFullName);
    }
}
