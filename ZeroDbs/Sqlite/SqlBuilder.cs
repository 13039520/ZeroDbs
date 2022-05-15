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
        public override string GetTableName(ITableInfo tableInfo)
        {
            return string.Format("[{0}]", tableInfo.Name);
        }
        public override string GetColunmName(string colName)
        {
            return string.Format("[{0}]", colName);
        }
        public override Common.SqlInfo Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField = "", params object[] paras)
        {
            var tableInfo = this.GetTable<DbEntity>();

            page = page < 1 ? 1 : page;
            size = size < 1 ? 1 : size;

            long startIndex = page * size - size;
            StringBuilder sql = new StringBuilder();
            StringBuilder fieldStr = new StringBuilder();
            bool needCheck = true;
            if (fields == null || fields.Length < 1)
            {
                needCheck = false;
                fields = tableInfo.Colunms.FindAll(o => o.MaxLength < 1000).Select(o => o.Name).ToArray();
            }
            for (int i = 0; i < fields.Length; i++)
            {
                if (needCheck)
                {
                    var col = tableInfo.Colunms.Find(o => string.Equals(o.Name, fields[i], StringComparison.OrdinalIgnoreCase));
                    if (col != null)
                    {
                        fieldStr.AppendFormat("{0},", GetColunmName(col.Name));
                    }
                }
                else
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
            if (string.IsNullOrEmpty(orderby))
            {
                orderby = "(SELECT NULL)";
            }
            if (string.IsNullOrEmpty(uniqueField))
            {
                var ts = GetUniqueFieldName(tableInfo);
                uniqueField = ts.Length == 1 ? ts[0] : string.Empty;
            }
            string tableName = GetTableName(tableInfo);
            if (!string.IsNullOrEmpty(uniqueField))//具有唯一性字段
            {
                string resultFieldName = uniqueField;

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
            Common.SqlInfo reval = new Common.SqlInfo(paras.Length);
            reval.Sql = sql.ToString();
            SqlInfoUseParas(ref reval, paras);
            return reval;
        }
        public override Common.SqlInfo Select<DbEntity>(string where, string orderby, int top, string[] fields, params object[] paras)
        {
            Common.SqlInfo reval = new Common.SqlInfo(paras.Length);
            var tableInfo = this.GetTable<DbEntity>();
            StringBuilder field = new StringBuilder();
            bool needCheck = true;
            if (fields == null || fields.Length < 1)
            {
                needCheck = false;
                fields = tableInfo.Colunms.FindAll(o => o.MaxLength < 1000).Select(o => o.Name).ToArray();
            }
            for (int i = 0; i < fields.Length; i++)
            {
                if (needCheck)
                {
                    var col = tableInfo.Colunms.Find(o => string.Equals(o.Name, fields[i], StringComparison.OrdinalIgnoreCase));
                    if (col != null)
                    {
                        field.AppendFormat("{0},", GetColunmName(col.Name));
                    }
                }
                else
                {
                    field.AppendFormat("{0},", GetColunmName(fields[i]));
                }
            }
            if (field.Length > 0)
            {
                field.Remove(field.Length - 1, 1);
            }
            else
            {
                field.Append("*");
            }
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
                    reval.Sql = string.Format("SELECT {1} FROM {2} WHERE {3} LIMIT {0}", top, field, tableName, where);
                }
                else
                {
                    reval.Sql = string.Format("SELECT {1} FROM {2} WHERE {3} ORDER BY {4} LIMIT {0}", top, field, tableName, where, orderby);
                }
            }
            SqlInfoUseParas(ref reval, paras);
            return reval;
        }


    }

}
