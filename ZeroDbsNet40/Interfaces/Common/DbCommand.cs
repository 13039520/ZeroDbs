using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces.Common
{
    public class DbCommand: IDbCommand
    {
        private readonly System.Data.Common.DbCommand dbCommand = null;
        private int commandTimeout = 15;
        private string commandText = "";
        private System.Data.CommandType commandType = System.Data.CommandType.Text;
        private string dbKey = "";
        private bool isCheckCommandText = true;
        private ZeroDbs.Interfaces.IDbSqlBuilder dbSqlBuilder = null;

        public string CommandText { get { return commandText; } set { commandText = value; } }
        public int CommandTimeout { get { return commandTimeout; } set { commandTimeout = value; } }
        public bool IsCheckCommandText { get { return isCheckCommandText; } set { isCheckCommandText = value; } }
        public System.Data.Common.DbParameterCollection Parameters { get { return dbCommand.Parameters; } }
        public System.Data.CommandType CommandType { get { return commandType; } set { commandType = value; } }
        public System.Data.Common.DbConnection DbConnection { get { return dbCommand.Connection; } }
        public ZeroDbs.Interfaces.IDbSqlBuilder DbSqlBuilder { get { return dbSqlBuilder; } }
        public string DbKey { get { return dbKey; } }

        private event Common.DbExecuteSqlEvent OnExecuteSqlEvent;
        protected void FireExecuteSql(Common.DbExecuteSqlEventArgs e)
        {
            if (OnExecuteSqlEvent != null)
            {
                OnExecuteSqlEvent(this, e);
            }
        }
        public DbCommand(string dbKey, System.Data.Common.DbCommand dbCommand, Common.DbExecuteSqlEvent onExecuteSqlEvent, ZeroDbs.Interfaces.IDbSqlBuilder dbSqlBuilder)
        {
            this.OnExecuteSqlEvent = onExecuteSqlEvent;
            this.dbKey = dbKey;
            this.dbCommand = dbCommand;
            this.dbSqlBuilder = dbSqlBuilder;
        }
        public System.Data.Common.DbParameter CreateParameter()
        {
            return dbCommand.CreateParameter();
        }
        public System.Data.Common.DbParameter CreateParameter(string parameterName, object value)
        {
            var parameter = CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            return parameter;
        }
        public System.Data.Common.DbParameter CreateParameter(string parameterName, System.Data.DbType dbType, int size, object value)
        {
            var parameter = CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            parameter.DbType = dbType;
            parameter.Size = size;
            return parameter;
        }
        public int ExecuteNonQuery()
        {
            List<string> sqlList = new List<string>();
            sqlList.Add(CommandText);
            try
            {
                if (IsCheckCommandText && CommandType == System.Data.CommandType.Text)
                {
                    string msg = "";
                    if(!Common.SqlCheck.IsInsertSql(CommandText, ref msg)
                        && Common.SqlCheck.IsDeleteSql(CommandText, ref msg)
                        && Common.SqlCheck.IsUpdateSql(CommandText, ref msg))
                    {
                        throw new Exception("不符合INSERT|DELETE|UPDATE命令模式");
                    }
                }
                dbCommand.CommandText = CommandText;
                dbCommand.CommandTimeout = CommandTimeout;
                dbCommand.CommandType = CommandType;
                FireExecuteSql(new DbExecuteSqlEventArgs(
                    DbKey,
                    sqlList,
                    DbExecuteSqlType.NONQUERY,
                    "OK"));
                return dbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                FireExecuteSql(new DbExecuteSqlEventArgs(
                    DbKey,
                    sqlList,
                    DbExecuteSqlType.NONQUERY,
                    ex.Message));
                throw ex;
            }
        }
        public List<T> ExecuteReader<T>(bool useEmit = true) where T : class, new()
        {
            List<string> sqlList = new List<string>();
            sqlList.Add(CommandText);
            try
            {
                if (IsCheckCommandText && CommandType == System.Data.CommandType.Text)
                {
                    string msg = "";
                    if(!Common.SqlCheck.IsSelectSql(CommandText, ref msg))
                    {
                        throw new Exception(msg);
                    }
                }
                dbCommand.CommandText = CommandText;
                dbCommand.CommandTimeout = CommandTimeout;
                dbCommand.CommandType = CommandType;
                System.Data.Common.DbDataReader dr = dbCommand.ExecuteReader();
                FireExecuteSql(new DbExecuteSqlEventArgs(
                    DbKey,
                    sqlList,
                    DbExecuteSqlType.QUERY,
                    "OK"));
                if (useEmit)
                {
                    return DbDataReaderToEntity<T>.EntityListByEmit(dr);
                }
                return DbDataReaderToEntity<T>.EntityList(dr);
            }
            catch (Exception ex)
            {
                FireExecuteSql(new DbExecuteSqlEventArgs(
                    DbKey,
                    sqlList,
                    DbExecuteSqlType.QUERY,
                    ex.Message));
                throw ex;
            }
        }
        public System.Data.IDataReader ExecuteReader()
        {
            List<string> sqlList = new List<string>();
            sqlList.Add(CommandText);
            try
            {
                if (IsCheckCommandText && CommandType == System.Data.CommandType.Text)
                {
                    string msg = "";
                    if (!Common.SqlCheck.IsSelectSql(CommandText, ref msg))
                    {
                        throw new Exception(msg);
                    }
                }
                dbCommand.CommandText = CommandText;
                dbCommand.CommandTimeout = CommandTimeout;
                dbCommand.CommandType = CommandType;
                System.Data.Common.DbDataReader dr = dbCommand.ExecuteReader();
                FireExecuteSql(new DbExecuteSqlEventArgs(
                    DbKey,
                    sqlList,
                    DbExecuteSqlType.QUERY,
                    "OK"));
                return dr;
            }
            catch (Exception ex)
            {
                FireExecuteSql(new DbExecuteSqlEventArgs(
                    DbKey,
                    sqlList,
                    DbExecuteSqlType.QUERY,
                    ex.Message));
                throw ex;
            }
        }
        public object ExecuteScalar()
        {
            List<string> sqlList = new List<string>();
            sqlList.Add(CommandText);
            try
            {
                if (IsCheckCommandText && CommandType == System.Data.CommandType.Text)
                {
                    string msg = "";
                    if (!Common.SqlCheck.IsSelectSql(CommandText, ref msg))
                    {
                        throw new Exception(msg);
                    }
                }
                dbCommand.CommandText = CommandText;
                dbCommand.CommandTimeout = CommandTimeout;
                dbCommand.CommandType = CommandType;
                FireExecuteSql(new DbExecuteSqlEventArgs(
                    DbKey,
                    sqlList,
                    DbExecuteSqlType.QUERY,
                    "OK"));
                return dbCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                FireExecuteSql(new DbExecuteSqlEventArgs(
                    dbCommand.Connection.ConnectionString,
                    sqlList,
                    DbExecuteSqlType.QUERY,
                    ex.Message));
                throw ex;
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
            if (disposing)
            {
                if (dbCommand.Connection != null)
                {
                    if (dbCommand.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        dbCommand.Connection.Close();
                    }
                }
                dbCommand.Dispose();
            }
            _disposed = true;
        }
    }
}
