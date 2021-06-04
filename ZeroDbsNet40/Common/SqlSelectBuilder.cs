using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroDbs.Common
{
    public class SqlSelectBuilder: ISqlSelectBuilder
    {
        string tableName = "";
        string where = "";
        string orderby = "";
        string[] fields = null;
        string[] groupby = null;
        int top = 0;
        DatabaseType topType = DatabaseType.SqlServer;
        public SqlSelectBuilder(string tableName)
        {
            this.tableName = tableName;
        }
        public ISqlSelectBuilder Fields(params string[] fields)
        {
            this.fields = fields;
            return this;
        }
        public ISqlSelectBuilder Where(string where)
        {
            this.where = where;
            return this;
        }
        public ISqlSelectBuilder Orderby(string orderby)
        {
            this.orderby = orderby;
            return this;
        }
        public ISqlSelectBuilder Groupby(params string[] fields)
        {
            this.groupby = fields;
            return this;
        }
        public ISqlSelectBuilder Top(int top)
        {
            return Top(top, DatabaseType.SqlServer);
        }
        public ISqlSelectBuilder Top(int top, DatabaseType useDatabaseType)
        {
            this.top = top;
            this.topType = useDatabaseType;
            return this;
        }
        public override string ToString()
        {
            StringBuilder s = new StringBuilder("SELECT");
            bool hasTopNum = top > 0;
            bool isSqlServer = topType == DatabaseType.SqlServer;
            if (hasTopNum && isSqlServer)
            {
                s.AppendFormat(" TOP {0}", top);
            }
            if (fields != null && fields.Length > 0)
            {
                s.AppendFormat(" {0}", string.Join(",", fields));
            }
            else
            {
                s.Append(" *");
            }
            s.AppendFormat(" FROM {0}", tableName);
            if (!string.IsNullOrEmpty(where))
            {
                s.AppendFormat(" WHERE {0}", where);
            }
            if (groupby != null && groupby.Length > 0)
            {
                s.AppendFormat(" GROUP BY {0}", string.Join(",", fields));
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                s.AppendFormat(" ORDER BY {0}", orderby);
            }
            if(hasTopNum&&!isSqlServer)
            {
                switch (topType)
                {
                    case DatabaseType.MySql:
                        s.AppendFormat(" LIMIT 0,{0}", top);
                        break;
                    case DatabaseType.Sqlite:
                        s.AppendFormat(" LIMIT {0} OFFSET 0", top);
                        break;
                }
            }
            return s.ToString();
        }
    }
}
