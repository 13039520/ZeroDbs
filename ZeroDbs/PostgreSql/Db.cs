using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using ZeroDbs.Common;

namespace ZeroDbs.PostgreSql
{
    public class Db: Common.Db
    {
        readonly string tableInfoQuerySql;
        IDbParameterCreator postgreSqlParameterCreator;
        public Db(IDbInfo dbInfo) :base()
        {
            this.dbInfo = dbInfo;
            this.sqlBuilder = new SqlBuilder(this);
            this.dataTypeMaping = new DbDataTypeMaping();
            postgreSqlParameterCreator = new PostgreSqlParameterCreator(dbInfo.Type);
            tableInfoQuerySql = "select ordinal_position,column_name,raw_typname as type_name,data_type,"
                + "coalesce(character_maximum_length,numeric_precision,0) as max_length,"
                + "character_octet_length,numeric_scale,"
                + "case is_nullable when 'NO' then 0 else 1 end as can_null,"
                + "column_default as default_value,"
                + "case  when (is_identity='YES' OR position('nextval' in column_default)>0) then 1 else 0 end as is_identity,"
                + "case when b.pk_name is null then 0 else 1 end as is_pk,c.DeText as comment"
                + " from information_schema.columns"
                + " left join ("
                + "select pg_attr.attname as colname,pg_constraint.conname as pk_name from pg_constraint"
                + " inner join pg_class on pg_constraint.conrelid = pg_class.oid"
                + " inner join pg_attribute pg_attr on pg_attr.attrelid = pg_class.oid and  pg_attr.attnum = pg_constraint.conkey[1]"
                + " inner join pg_type on pg_type.oid = pg_attr.atttypid"
                + " where pg_class.relname='{0}' and pg_constraint.contype='p'"
                + ") b on b.colname = information_schema.columns.column_name"
                + " left join ("
                + "select attname,description as DeText,pg_type.typname as raw_typname from pg_class"
                + " left join pg_attribute pg_attr on pg_attr.attrelid= pg_class.oid"
                + " left join pg_description pg_desc on pg_desc.objoid = pg_attr.attrelid and pg_desc.objsubid=pg_attr.attnum"
                + " left join pg_type on pg_type.oid=pg_attr.atttypid"
                + " where pg_attr.attnum>0 and pg_attr.attrelid=pg_class.oid and pg_class.relname='{0}'"
                + ")c on c.attname = information_schema.columns.column_name"
                + " where table_schema='public' and table_name='{0}' order by ordinal_position asc";
        }
        public override IDbCommand CreateDbCommand(System.Data.Common.DbCommand cmd)
        {
            return base.CreateDbCommand(cmd, null);//postgreSqlWriteParameterCorrector
        }
        public override System.Data.Common.DbConnection GetDbConnection(bool useSecondDb = false)
        {
            if (useSecondDb && !string.IsNullOrEmpty(DbInfo.ConnectionString2))
            {
                return new NpgsqlConnection(DbInfo.ConnectionString2);
            }
            return new NpgsqlConnection(DbInfo.ConnectionString);
        }
        public override ITableInfo GetTable<DbEntity>()
        {
            return GetTable(typeof(DbEntity).FullName);
        }
        public override ITableInfo GetTable(string entityFullName)
        {
            if (!IsMappingToDbKey(entityFullName))
            {
                throw new Exception("类型" + entityFullName + "没有映射到" + DbInfo.Key + "上");
            }
            string key = entityFullName;
            var value = Common.DbDataviewStructCache.Get(key);
            if (value != null)
            {
                return value;
            }

            var cmd = this.GetDbCommand();
            try
            {
                var dv = Common.DbMapping.GetDbTableEntityMapByEntityFullName(entityFullName).Find(o => string.Equals(o.DbKey, DbInfo.Key, StringComparison.OrdinalIgnoreCase));
                string getTableOrViewSql = "select * from sqlite_master where name='" + dv.TableName + "' and type IN('table','view')";
                getTableOrViewSql = "SELECT A.oid,A.relname,A.relkind,B.description as comment"
                    + " FROM pg_class AS A"
                    + " LEFT JOIN pg_description AS B ON B.objoid=A.oid AND B.objsubid=0"
                    + " WHERE A.relname='" + dv.TableName + "' AND A.relkind IN('r','v')"
                    + " AND A.relnamespace=(SELECT oid FROM pg_namespace WHERE nspname='public' LIMIT 1)"
                    + " ORDER BY relkind,relname";

                Common.TableInfo dbDataTableInfo = null;
                cmd.CommandText = getTableOrViewSql;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dbDataTableInfo = ToTableInfo(reader, cmd.DbConnection.Database);
                }
                reader.Close();
                reader.Dispose();

                if (dbDataTableInfo == null)
                {
                    throw new Exception("查询" + dv.TableName + "的表信息不成功");
                }

                string getColumnInfoSql = string.Format(tableInfoQuerySql, dv.TableName);
                cmd.CommandText = getColumnInfoSql;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dbDataTableInfo.Colunms.Add(ToColumnInfo(reader));
                }
                reader.Close();
                reader.Dispose();

                cmd.Dispose();

                Common.DbDataviewStructCache.Set(key, dbDataTableInfo);

                return dbDataTableInfo;

            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw;
            }
        }
        public override List<ITableInfo> GetTables()
        {
            var cmd = this.GetDbCommand();
            try
            {
                var dbName = cmd.DbConnection.DataSource;//cmd.DbConnection.Database;
                List<string> sqlList = new List<string>();
                string getAllTableAndViewSql = "SELECT A.oid,A.relname,A.relkind,B.description as comment"
                    +" FROM pg_class AS A"
                    +" LEFT JOIN pg_description AS B ON B.objoid=A.oid AND B.objsubid=0"
                    +" WHERE A.relkind IN('r','v')"
                    +" AND A.relnamespace=(SELECT oid FROM pg_namespace WHERE nspname='public' LIMIT 1)"
                    +" ORDER BY relkind,relname";
               
                List<ITableInfo> List = new List<ITableInfo>();
                cmd.CommandText = getAllTableAndViewSql;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    List.Add(ToTableInfo(reader, cmd.DbConnection.Database));
                }
                reader.Close();
                reader.Dispose();

                foreach (TableInfo m in List)
                {
                    string sql = string.Format(tableInfoQuerySql,m.Name);
                    cmd.CommandText = sql;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        m.Colunms.Add(ToColumnInfo(reader));
                    }
                    reader.Close();
                    reader.Dispose();
                }

                cmd.Dispose();

                return List;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw;
            }
        }
        private TableInfo ToTableInfo(System.Data.IDataReader reader, string dbName)
        {
            TableInfo t = new TableInfo();
            string type = (reader["relkind"].ToString()).Trim();
            string name = (reader["relname"].ToString()).Trim();
            string comment = (reader["comment"].ToString()).Trim();
            t.DbName = dbName;//cmd.DbConnection.Database;
            t.Name = name;
            t.IsView = "r" != type;
            t.Description = string.IsNullOrEmpty(comment) ? (t.IsView ? "VIEW:" : "TABLE:") + t.Name : comment;
            t.Colunms = new List<IColumnInfo>();
            return t;
        }
        private ColumnInfo ToColumnInfo(System.Data.IDataReader reader)
        {
            ColumnInfo column = new Common.ColumnInfo();
            int isIdentity = Convert.ToInt32(reader["is_identity"]);
            int isPk = Convert.ToInt32(reader["is_pk"]);
            int canNull = Convert.ToInt32(reader["can_null"]);
            int index = reader.GetOrdinal("numeric_scale");
            int numeric_scale = -1;
            if (!reader.IsDBNull(index))
            {
                numeric_scale = reader.GetInt32(index);
            }
            index = reader.GetOrdinal("character_octet_length");
            int character_octet_length = -1;
            if (!reader.IsDBNull(index))
            {
                character_octet_length = reader.GetInt32(index);
            }
            string type = reader["type_name"].ToString();
            string dtype = reader["data_type"].ToString();
            string comment = reader["comment"].ToString();
            column.Name = reader["column_name"].ToString();
            column.MaxLength = Convert.ToInt32(reader["max_length"]);
            column.Byte = character_octet_length;
            column.DecimalDigits = numeric_scale != -1 ? numeric_scale : -1;
            column.DefaultValue = null;
            column.Description = string.IsNullOrEmpty(comment) ? string.Format("{0}({1})", type, dtype) : comment;
            column.IsIdentity = isIdentity != 0;
            column.IsNullable = canNull != 0;
            column.IsPrimaryKey = isPk != 0;
            column.Type = this.DataTypeMaping.GetDotNetTypeFullName(type, column.MaxLength);

            return column;
        }

    }
    
}
