using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbCommand: IDbCommand
    {
        private readonly System.Data.Common.DbCommand dbCommand = null;
        private int commandTimeout = 15;
        private string commandText = "";
        private System.Data.CommandType commandType = System.Data.CommandType.Text;
        private string dbKey = "";
        private bool isCheckCommandText = true;
        private Common.SqlBuilder dbSqlBuilder = null;

        public string CommandText { get { return commandText; } set { commandText = value; } }
        public int CommandTimeout { get { return commandTimeout; } set { commandTimeout = value; } }
        public bool IsCheckCommandText { get { return isCheckCommandText; } set { isCheckCommandText = value; } }
        public System.Data.Common.DbParameterCollection Parameters { get { return dbCommand.Parameters; } }
        public System.Data.CommandType CommandType { get { return commandType; } set { commandType = value; } }
        public System.Data.Common.DbConnection DbConnection { get { return dbCommand.Connection; } }
        public Common.SqlBuilder DbSqlBuilder { get { return dbSqlBuilder; } }
        public string DbKey { get { return dbKey; } }

        private event DbExecuteSqlEvent OnExecuteSqlEvent;
        protected void FireExecuteSql(Common.DbExecuteSqlEventArgs e)
        {
            if (OnExecuteSqlEvent != null)
            {
                OnExecuteSqlEvent(this, e);
            }
        }
        public DbCommand(string dbKey, System.Data.Common.DbCommand dbCommand, Common.DbExecuteSqlEvent onExecuteSqlEvent, Common.SqlBuilder dbSqlBuilder)
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
            parameter.Value = value is null ? DBNull.Value : value;
            return parameter;
        }
        public System.Data.Common.DbParameter CreateParameter(string parameterName, System.Data.DbType dbType, int size, object value)
        {
            var parameter = CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value is null ? DBNull.Value : value;
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
                return useEmit ? DbDataReaderToEntity<T>.EntityListByEmit(dr) : DbDataReaderToEntity<T>.EntityList(dr);
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
        public void ExecuteReader<T>(Common.DbExecuteReadOnebyOneAction<T> action, bool useEmit = true) where T : class, new()
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
                if (useEmit)
                {
                    DbDataReaderToEntity<T>.EntityListByEmit(dr, action);
                }
                else
                {
                    DbDataReaderToEntity<T>.EntityList(dr, action);
                }
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
        public void ParametersFromEntity(object entity)
        {
            Parameters.Clear();
            if (entity == null) { return; }
            System.Reflection.PropertyInfo[] ps = entity.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo p in ps)
            {
                object value = p.GetValue(entity, null);
                if (value == null)
                {
                    value = DBNull.Value;
                }
                Parameters.Add(CreateParameter("@" + p.Name, value));
            }
        }
        public void ParametersFromParas(params object[] paras)
        {
            Parameters.Clear();
            if (paras.Length < 1) { return; }
            for(int i=0; i < paras.Length; i++)
            {
                Parameters.Add(CreateParameter("@" + i, paras[i]));
            }
        }
        public void ParametersFromDictionary(Dictionary<string,object> dic)
        {
            Parameters.Clear();
            foreach (string key in dic.Keys)
            {
                Parameters.Add(CreateParameter("@" + key, dic[key]));
            }
        }
        public ISqlInsertBuilder Insert(string tableName)
        {
            return new SqlInsertBuilder(tableName);
        }
        public ISqlDeleteBuilder Delete(string tableName)
        {
            return new SqlDeleteBuilder(tableName);
        }
        public ISqlUpdateBuilder Update(string tableName)
        {
            return new SqlUpdateBuilder(tableName);
        }
        public ISqlSelectBuilder Select(string tableName)
        {
            return new SqlSelectBuilder(tableName);
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
