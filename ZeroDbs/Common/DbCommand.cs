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
        private string transactionInfo = "";
        private System.Data.CommandType commandType = System.Data.CommandType.Text;
        private string dbKey = "";
        private bool isCheckCommandText = true;
        private Common.SqlBuilder sqlBuilder = null;
        private bool hasDbExecuteHandler = false;

        public string TransactionInfo { get { return transactionInfo; } set { transactionInfo = value; } }
        public string CommandText { get { return commandText; } set { commandText = value; } }
        public int CommandTimeout { get { return commandTimeout; } set { commandTimeout = value; } }
        public bool IsCheckCommandText { get { return isCheckCommandText; } set { isCheckCommandText = value; } }
        public System.Data.Common.DbParameterCollection Parameters { get { return dbCommand.Parameters; } }
        public System.Data.CommandType CommandType { get { return commandType; } set { commandType = value; } }
        public System.Data.Common.DbConnection DbConnection { get { return dbCommand.Connection; } }
        public Common.SqlBuilder SqlBuilder { get { return sqlBuilder; } }
        public string DbKey { get { return dbKey; } }

        private event DbExecuteHandler OnDbExecute;
        protected void FireExecuteSql(DbExecuteSqlType type, string msg = "OK")
        {
            try
            {
                OnDbExecute(this, new DbExecuteArgs(dbKey,commandText,transactionInfo,type,msg));
            }
            catch { }
        }
        public DbCommand(string dbKey, System.Data.Common.DbCommand dbCommand, DbExecuteHandler dbExecuteHandler, SqlBuilder sqlBuilder)
        {
            this.OnDbExecute = dbExecuteHandler;
            this.dbKey = dbKey;
            this.dbCommand = dbCommand;
            this.sqlBuilder = sqlBuilder;
            this.hasDbExecuteHandler = dbExecuteHandler != null;
        }
        public System.Data.Common.DbParameter CreateParameter()
        {
            return dbCommand.CreateParameter();
        }
        public System.Data.Common.DbParameter CreateParameter(string parameterName, object value)
        {
            var parameter = CreateParameter();
            parameter.ParameterName = parameterName[0] == '@' ? parameterName : String.Format("@{0}", parameterName);
            parameter.Value = value is null ? DBNull.Value : value;
            return parameter;
        }
        public System.Data.Common.DbParameter CreateParameter(string parameterName, System.Data.DbType dbType, int size, object value)
        {
            var parameter = CreateParameter();
            parameter.ParameterName = parameterName[0] == '@' ? parameterName : String.Format("@{0}", parameterName);
            parameter.Value = value is null ? DBNull.Value : value;
            parameter.DbType = dbType;
            parameter.Size = size;
            return parameter;
        }
        public int ExecuteNonQuery()
        {
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
                if (hasDbExecuteHandler)
                {
                    FireExecuteSql(DbExecuteSqlType.NONQUERY);
                }
                
                return dbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (hasDbExecuteHandler)
                {
                    FireExecuteSql(DbExecuteSqlType.NONQUERY,ex.Message);
                }
                throw ex;
            }
        }
        public int ExecuteNonQuery(string rawSql, params object[] paras)
        {
            var info = this.SqlBuilder.RawSql(rawSql, paras);
            return this.ExecuteNonQuery(info);
        }
        public int ExecuteNonQuery(SqlInfo info)
        {
            this.commandText = info.Sql;
            this.ParametersFromDictionary(info.Paras);
            return this.ExecuteNonQuery();
        }
        public List<T> ExecuteQuery<T>(string rawSql, params object[] paras) where T : class, new()
        {
            var info = this.SqlBuilder.RawSql(rawSql, paras);
            return this.ExecuteQuery<T>(info);
        }
        public List<T> ExecuteQuery<T>(SqlInfo info) where T : class, new()
        {
            this.commandText = info.Sql;
            this.ParametersFromDictionary(info.Paras);
            return this.ExecuteReader<T>();
        }
        public List<T> ExecuteReader<T>(bool useEmit = true) where T : class, new()
        {
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
                if (hasDbExecuteHandler)
                {
                    FireExecuteSql(DbExecuteSqlType.QUERY);
                }
                return useEmit ? Entities.ListFromDataReaderByEmit<T>(dr) : Entities.ListFromDataReader<T>(dr);
            }
            catch (Exception ex)
            {
                if (hasDbExecuteHandler)
                {
                    FireExecuteSql(DbExecuteSqlType.QUERY, ex.Message);
                }
                throw ex;
            }
        }
        public System.Data.DataTable ExecuteQuery(string rawSql, params object[] paras)
        {
            var info = this.SqlBuilder.RawSql(rawSql, paras);
            return this.ExecuteQuery(info);
        }
        public System.Data.DataTable ExecuteQuery(SqlInfo info)
        {
            this.commandText = info.Sql;
            this.ParametersFromDictionary(info.Paras);
            var reader = this.ExecuteReader();
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Load(reader);
            reader.Close();
            return dt;
        }
        public void ExecuteReader<T>(DataReadHandler<T> action, bool useEmit = true) where T : class, new()
        {
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
                if (hasDbExecuteHandler)
                {
                    FireExecuteSql(DbExecuteSqlType.QUERY);
                }
                if (useEmit)
                {
                    Entities.ListFromDataReaderByEmit<T>(dr, action);
                }
                else
                {
                    Entities.ListFromDataReader<T>(dr, action);
                }
            }
            catch (Exception ex)
            {
                if (hasDbExecuteHandler)
                {
                    FireExecuteSql(DbExecuteSqlType.QUERY, ex.Message);
                }
                throw ex;
            }
        }
        public System.Data.IDataReader ExecuteReader()
        {
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
                if (hasDbExecuteHandler)
                {
                    FireExecuteSql(DbExecuteSqlType.QUERY);
                }
                return dr;
            }
            catch (Exception ex)
            {
                if (hasDbExecuteHandler)
                {
                    FireExecuteSql(DbExecuteSqlType.QUERY, ex.Message);
                }
                throw ex;
            }
        }
        public object ExecuteScalar()
        {
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
                if (hasDbExecuteHandler)
                {
                    FireExecuteSql(DbExecuteSqlType.QUERY);
                }
                return dbCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                if (hasDbExecuteHandler)
                {
                    FireExecuteSql(DbExecuteSqlType.QUERY, ex.Message);
                }
                throw ex;
            }
        }
        
        public void ParametersFromEntity(object entity)
        {
            Parameters.Clear();
            if (entity == null) { return; }
            var ps = PropertyInfoCache.GetPropertyInfoList(entity.GetType());
            foreach (System.Reflection.PropertyInfo p in ps)
            {
                object value = p.GetValue(entity, null);
                if (value == null)
                {
                    value = DBNull.Value;
                }
                Parameters.Add(CreateParameter(p.Name, value));
            }
        }
        public void ParametersFromParas(params object[] paras)
        {
            Parameters.Clear();
            if (paras.Length < 1) { return; }
            for(int i=0; i < paras.Length; i++)
            {
                object value = paras[i];
                if (value == null)
                {
                    value = DBNull.Value;
                }
                Parameters.Add(CreateParameter(i.ToString(), value));
            }
        }
        public void ParametersFromDictionary(Dictionary<string,object> dic)
        {
            Parameters.Clear();
            foreach (string key in dic.Keys)
            {
                object value = dic[key];
                if (value == null)
                {
                    value = DBNull.Value;
                }
                Parameters.Add(CreateParameter(key, value));
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
