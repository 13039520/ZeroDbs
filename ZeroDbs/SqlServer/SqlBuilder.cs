using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroDbs.Common;

namespace ZeroDbs.SqlServer
{
    internal class SqlBuilder:Common.SqlBuilder
    {
        public SqlBuilder(IDb db) : base(db)
        {

        }
        public override string GetTableName(DbDataTableInfo tableInfo)
        {
            return string.Format("[{0}]", tableInfo.Name);
        }
        public override string GetColunmName(string colName)
        {
            return string.Format("[{0}]", colName);
        }
    }
}
