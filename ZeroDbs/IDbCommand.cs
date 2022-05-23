using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbCommand : IDisposable
    {
        string TransactionInfo { get; set; }
        string CommandText { get; set; }
        int CommandTimeout { get; set; }
        bool IsCheckCommandText { get; set; }
        System.Data.Common.DbParameterCollection Parameters { get; }
        System.Data.CommandType CommandType { get; set; }
        System.Data.Common.DbConnection DbConnection { get; }
        Common.SqlBuilder SqlBuilder { get; }
        Common.ListQuery ListQuery();
        Common.ListQuery ListQuery(System.Collections.Specialized.NameValueCollection queryNVC);
        Common.PageQuery PageQuery();
        Common.PageQuery PageQuery(System.Collections.Specialized.NameValueCollection queryNVC);
        System.Data.Common.DbParameter CreateParameter();
        System.Data.Common.DbParameter CreateParameter(string parameterName, object value);
        System.Data.Common.DbParameter CreateParameter(string parameterName, System.Data.DbType dbType, int size, object value);
        int ExecuteNonQuery();
        int ExecuteNonQuery(Common.SqlInfo info);
        int ExecuteNonQuery(string rawSql, params object[] paras);
        List<T> ExecuteQuery<T>(Common.SqlInfo info) where T : class, new();
        List<T> ExecuteQuery<T>(string rawSql, params object[] paras) where T : class, new();
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
