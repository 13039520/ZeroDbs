﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs.MySql
{
    internal class SqlBuilder: Common.SqlBuilder
    {
        public SqlBuilder(IDb db) : base(db)
        {

        }
        public override string GetTableName(Common.DbDataTableInfo tableInfo)
        {
            return string.Format("`{0}`", tableInfo.Name);
        }
        public override string GetColunmName(string colName)
        {
            return String.Format("`{0}`", colName);
        }
        public override Common.SqlInfo Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField = "", params object[] paras)
        {
            var tableInfo = this.ZeroDb.GetTable<DbEntity>();

            page = page < 0 ? 0 : page;
            page = page > 0 ? page - 1 : page;
            size = size < 1 ? 1 : size;

            long startIndex = page * size;
            long endIndex = startIndex + size;
            StringBuilder sql = new StringBuilder();
            StringBuilder fieldStr = new StringBuilder();
            for (int i = 0; i < fields.Length; i++)
            {
                if (!string.IsNullOrEmpty(fields[i]))
                {
                    fieldStr.AppendFormat("{0},", GetColunmName(fields[i]));
                }
            }
            if (fieldStr.Length > 0)
            {
                fieldStr.Remove(fieldStr.Length - 1, 1);
            }
            else
            {
                fieldStr.Append("*");
            }
            if (string.IsNullOrEmpty(uniqueField))
            {
                var ts = GetUniqueFieldName(tableInfo);
                uniqueField = ts.Length == 1 ? ts[0] : string.Empty;
            }
            var tableName= GetTableName(tableInfo);
            if (!string.IsNullOrEmpty(uniqueField))
            {
                uniqueField = GetColunmName(uniqueField);
                string SubSql = string.Format("SELECT {0} FROM {1}", uniqueField, tableName);
                if (!string.IsNullOrEmpty(where))
                {
                    SubSql += string.Format(" WHERE {0}", where);
                }
                if (!string.IsNullOrEmpty(orderby))
                {
                    SubSql += string.Format(" ORDER BY {0}", orderby);
                }
                SubSql += string.Format(" LIMIT {0},{1}", startIndex, size);

                sql.AppendFormat("SELECT {0} FROM {1} WHERE {2} IN(SELECT {2} FROM ({3}) AS DbEntity)", fieldStr, tableName, uniqueField, SubSql);
                if (!string.IsNullOrEmpty(orderby))
                {
                    sql.AppendFormat(" ORDER BY {0}", orderby);
                }
            }
            else
            {
                sql.AppendFormat("SELECT {0} FROM {1}", fieldStr, tableName);
                if (!string.IsNullOrEmpty(where))
                {
                    sql.AppendFormat(" WHERE {0}", where);
                }
                if (!string.IsNullOrEmpty(orderby))
                {
                    sql.AppendFormat(" ORDER BY {0}", orderby);
                }
                sql.AppendFormat(" LIMIT {0},{1}", startIndex, size);
            }
            Common.SqlInfo reval = new Common.SqlInfo();
            reval.Sql = sql.ToString();
            int n = 0;
            int m = paras.Length;
            while (n < m)
            {
                reval.Paras.Add(n.ToString(), paras[n]);
                n++;
            }
            return reval;
        }
        public override Common.SqlInfo Select<DbEntity>(string where, string orderby, int top, string[] fields, params object[] paras)
        {
            Common.SqlInfo reval = new Common.SqlInfo();
            var tableInfo = this.GetTable<DbEntity>();
            string[] fieldArray = tableInfo.Colunms.Select(o => o.Name).ToArray();
            if (fields != null && fields.Length > 0)
            {
                int i = 0;
                List<string> temp = new List<string>();
                while (i < fields.Length)
                {
                    int j = 0;
                    while (j < fieldArray.Length)
                    {
                        if (string.Equals(fields[i], fieldArray[j], StringComparison.OrdinalIgnoreCase))
                        {
                            temp.Add(fieldArray[j]);
                            break;
                        }
                        j++;
                    }
                    i++;
                }
                fields = temp.Distinct().ToArray();
                if (fields.Length > 0)
                {
                    fieldArray = fields;
                }
            }
            StringBuilder field = new StringBuilder();
            foreach (string s in fieldArray)
            {
                field.AppendFormat("{0},", GetColunmName(s));
            }
            if (field.Length < 1)
            {
                throw new Exception("The resulting return field is invalid");
            }
            field.Remove(field.Length - 1, 1);
            if (string.IsNullOrEmpty(where))
            {
                where = "1>0";
            }
            var tableName = GetTableName(tableInfo);
            if (top < 1)
            {
                if (string.IsNullOrEmpty(orderby))
                {
                    reval.Sql = string.Format("SELECT {0} FROM {1} WHERE {2}", field, tableName, where);
                }
                else
                {
                    reval.Sql = string.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3}", field, tableName, where, orderby);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(orderby))
                {
                    reval.Sql = string.Format("SELECT {0} FROM {1} WHERE {2} LIMIT {3},{4}", field, tableName, where, 0, top);
                }
                else
                {
                    reval.Sql = string.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3} LIMIT {4},{5}", field, tableName, where, orderby, 0, top);
                }
            }
            int n = 0;
            int m = paras.Length;
            while(n < m)
            {
                reval.Paras.Add(n.ToString(), paras[n]);
                n++;
            }
            return reval;
        }


    }
}
