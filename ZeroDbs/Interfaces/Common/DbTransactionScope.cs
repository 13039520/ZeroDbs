using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces.Common
{
    public class DbTransactionScope: IDbTransactionScope
    {
        System.Data.Common.DbTransaction dbTransaction = null;
        bool completedFlag = false;
        volatile int ExecuteCount = 0;
        System.Threading.Timer timer = null;
        string _Identification = DateTime.Now.ToString("HHmmssfff");
        string _GroupId = DateTime.Now.ToString("yyyyMMdd");
        string executeExceptionMsg = string.Empty;
        public string Identification { get { return _Identification; } }
        public string GroupId { get { return _GroupId; } }
        IDb db = null;
        IDbCommand dbCommand = null;

        public DbTransactionScope(IDb db, string identification="", string groupId="")
        {
            this.db = db;
            this.dbCommand = db.GetDbCommand();

            var conn = db.GetDbConnection();
            conn.Open();
            this.dbTransaction = conn.BeginTransaction();
            if (!string.IsNullOrEmpty(identification))
            {
                this._Identification = identification;
            }
            if (!string.IsNullOrEmpty(groupId))
            {
                this._GroupId = groupId;
            }
            timer = new System.Threading.Timer(new System.Threading.TimerCallback(timerCallback));
            timer.Change(5000, 1000);
        }
        private void FireEvent(DbExecuteSqlEventArgs args)
        {
            db.FireZeroDbExecuteSqlEvent(args);
        }

        public void Execute(DbTransactionCommandDelegate transactionDelegate)
        {
            ExecuteCount++;
            if (transactionDelegate != null)
            {
                try
                {
                    transactionDelegate(this.dbCommand);
                    //此处不能提交事务：
                    //1.单个DbTransactionScope让调用者触发Complete
                    //2.当被DbTransactionScopeCollections包含时由DbTransactionScopeCollections触发Complete
                }
                catch (Exception ex)
                {
                    executeExceptionMsg = ex.Message;
                    Complete(false);
                    //抛出异常让DbTransactionScopeCollections能够捕获并及时中断可能存在的后续的其它事务
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 确认事务完成
        /// <para>1.单个DbTransactionScope的场景中调用者必须主动触发</para>
        /// <para>2.当被DbTransactionScopeCollections包含时由DbTransactionScopeCollections触发</para>
        /// </summary>
        /// <param name="isSuccess"></param>
        public void Complete(bool isSuccess)
        {
            if (ExecuteCount > 0)
            {
                if (!completedFlag)
                {
                    completedFlag = true;
                    if (isSuccess)
                    {
                        this.dbTransaction.Commit();
                        FireEvent(new DbExecuteSqlEventArgs(db.DbConfigDatabaseInfo.dbKey, new List<string>(), DbExecuteSqlType.TRANSACTION, "事务" + this.Identification + "(" + this.GroupId + ")已经提交"));
                    }
                    else
                    {
                        this.dbTransaction.Rollback();
                        if (string.IsNullOrEmpty(executeExceptionMsg))
                        {
                            FireEvent(new DbExecuteSqlEventArgs(db.DbConfigDatabaseInfo.dbKey, new List<string>(), DbExecuteSqlType.TRANSACTION, "事务" + this.Identification + "(" + this.GroupId + ")已经回滚"));
                        }
                        else
                        {
                            FireEvent(new DbExecuteSqlEventArgs(db.DbConfigDatabaseInfo.dbKey, new List<string>(), DbExecuteSqlType.TRANSACTION, "事务" + this.Identification + "(" + this.GroupId + ")已经回滚(" + executeExceptionMsg + ")"));
                        }
                    }
                }
                else
                {
                    FireEvent(new DbExecuteSqlEventArgs(db.DbConfigDatabaseInfo.dbKey, new List<string>(), DbExecuteSqlType.TRANSACTION, "事务" + this.Identification + "(" + this.GroupId + ")已经执行过Complete()"));
                }
            }
            Dispose();
        }

        private void timerCallback(object obj)
        {
            if (ExecuteCount < 1)
            {
                Dispose();
            }
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
                this.dbCommand.Dispose();
                if (this.dbTransaction.Connection != null)
                {
                    if (this.dbTransaction.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        this.dbTransaction.Connection.Close();
                    }
                }
                this.dbTransaction.Dispose();
                this.timer.Dispose();
            }
        }
    }
}
