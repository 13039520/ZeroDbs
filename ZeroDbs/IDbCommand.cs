using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbCommand : IDisposable
    {
        string CommandText { get; set; }
        int CommandTimeout { get; set; }
        bool IsCheckCommandText { get; set; }
        System.Data.Common.DbParameterCollection Parameters { get; }
        System.Data.CommandType CommandType { get; set; }
        System.Data.Common.DbConnection DbConnection { get; }
        Common.SqlBuilder DbSqlBuilder { get; }
        System.Data.Common.DbParameter CreateParameter();
        System.Data.Common.DbParameter CreateParameter(string parameterName, object value);
        System.Data.Common.DbParameter CreateParameter(string parameterName, System.Data.DbType dbType, int size, object value);
        int ExecuteNonQuery();
        List<T> ExecuteReader<T>(bool useEmit = true) where T : class, new();
        /// <summary>
        /// Used for reading large quantities of data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="useEmit"></param>
        void ExecuteReader<T>(Common.DbExecuteReadOnebyOneAction<T> action, bool useEmit = true) where T : class, new();
        System.Data.IDataReader ExecuteReader();
        object ExecuteScalar();
        void ParametersFromEntity(object entity);
        void ParametersFromParas(params object[] paras);
        void ParametersFromDictionary(Dictionary<string, object> dic);
        ISqlInsertBuilder Insert(string tableName);
        ISqlDeleteBuilder Delete(string tableName);
        ISqlUpdateBuilder Update(string tableName);
        ISqlSelectBuilder Select(string tableName);

    }
}
