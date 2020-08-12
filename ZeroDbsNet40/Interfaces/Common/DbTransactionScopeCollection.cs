using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces.Common
{
    public class DbTransactionScopeCollection: IDbTransactionScopeCollection
    {
        List<IDbTransactionScope> transactionScopeList = null;
        public DbTransactionScopeCollection()
        {
            transactionScopeList = new List<IDbTransactionScope>();
        }
        public void Add(IDbTransactionScope transactionScope)
        {
            transactionScopeList.Add(transactionScope);
        }
        public void Execute(DbTransactionCommandDelegate dbTransactionCommandDelegate)
        {
            if (dbTransactionCommandDelegate != null)
            {
                bool isOk = true;
                foreach (IDbTransactionScope scope in transactionScopeList)
                {
                    try
                    {
                        scope.Execute(dbTransactionCommandDelegate);
                    }
                    catch
                    {
                        isOk = false;
                        break;
                    }
                }
                Complete(isOk);
            }
        }
        /// <summary>
        /// 不公开
        /// </summary>
        /// <param name="isSuccess"></param>
        protected void Complete(bool isSuccess)
        {
            foreach (IDbTransactionScope scope in transactionScopeList)
            {
                scope.Complete(isSuccess);
            }
            Dispose();
        }

        bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposing)
            {
                foreach (IDbTransactionScope scope in transactionScopeList)
                {
                    scope.Dispose();
                }
            }
        }
    }
}
