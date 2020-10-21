using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbTransactionScope : IDisposable
    {
        void Execute(Common.DbTransactionCommandDelegate transactionCommandDelegate);
        void Complete(bool isSuccess);
    }
}
