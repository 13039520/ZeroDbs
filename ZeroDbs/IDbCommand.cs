using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Text;

namespace ZeroDbs
{
    public interface IDbCommand : IDisposable
    {
        string DbKey { get; }
        string DbType { get; }
        string TransactionInfo { get; set; }
        string CommandText { get; set; }
        int CommandTimeout { get; set; }
        System.Data.Common.DbParameterCollection Parameters { get; }
        System.Data.CommandType CommandType { get; set; }
        System.Data.Common.DbConnection DbConnection { get; }
        Common.SqlBuilder SqlBuilder { get; }
        System.Data.Common.DbParameter CreateParameter();
        System.Data.Common.DbParameter CreateParameter(string pName, object pValue);
        System.Data.Common.DbParameter CreateParameter(string pName, System.Data.DbType dbType, int size, object pValue);
        int ExecuteNonQuery();
        int ExecuteNonQuery(Common.SqlInfo info);
        int ExecuteNonQuery(string rawSql, params object[] paras);
        List<T> ExecuteQuery<T>(Common.SqlInfo info) where T : class, new();
        List<T> ExecuteQuery<T>(string rawSql, params object[] paras) where T : class, new();
        System.Data.DataTable ExecuteQuery(string rawSql, params object[] paras);
        System.Data.DataTable ExecuteQuery(Common.SqlInfo info);
        List<T> ExecuteReader<T>(bool useEmit = true) where T : class, new();
        /// <summary>
        /// Used for reading large quantities of data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="useEmit"></param>
        void ExecuteReader<T>(Common.DataReadHandler<T> action, bool useEmit = true) where T : class, new();
        System.Data.IDataReader ExecuteReader();
        object ExecuteScalar();
        
        void ParametersFromEntity(object entity);
        void ParametersFromParas(params object[] paras);
        void ParametersFromDictionary(Dictionary<string, object> dic);

    }
}
