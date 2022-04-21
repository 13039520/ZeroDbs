﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs.Sqlite
{
    internal class SqlBuilder:Common.SqlBuilder
    {
        public SqlBuilder(IDb db) : base(db)
        {

        }
        public override string Page<DbEntity>(long page, long size, string where, string orderby, string[] returnFieldNames, string uniqueFieldName = "")
        {
            var tableInfo = this.GetTable<DbEntity>();

            page = page < 1 ? 1 : page;
            size = size < 1 ? 1 : size;

            long startIndex = page * size - size;
            StringBuilder sql = new StringBuilder();
            StringBuilder fieldStr = new StringBuilder();
            if (returnFieldNames == null)
            {
                returnFieldNames = tableInfo.Colunms.FindAll(o => o.MaxLength < 1000).Select(o => o.Name).ToArray();
            }
            for (int i = 0; i < returnFieldNames.Length; i++)
            {
                if (!string.IsNullOrEmpty(returnFieldNames[i]))
                {
                    fieldStr.AppendFormat("{0},", returnFieldNames[i]);
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
            if (string.IsNullOrEmpty(orderby))
            {
                orderby = "(SELECT NULL)";
            }
            if (string.IsNullOrEmpty(uniqueFieldName))
            {
                var ts = GetUniqueFieldName(tableInfo);
                uniqueFieldName = ts.Length == 1 ? ts[0] : string.Empty;
            }
            string tableName = GetTableName(tableInfo);
            if (!string.IsNullOrEmpty(uniqueFieldName))//具有唯一性字段
            {
                string resultFieldName = uniqueFieldName;

                sql.AppendFormat("SELECT {0} FROM {1}", fieldStr, tableName);
                //获取唯一性字段集合
                sql.AppendFormat(" WHERE {0} IN(", resultFieldName);
                sql.AppendFormat("SELECT {0} FROM {1}", resultFieldName, tableName);
                if (!string.IsNullOrEmpty(where))
                {
                    sql.AppendFormat(" WHERE {0}", where);
                }
                sql.AppendFormat(" ORDER BY {0}", orderby);
                sql.AppendFormat(" LIMIT {0} OFFSET {1}", size, startIndex);
                sql.AppendFormat(") ORDER BY {0}", orderby);
            }
            else//没有唯一性字段
            {
                sql.AppendFormat("SELECT {0} FROM {1}", fieldStr, tableName);
                if (!string.IsNullOrEmpty(where))
                {
                    sql.AppendFormat(" WHERE {0}", where);
                }
                sql.AppendFormat(" ORDER BY {0}", orderby);
                sql.AppendFormat(" LIMIT {0} OFFSET {1}", size, startIndex);
            }
            return sql.ToString();
        }
        public override Common.SqlInfo Select<DbEntity>(string where, string orderby, int top, string[] returnFieldNames, params object[] paras)
        {
            Common.SqlInfo reval = new Common.SqlInfo();
            var tableInfo = this.GetTable<DbEntity>();
            string[] fieldArray = tableInfo.Colunms.Select(o => o.Name).ToArray();
            if (returnFieldNames != null && returnFieldNames.Length > 0)
            {
                int i = 0;
                List<string> temp = new List<string>();
                while (i < returnFieldNames.Length)
                {
                    int j = 0;
                    while (j < fieldArray.Length)
                    {
                        if (string.Equals(returnFieldNames[i], fieldArray[j], StringComparison.OrdinalIgnoreCase))
                        {
                            temp.Add(fieldArray[j]);
                            break;
                        }
                        j++;
                    }
                    i++;
                }
                returnFieldNames = temp.Distinct().ToArray();
                if (returnFieldNames.Length > 0)
                {
                    fieldArray = returnFieldNames;
                }
            }
            StringBuilder field = new StringBuilder();
            foreach (string s in fieldArray)
            {
                field.AppendFormat("{0},", s);
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
            if (top < 1)
            {
                if (string.IsNullOrEmpty(orderby))
                {
                    reval.Sql = string.Format("SELECT {0} FROM {1} WHERE {2}", field, GetTableName(tableInfo), where);
                }
                else
                {
                    reval.Sql = string.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3}", field, GetTableName(tableInfo), where, orderby);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(orderby))
                {
                    reval.Sql = string.Format("SELECT {1} FROM {2} WHERE {3} LIMIT {0}", top, field, GetTableName(tableInfo), where);
                }
                else
                {
                    reval.Sql = string.Format("SELECT {1} FROM {2} WHERE {3} ORDER BY {4} LIMIT {0}", top, field, GetTableName(tableInfo), where, orderby);
                }
            }
            int n = 0;
            int m = paras.Length;
            while (n < m)
            {
                reval.Paras.Add(n.ToString(), paras[n]);
                n++;
            }
            return reval;
        }
    }

}