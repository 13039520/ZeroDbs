using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbTransactionScopeCollection: IDisposable
    {
        void Add(IDbTransactionScope transactionScope);
        void Execute(Common.DbTransactionCommandDelegate dbTransactionCommandDelegate);
    }
}
