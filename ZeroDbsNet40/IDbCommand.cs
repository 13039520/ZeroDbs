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
        IDbSqlBuilder DbSqlBuilder { get; }
        System.Data.Common.DbParameter CreateParameter();
        System.Data.Common.DbParameter CreateParameter(string parameterName, object value);
        System.Data.Common.DbParameter CreateParameter(string parameterName, System.Data.DbType dbType, int size, object value);
        int ExecuteNonQuery();
        List<T> ExecuteReader<T>(bool useEmit = true) where T : class, new();
        System.Data.IDataReader ExecuteReader();
        object ExecuteScalar();




    }
}
