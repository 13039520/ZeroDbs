using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroDbs.Common
{
    public class SqlUpdateBuilder : ISqlUpdateBuilder
    {
        string tableName = "";
        string where = "";
        string datetimeFormat = "";
        string[] fields = null;
        object[] values = null;
        string[] whereFields = null;
        object[] whereValues = null;
        public SqlUpdateBuilder(string tableName)
        {
            this.tableName = tableName;
        }
        public ISqlUpdateBuilder SetFields(params string[] fields)
        {
            this.fields = fields;
            return this;
        }
        public ISqlUpdateBuilder SetValues(params object[] values)
        {
            this.values = values;
            return this;
        }
        public ISqlUpdateBuilder WhereFields(params string[] fields)
        {
            this.whereFields = fields;
            return this;
        }
        public ISqlUpdateBuilder WhereValues(params object[] values)
        {
            this.whereValues = values;
            return this;
        }
        public ISqlUpdateBuilder Where(string where)
        {
            this.where = where;
            return this;
        }
        public ISqlUpdateBuilder DateTimeFormat(string format)
        {
            this.datetimeFormat = format;
            return this;
        }
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("UPDATE {0}", tableName);
            if (fields != null && fields.Length > 0)
            {
                s.Append(" SET ");
                if (values != null && values.Length > 0)
                {
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (i < values.Length)
                        {
                            string val = ValueConvert.SqlValueStrByValue(values[i], datetimeFormat);
                            s.AppendFormat("{0}={1},", fields[i], val);
                        }
                        else
                        {
                            s.AppendFormat("{0}=NULL,", fields[i]);
                        }
                    }
                }
                else
                {
                    foreach (string name in fields)
                    {
                        s.AppendFormat("{0}=@{0},", name);
                    }
                }
                s.Remove(s.Length - 1, 1);
            }
            if (whereFields != null && whereFields.Length > 0)
            {
                s.Append(" WHERE ");
                if (whereValues != null && whereValues.Length > 0)
                {
                    for (int i = 0; i < whereFields.Length; i++)
                    {
                        if (i < whereValues.Length)
                        {
                            string val = ValueConvert.SqlValueStrByValue(whereValues[i], datetimeFormat);
                            s.AppendFormat("{0}={1} AND ", whereFields[i], val);
                        }
                        else
                        {
                            s.AppendFormat("{0}=NULL AND ", whereFields[i]);
                        }
                    }
                }
                else
                {
                    foreach (string name in whereFields)
                    {
                        s.AppendFormat("{0}=@{0} AND ", name);
                    }
                }
                if (!string.IsNullOrEmpty(where))
                {
                    s.Append(where);
                }
                else
                {
                    s.Remove(s.Length - 5, 5);//移除" AND "
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(where))
                {
                    s.AppendFormat(" WHERE {0}", where);
                }
            }
            
            return s.ToString();
        }

    }
}
