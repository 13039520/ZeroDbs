using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroDbs.Common
{
    public class SqlInsertBuilder : ISqlInsertBuilder
    {
        string tableName = "";
        string where = "";
        string datetimeFormat = "";
        string[] fields = null;
        object[] values = null;
        public SqlInsertBuilder(string tableName)
        {
            this.tableName = tableName;
        }
        public ISqlInsertBuilder Fields(params string[] fields)
        {
            this.fields = fields;
            return this;
        }
        public ISqlInsertBuilder Values(params object[] values)
        {
            this.values = values;
            return this;
        }
        public ISqlInsertBuilder Where(string where)
        {
            this.where = where;
            return this;
        }
        public ISqlInsertBuilder DateTimeFormat(string format)
        {
            this.datetimeFormat = format;
            return this;
        }
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("INSERT INTO {0}", tableName);
            if (fields != null && fields.Length > 0)
            {
                s.AppendFormat("({0})", string.Join(",", fields));
                s.Append(" VALUES(");
                if (values != null && values.Length > 0)
                {
                    for (int i = 0; i < fields.Length; i++)
                    {
                        string val = "NULL";
                        if (i < values.Length)
                        {
                            val = ValueConvert.SqlValueStrByValue(values[i], datetimeFormat);
                        }
                        s.AppendFormat("{0},", val);
                    }
                }
                else
                {
                    foreach (string name in fields)
                    {
                        s.AppendFormat("@{0},", name);
                    }
                }
                s.Remove(s.Length - 1, 1);
                s.Append(")");
            }
            if (!string.IsNullOrEmpty(where))
            {
                s.AppendFormat(" WHERE {0}", where);
            }
            return s.ToString();
        }
    }
}
