using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs.Common
{
    internal class DbParameterCreator : IDbParameterCreator
    {
        public string DbType { get{ return "Default"; } }
        private readonly System.Data.Common.DbCommand dbCommand;
        public DbParameterCreator(System.Data.Common.DbCommand dbCommand)
        {
            this.dbCommand = dbCommand;
        }
        public System.Data.Common.DbParameter Create()
        {
            return dbCommand.CreateParameter();
        }
        public System.Data.Common.DbParameter Create(string pName, object pValue)
        {
            var parameter = Create();
            parameter.ParameterName = pName;
            parameter.Value = pValue is null ? DBNull.Value : pValue;
            return parameter;
        }
        public System.Data.Common.DbParameter Create(string pName, System.Data.DbType dbType, int size, object pValue)
        {
            var parameter = Create();
            parameter.ParameterName = pName;
            parameter.Value = pValue is null ? DBNull.Value : pValue;
            parameter.DbType = dbType;
            parameter.Size = size;
            return parameter;
        }
    }
}
