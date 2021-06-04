using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroDbs.Common
{
    public class SqlDeleteBuilder : ISqlDeleteBuilder
    {
        string tableName = "";
        string where = "";
        string datetimeFormat = "";
        string[] whereFields = null;
        object[] whereValues = null;
        public SqlDeleteBuilder(string tableName)
        {
            this.tableName = tableName;
        }
        public ISqlDeleteBuilder WhereFields(params string[] fields)
        {
            this.whereFields = fields;
            return this;
        }
        public ISqlDeleteBuilder WhereValues(params object[] values)
        {
            this.whereValues = values;
            return this;
        }
        public ISqlDeleteBuilder Where(string where)
        {
            this.where = where;
            return this;
        }
        public ISqlDeleteBuilder DateTimeFormat(string format)
        {
            this.datetimeFormat = format;
            return this;
        }
        public override string ToString()
        {
            StringBuilder s = new StringBuilder("DELETE FROM");
            s.AppendFormat(" {0}", tableName);
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
