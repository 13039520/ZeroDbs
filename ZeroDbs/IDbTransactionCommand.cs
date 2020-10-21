using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbTransactionCommand : IDisposable
    {
        string CommandText { get; set; }
        int CommandTimeout { get; set; }
        System.Data.Common.DbParameterCollection Parameters { get; }
        System.Data.CommandType CommandType { get; set; }
        System.Data.Common.DbCommand DbCommand { get; }
        string Identification { get; }
        string GroupId { get; }

        int ExecuteNonQuery();
        List<T> ExecuteReader<T>() where T : class, new();
        object ExecuteScalar();
    }
}
