﻿using System;
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
            return string.Format("[{0}].[dbo].[{1}]", tableInfo.DbName, tableInfo.Name);
        }
    }
}
