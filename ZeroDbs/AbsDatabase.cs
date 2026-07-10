using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ZeroDbs
{
    public abstract class AbsDatabase
    {
        readonly string _dbKey;
        readonly string _connectionString;
        readonly ISnowflakeIdGenerator _snowflake;
        /// <summary>
        /// [protected]数据库连接
        /// </summary>
        protected string ConnectionString { get { return _connectionString; } }
        public string DbKey { get { return _dbKey; } }
        /// <summary>
        /// [abstract]数据库类型
        /// </summary>
        public abstract string DbType { get; }
        /// <summary>
        /// 雪花ID生成器
        /// </summary>
        public ISnowflakeIdGenerator Snowflake {  get { return _snowflake; } }

        public AbsDatabase(string dbKey, string connectionString, ISnowflakeIdGenerator snowflake)
        {
            _dbKey = dbKey;
            _connectionString = connectionString;
            _snowflake = snowflake;
        }

        #region -- 编译方法(protected) --
        protected ISql CompileInValues(IInValueOptions inValuesOptions, int pIndex)
        {
            var opts = inValuesOptions;
            if (opts == null || opts.Count < 1) { throw new ArgumentNullException(nameof(inValuesOptions)); }
            if (pIndex < 0) { throw new ArgumentOutOfRangeException(nameof(pIndex)); }
            int index = pIndex;
            List<object> ps = new List<object>(opts.Count);
            List<string> pns = new List<string>(opts.Count);
            for (int i = 0; i < opts.Count; i++)
            {
                pns.Add(Param(i + index));
                ps.Add(opts[i]);
            }
            return new Sql { Text = string.Join(",", pns), Params = ps.ToArray() };
        }
        protected ISql CompileWhere(IWhereOptions opts, int pIndex)
        {
            if (opts == null || opts.Count < 1) { throw new ArgumentNullException(nameof(opts)); }
            if (pIndex < 0) { pIndex = 0; }

            List<object> ps = new List<object>();
            StringBuilder gSql = new StringBuilder();
            int gIndex = pIndex;
            foreach (IWhereGroup group in opts)
            {
                if (gIndex > pIndex)
                {
                    gSql.Append(group.IsAnd ? " AND " : " OR ");
                }
                gIndex++;
                gSql.Append("(");
                int partIndex = 0;
                foreach (IWherePartOptions p in group)
                {
                    string pSql = p.Template;
                    if (p.Fields != null)
                    {
                        //倒序替换(避免小编号替换了大编号)
                        for (int i = p.Fields.Length - 1; i >= 0; i--)
                        {
                            string num = i.ToString();
                            pSql = pSql.Replace("@n" + num, Quote(p.Fields[i]));
                        }
                    }
                    if (p.Params != null)
                    {
                        int paramIndex = pIndex + ps.Count;
                        List<string> pNames = new List<string>(p.Params.Length);
                        for (int i = 0; i < p.Params.Length; i++)
                        {
                            if (p.Params[i] is IInValueOptions)
                            {
                                var r = CompileInValues((IInValueOptions)p.Params[i], paramIndex);
                                if (r.Params == null)
                                {
                                    throw new InvalidOperationException("IInValuesOptions.Params is null");
                                }
                                pNames.Add(r.Text);
                                ps.AddRange(r.Params);
                                paramIndex += r.Params.Length;
                            }
                            else
                            {
                                ps.Add(p.Params[i]);
                                pNames.Add(Param(paramIndex));
                                paramIndex++;
                            }
                        }
                        //倒序替换参数
                        for (int i = pNames.Count - 1; i >= 0; i--)
                        {
                            pSql = pSql.Replace("@p" + i, pNames[i]);
                        }
                    }
                    if (partIndex > 0)
                    {
                        gSql.Append(p.IsAnd ? " AND " : " OR ");
                    }
                    gSql.Append(pSql);
                    partIndex++;
                    gIndex++;
                }
                gSql.Append(")");
            }
            return new Sql { Text = gSql.ToString(), Params = ps.ToArray() };
        }
        protected ISql CompileSql(ISqlOptions opts, int pIndex)
        {
            if (opts == null) { throw new ArgumentNullException(nameof(opts)); }
            if (string.IsNullOrWhiteSpace(opts.Template)) { throw new ArgumentNullException(opts.Template); }
            if (pIndex < 0) { pIndex = 0; }//这里的 pIndex 参数只是为了符合委托签名，没有偏移的必要
            List<object> ps = new List<object>();
            string sql = opts.Template;
            if (opts.Names != null)
            {
                for (int i = 0; i < opts.Names.Length; i++)
                {
                    sql = sql.Replace("@n" + i, Quote(opts.Names[i]));
                }
            }
            if (opts.Params != null)
            {
                List<string> pNames = new List<string>();
                foreach (object p in opts.Params)
                {
                    string nPName;
                    if (p is IWhereOptions)
                    {
                        var r = CompileWhere((IWhereOptions)p, pIndex);
                        if (r.Params != null && r.Params.Length > 0)
                        {
                            ps.AddRange(r.Params);
                            pIndex += r.Params.Length;
                        }
                        nPName = r.Text;
                    }
                    else if (p is IInValueOptions)
                    {
                        var r = CompileInValues((IInValueOptions)p, pIndex);
                        if (r.Params == null || r.Params.Length < 1)
                        {
                            throw new ArgumentException("IInValuesOptions.Params is null");
                        }
                        ps.AddRange(r.Params);
                        nPName = r.Text;
                        pIndex += r.Params.Length;
                    }
                    else
                    {
                        ps.Add(p);
                        nPName = Param(pIndex);
                        pIndex++;
                    }
                    pNames.Add(nPName);
                }
                for (int i = pNames.Count - 1; i >= 0; i--)
                {
                    sql = sql.Replace("@p" + i, pNames[i]);
                }
            }
            return new Sql { Text = sql, Params = ps.ToArray() };
        }
        protected ISql CompileInsertSql(IInsertOptions opts, int pIndex)
        {
            if (opts == null) { throw new ArgumentNullException(nameof(opts)); }
            if (string.IsNullOrWhiteSpace(opts.TableName)) { throw new ArgumentNullException(nameof(opts.TableName)); }
            if (opts.KeyValuePairs == null || opts.KeyValuePairs.Count < 1) { throw new ArgumentNullException(nameof(opts.KeyValuePairs)); }

            bool ignoreFieldsNotNull = opts.IgnoreFields != null && opts.IgnoreFields.Count > 0;

            List<string> fields = new List<string>();
            List<string> placeholders = new List<string>();
            List<object> values = new List<object>();
            int index = 0;
            foreach (var item in opts.KeyValuePairs)
            {
                if (ignoreFieldsNotNull && opts.IgnoreFields.Contains(item.Key)) { continue; }
                fields.Add(Quote(item.Key));
                placeholders.Add(Param(index));
                values.Add(item.Value);
                index++;
            }
            if (index < 1) { throw new ArgumentException("none insert fields", nameof(opts.KeyValuePairs)); }
            string format = string.Format("INSERT INTO {0}({{0}}) VALUES({{1}})", Quote(opts.TableName));
            StringBuilder sql = new StringBuilder("INSERT INTO ");
            sql.Append(Quote(opts.TableName));
            sql.AppendFormat("({0}) VALUES({1});", string.Join(",", fields), string.Join(",", placeholders));
            if (!string.IsNullOrWhiteSpace(opts.ReturnIdentityColumn))
            {
                sql.Append(GetReturnIdentityColumnSqlPart(opts.ReturnIdentityColumn));
            }
            return new Sql { Text = sql.ToString(), Params = values.ToArray() };
        }
        protected ISql CompileUpdateSql(IUpdateOptions opts, int pIndex)
        {
            if (opts == null) { throw new ArgumentNullException(nameof(opts)); }
            if (string.IsNullOrWhiteSpace(opts.TableName)) { throw new ArgumentNullException(nameof(opts.TableName)); }
            if (opts.KeyValuePairs == null || opts.KeyValuePairs.Count < 1) { throw new ArgumentNullException(nameof(opts.KeyValuePairs)); }

            List<object> values = new List<object>();
            List<string> setParts = new List<string>();
            int index = values.Count;
            foreach (var v in opts.KeyValuePairs)
            {
                string pName = Param(index);
                setParts.Add(string.Format("{0}={1}", Quote(v.Key), pName));
                values.Add(v.Value);
                index++;
            }
            ISql whereSql = null;
            if (opts.Where != null && opts.Where.Count > 0)
            {
                whereSql = opts.Where.Compile(index);
                values.AddRange(whereSql.Params);
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE ");
            sql.Append(Quote(opts.TableName));
            sql.Append(" SET ");
            sql.Append(string.Join(",", setParts));
            if (whereSql != null)
            {
                sql.Append(" WHERE ");
                sql.Append(whereSql.Text);
            }
            return new Sql { Text = sql.ToString(), Params = values.ToArray() };
        }
        protected ISql CompileDeleteSql(IDeleteOptions opts, int pIndex)
        {
            if (opts == null) { throw new ArgumentNullException(nameof(opts)); }
            if (string.IsNullOrWhiteSpace(opts.TableName)) { throw new ArgumentNullException(nameof(opts.TableName)); }

            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE FROM ");
            sql.Append(Quote(opts.TableName));
            ISql whereSql = null;
            if (opts.Where != null && opts.Where.Count > 0)
            {
                whereSql = opts.Where.Compile(0);
            }
            if (whereSql != null)
            {
                sql.Append(" WHERE ");
                sql.Append(whereSql.Text);
            }
            return new Sql { Text = sql.ToString(), Params = whereSql.Params };
        }
        #endregion

        #region -- 通用辅助方法 --
        public Guid SequentialGuid()
        {
            return SequentialGuidGenerator.Next();
        }
        public ISqlParameter SqlParam(object value, System.Data.ParameterDirection? direction, string? dbDataTypeName = "")
        {
            return new SqlParameter
            {
                Value = value,
                DbDataTypeName = dbDataTypeName,
                Direction = direction.HasValue ? direction.Value : ParameterDirection.Input
            };
        }
        public IInsertOptions InsertOptions(string tableName)
        {
            return new InsertOptions(CompileInsertSql).SetTableName(tableName);
        }
        public IDeleteOptions DeleteOptions(string tableName)
        {
            return new DeleteOptions(CompileDeleteSql).SetTableName(tableName);
        }
        public IUpdateOptions UpdateOptions(string tableName)
        {
            return new UpdateOptions(CompileUpdateSql).SetTableName(tableName);
        }
        public ISelectOptions<T> SelectOptions<T>(string tableName)
        {
            return SelectOptions<T>().SetTableName(tableName);
        }
        public IPageOptions<T> PageOptions<T>(string tableName)
        {
            return new PageOptions<T>().SetTableName(tableName);
        }
        public ISelectOptions<T> SelectOptions<T>()
        {
            return new SelectOptions<T>();
        }
        public IInValueOptions InValueOptions<T>(params T[] values)
        {
            if (values == null || values.Length == 0) { throw new ArgumentNullException(nameof(values)); }
            object[] os = new object[values.Length];
            for (int i = 0; i < os.Length; i++)
            {
                os[i] = values[i];
            }
            return new InValueOptions(CompileInValues, os);
        }
        public IWherePartOptions WherePartOptions(string template, bool isAnd = true)
        {
            if (string.IsNullOrWhiteSpace(template)) {  throw new ArgumentNullException(nameof(template)); }
            return new WherePartOptions(template, isAnd);
        }
        public IWhereOptions WhereOptions(params IWherePartOptions[] parts)
        {
            var opt = new WhereOptions(CompileWhere);
            if (parts != null && parts.Length > 0)
            {
                opt.And(parts);//第一个组的 AND 和 OR 会被忽略
            }
            return opt;
        }
        public ISqlOptions SqlOptions(string template)
        {
            if(template == null) { throw new ArgumentNullException(nameof (template)); }
            return new SqlOptions(CompileSql, template);
        }
        public IRawSqlOptions RawSqlOptions(ISqlOptions sqlOpts)
        {
            var t = sqlOpts.Compile();
            return new RawSqlOptions().SetCmdText(t.Text, t.Params);
        }
        public IRawSqlOptions RawSqlOptions(ISql sql) {
            return new RawSqlOptions().SetCmdText(sql.Text, sql.Params);
        }
        public IRawSqlOptions RawSqlOptions(string cmdText, params object[]? cmdParams)
        {
            return new RawSqlOptions().SetCmdText(cmdText, cmdParams);
        }
        public INameOptions NameOptions(string name) { 
            return new NameOptions(name);
        }
        public INameOptions NameOptions(IEnumerable<string> names)
        {
            return new NameOptions(names);
        }
        public IOrderbyOptions OrderbyOptions(string field, bool isAscending = true)
        {
            return new OrderbyOptions(field, isAscending);
        }
        public IKeyValueOptions KeyValueOptions()
        {
            return new KeyValueOptions();
        }
        public IKeyValueOptions KeyValueOptions(IDictionary<string,object> dict)
        {
            return new KeyValueOptions(dict);
        }
        public IKeyValueOptions KeyValueOptions(string key, object value)
        {
            return new KeyValueOptions().Add(key, value);
        }
        public void UseDbCommand(DbCommandHandler callback)
        {
            UseDbConnection(conn =>
            {
                using (var cmd = conn.CreateCommand())
                {
                    callback(cmd);
                }
            });
        }
        public void UseDbTransaction(DbTransactionHandler callback)
        {
            UseDbConnection(conn =>
            {
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        callback(tran);
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            });
        }
        public void UseDbCommandWithTransaction(DbCommandWithTransactionHandler callback)
        {
            UseDbConnection(conn =>
            {
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        bool isOK = false;
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            isOK = callback(cmd);
                        }
                        if (isOK)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            });
        }
        #endregion

        #region -- abstract method --
        public abstract void SetDbDataTypeMapping(string dbDataType, Type clrType);
        public abstract string GetReturnIdentityColumnSqlPart(string returnIdentityColumnName);
        public abstract IDbDataParameter CreateParameter(string name, object value);
        public abstract IDbDataParameter CreateParameter(int index, object value);
        public abstract ITable GetTable(string tableName);
        public abstract List<ITable> GetTables();
        public abstract void UseDbConnection(DbConnectionHandler callback);
        public abstract string Quote(string name);
        public abstract string Param(string name);
        public abstract string Param(int index);
        #endregion

        #region -- virtual method --
        protected virtual ISql CompileSelectOptions<T>(ISelectOptions<T> opts)
        {
            if (opts == null)
            {
                throw new ArgumentNullException(nameof(opts));
            }
            if (string.IsNullOrEmpty(opts.TableName))
            {
                throw new InvalidOperationException($"{nameof(opts.TableName)} is null or empty");
            }
            string fieldsPart = opts.Fields != null && opts.Fields.Count > 0 ? string.Join(",", opts.Fields.Select(f => Quote(f))) : "*";
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(fieldsPart);
            sql.Append(" FROM ");
            sql.Append(Quote(opts.TableName));
            if (!string.IsNullOrEmpty(opts.Where))
            {
                sql.Append(" WHERE ");
                sql.Append(opts.Where);
            }
            string orderbyPart = opts.Orderby != null && opts.Orderby.Count > 0 ? string.Join(",", opts.Orderby.Select(o => Quote(o.Field) + (o.IsAscending ? " ASC" : " DESC"))) : "";
            if (!string.IsNullOrEmpty(orderbyPart))
            {
                sql.Append(" ORDER BY ");
                sql.Append(orderbyPart);
            }
            if (opts.Top > 0)
            {
                sql.Append(" LIMIT ");
                sql.Append(opts.Top);
            }
            return new Sql { Text = sql.ToString(), Params = opts.WhereParams };
        }
        
        public virtual List<T> Select<T>(ISelectOptions<T> opts)
        {
            var list = new List<T>();
            var sql = CompileSelectOptions(opts);
            UseDbCommand((cmd) => {
                cmd.CommandText = sql.Text;
                if (sql.Params != null)
                {
                    for (int i = 0; i < sql.Params.Length; i++)
                    {
                        cmd.Parameters.Add(CreateParameter(i, sql.Params[i]));
                    }
                }
                using (var reader = cmd.ExecuteReader())
                {
                    DataReaderWrapper dr = new DataReaderWrapper(reader);
                    while (reader.Read())
                    {
                        list.Add(opts.Converter(dr));
                    }
                }
            });
            return list;
        }
        public virtual void SelectEach<T>(ISelectOptions<T> opts, Action<T> callback)
        {
            if (opts == null || callback == null) { return; }
            var sql = CompileSelectOptions(opts);
            if (opts.Converter == null)
            {
                throw new ArgumentNullException(nameof(opts.Converter));
            }
            UseDbCommand((cmd) => {
                cmd.CommandText = sql.Text;
                if (sql.Params != null)
                {
                    for (int i = 0; i < sql.Params.Length; i++)
                    {
                        cmd.Parameters.Add(CreateParameter(i, sql.Params[i]));
                    }
                }
                using (var reader = cmd.ExecuteReader())
                {
                    DataReaderWrapper dr = new DataReaderWrapper(reader);
                    while (reader.Read())
                    {
                        callback(opts.Converter(dr));
                    }
                }
            });
        }
        /// <summary>
        /// Paginated query (based on LIMIT+OFFSET syntax, if your database doesn't support this query method, please implement it yourself)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="opts"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual IPageResult<T> Page<T>(IPageOptions<T> opts)
        {
            PageResult<T> reval = new PageResult<T> { Rows = new List<T>() };
            if (opts == null) { return reval; }
            if (string.IsNullOrWhiteSpace(opts.TableName))
            {
                throw new ArgumentNullException(nameof(opts.TableName));
            }
            if (opts.Converter == null)
            {
                throw new ArgumentNullException(nameof (opts.Converter));
            }
            int page=opts.Page;
            int size = opts.Size;
            page = page < 1 ? 1 : page;
            size = size < 1 ? 1 : size;

            reval.Page = page;
            reval.Size = size;

            string tableName = Quote(opts.TableName);

            long startIndex = page * size - size;
            long endIndex = page * size;

            string fieldsPart = opts.Fields != null && opts.Fields.Count > 0 ? string.Join(",", opts.Fields.Select(f => Quote(f))) : "*";
            string orderbyPart = opts.Orderby != null && opts.Orderby.Count>0 ? string.Join(",", opts.Orderby.Select(o => Quote(o.Field) + (o.IsAscending ? " ASC" : " DESC"))) : "";
            if (string.IsNullOrEmpty(orderbyPart))
            {
                orderbyPart = "(SELECT NULL)";
            }
            string where = opts.Where;
            if (string.IsNullOrEmpty(where))
            {
                where = "1>0";
            }
            string uniqueField = opts.UniqueField;
            StringBuilder sql = new StringBuilder();
            if (!string.IsNullOrEmpty(uniqueField))//具有唯一性字段
            {
                uniqueField = Quote(uniqueField);
                sql.AppendFormat("SELECT {0} FROM {1}", fieldsPart, tableName);
                sql.AppendFormat(" WHERE {0} IN(", uniqueField);
                sql.AppendFormat("SELECT {0} FROM {1}", uniqueField, tableName);
                sql.AppendFormat(" WHERE {0}", where);
                sql.AppendFormat(" ORDER BY {0}", orderbyPart);
                sql.AppendFormat(" LIMIT {0} OFFSET {1}", size, startIndex);
                sql.AppendFormat(") ORDER BY {0}", orderbyPart);

            }
            else//没有唯一性字段
            {
                sql.AppendFormat("SELECT {0} FROM {1}", fieldsPart, tableName);
                sql.AppendFormat(" WHERE {0}", where);
                sql.AppendFormat(" ORDER BY {0}", orderbyPart);
                sql.AppendFormat(" LIMIT {0} OFFSET {1}", size, startIndex);
            }
            UseDbCommand((cmd) =>
            {
                cmd.CommandText = string.Format("SELECT COUNT(1) FROM {0} WHERE {1}", tableName, where);
                if (opts.WhereParams != null)
                {
                    for (int i = 0; i < opts.WhereParams.Length; i++)
                    {
                        cmd.Parameters.Add(CreateParameter(i, opts.WhereParams[i]));
                    }
                }
                object? obj = cmd.ExecuteScalar();
                long total = 0;
                if (obj != null && obj != DBNull.Value)
                {
                    total = Convert.ToInt64(obj);
                }
                reval.Total = total;
                long pages = total % size == 0 ? total / size : (total / size + 1);
                if (page > pages)
                {
                    return;
                }
                cmd.CommandText = sql.ToString();
                using (var reader = cmd.ExecuteReader())
                {
                    DataReaderWrapper dr = new DataReaderWrapper(reader);
                    while (reader.Read())
                    {
                        reval.Rows.Add(opts.Converter(dr));
                    }
                }
            });
            return reval;
        }
        public virtual int Update(string tableName, IKeyValueOptions kvs, IWhereOptions where)
        {
            var sql = where.Compile();
            return Update(tableName, kvs, sql.Text, sql.Params);
        }
        public virtual int Update(string tableName, IKeyValueOptions kvs, string where, params object[]? whereParams)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            { throw new ArgumentNullException(nameof(tableName)); }
            if (kvs == null || kvs.Count == 0) { return 0; }
            List<object> mixValues = new List<object>();
            List<string> setParts = new List<string>();
            if (whereParams != null && whereParams.Length > 0)
            {
                mixValues.AddRange(whereParams);
            }
            int index = mixValues.Count;
            foreach (var v in kvs)
            {
                string pName = Param(index);
                setParts.Add(string.Format("{0}={1}", Quote(v.Key), pName));
                mixValues.Add(v.Value);
                index++;
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE ");
            sql.Append(Quote(tableName));
            sql.Append(" SET ");
            sql.Append(string.Join(",", setParts));
            if (!string.IsNullOrWhiteSpace(where))
            {
                sql.Append(" WHERE ");
                sql.Append(where);
            }
            int rowsAffected = 0;
            UseDbCommand((cmd) =>
            {
                cmd.CommandText = sql.ToString();
                for (int i = 0; i < mixValues.Count; i++)
                {
                    cmd.Parameters.Add(CreateParameter(i, mixValues[i]));
                }
                rowsAffected = cmd.ExecuteNonQuery();
            });

            return rowsAffected;
        }
        public virtual int Update<T>(string tableName, IEnumerable<T> values, KeyValueFillHandler<T> convert, INameOptions setFields, INameOptions keyFields)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            { throw new ArgumentNullException(nameof(tableName)); }
            if (values == null) { return 0; }
            if (setFields == null || setFields.Count < 1)
            {
                throw new ArgumentNullException(nameof(setFields));
            }
            if (keyFields == null || keyFields.Count < 1)
            {
                throw new ArgumentNullException(nameof(keyFields));
            }
            if (setFields.ContainsAny(keyFields))
            {
                throw new ArgumentException("参数\"setFields\"和\"keyFields\"存在重复项");
            }

            string[] paramKeys = new string[setFields.Count + keyFields.Count];

            List<string> sets = new List<string>();
            List<string> keys =new List<string>();
            int index = 0;
            foreach(string field in setFields)
            {
                sets.Add(string.Format("{0}={1}", Quote(field), Param(index)));
                paramKeys[index] = field;
                index++;
            }
            foreach (string field in keyFields)
            {
                keys.Add(string.Format("{0}={1}", Quote(field), Param(index)));
                paramKeys[index] = field;
                index++;
            }
            
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", Quote(tableName), string.Join(",",sets), string.Join(" AND ",keys));
            int rowsAffected = 0;
            UseDbCommandWithTransaction(cmd =>
            {
                try
                {
                    KeyValueOptions pairs = new KeyValueOptions();
                    cmd.CommandText = sql;
                    foreach (T t in values)
                    {
                        pairs.Clear();
                        cmd.Parameters.Clear();

                        convert(t, pairs);

                        if (paramKeys.Length > pairs.Count) { throw new ArgumentException($"生成的更新字典元素个数太少"); }
                        for (int i = 0; i < paramKeys.Length; i++)
                        {
                            object keyValue;
                            if (!pairs.TryGetValue(paramKeys[i], out keyValue))
                            {
                                throw new ArgumentException($"生成的更新字典里必须包含\"{paramKeys[i]}\"的键值对");
                            }
                            cmd.Parameters.Add(CreateParameter(i, keyValue));
                        }
                        rowsAffected += cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception)
                {
                    rowsAffected = 0;
                    throw;
                }
            });

            return rowsAffected;
        }
        public virtual int Delete(string tableName, IWhereOptions where)
        {
            var sql = where.Compile();
            return Delete(tableName, sql.Text, sql.Params);
        }
        public virtual int Delete(string tableName, string where, params object[]? whereParams)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE FROM ");
            sql.Append(Quote(tableName));
            if (!string.IsNullOrEmpty(where))
            {
                sql.Append(" WHERE ");
                sql.Append(where);
            }
            int rowsAffected = 0;
            UseDbCommand((cmd) =>
            {
                cmd.CommandText = sql.ToString();
                if (whereParams != null)
                {
                    for (int i = 0; i < whereParams.Length; i++)
                    {
                        cmd.Parameters.Add(CreateParameter(i, whereParams[i]));
                    }
                }
                rowsAffected = cmd.ExecuteNonQuery();
            });

            return rowsAffected;
        }
        public virtual int Insert(string tableName, IKeyValueOptions kvs, INameOptions? ignoreFields = null)
        {
            if (string.IsNullOrEmpty(tableName)) { throw new ArgumentNullException(nameof(tableName)); }
            if (kvs == null || kvs.Count < 1) { throw new ArgumentNullException(nameof(kvs)); }
            bool ignoreFieldsNotNull = ignoreFields != null && ignoreFields.Count > 0;

            string format = string.Format("INSERT INTO {0}({{0}}) VALUES({{1}})", Quote(tableName));

            int rowsAffected = 0;
            UseDbCommand(cmd =>
            {
                try
                {
                    List<string> fields = new List<string>();
                    List<string> placeholders = new List<string>();
                    int index = 0;
                    foreach (var item in kvs)
                    {
                        if (ignoreFieldsNotNull && ignoreFields.Contains(item.Key)) { continue; }
                        fields.Add(Quote(item.Key));
                        placeholders.Add(Param(index));
                        cmd.Parameters.Add(CreateParameter(index, item.Value));
                        index++;
                    }
                    if (index < 1) { return; }
                    cmd.CommandText = string.Format(format, string.Join(",", fields), string.Join(",", placeholders));
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    rowsAffected = 0;
                    throw;
                }
            });

            return rowsAffected;
        }
        public virtual int Insert<T>(string tableName, T value, KeyValueFillHandler<T> converter, INameOptions? ignoreFields = null)
        {
            if (string.IsNullOrEmpty(tableName)) { throw new ArgumentNullException(nameof(tableName)); }
            if (converter == null || value == null) { return 0; }
            bool ignoreFieldsNotNull = ignoreFields != null && ignoreFields.Count > 0;

            string format = string.Format("INSERT INTO {0}({{0}}) VALUES({{1}})", Quote(tableName));

            int rowsAffected = 0;
            UseDbCommand(cmd =>
            {
                try
                {
                    List<string> fields = new List<string>();
                    List<string> placeholders = new List<string>();
                    KeyValueOptions pairs = new KeyValueOptions();

                    converter(value, pairs);

                    int index = 0;
                    foreach (var item in pairs)
                    {
                        if (ignoreFieldsNotNull && ignoreFields.Contains(item.Key)) { continue; }
                        fields.Add(Quote(item.Key));
                        placeholders.Add(Param(index));
                        cmd.Parameters.Add(CreateParameter(index, item.Value));
                        index++;
                    }
                    if (index < 1) { return; }

                    cmd.CommandText = string.Format(format, string.Join(",", fields), string.Join(",", placeholders));

                    rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    rowsAffected = 0;
                    throw;
                }
            });

            return rowsAffected;
        }
        public virtual int Insert<T>(string tableName, IEnumerable<T> values, KeyValueFillHandler<T> converter, INameOptions? ignoreFields = null)
        {
            if (string.IsNullOrEmpty(tableName)) { throw new ArgumentNullException(nameof(tableName)); }
            if (values == null || converter == null) { return 0; }
            string format = string.Format("INSERT INTO {0}({{0}}) VALUES({{1}})", Quote(tableName));
            bool ignoreFieldsNotNull = ignoreFields != null && ignoreFields.Count > 0;

            int rowsAffected = 0;
            UseDbCommandWithTransaction(cmd =>
            {
                try
                {
                    List<string> fields = new List<string>();
                    List<string> placeholders = new List<string>();
                    KeyValueOptions pairs = new KeyValueOptions();
                    foreach (T t in values)
                    {
                        fields.Clear();
                        placeholders.Clear();
                        cmd.Parameters.Clear();
                        pairs.Clear();

                        converter(t, pairs);

                        int index = 0;
                        foreach (var item in pairs)
                        {
                            if (ignoreFieldsNotNull && ignoreFields.Contains(item.Key)) { continue; }
                            fields.Add(Quote(item.Key));
                            placeholders.Add(Param(index));
                            cmd.Parameters.Add(CreateParameter(index, item.Value));
                            index++;
                        }
                        if (index < 1) { continue; }
                        cmd.CommandText = string.Format(format, string.Join(",", fields), string.Join(",", placeholders));
                        rowsAffected += cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception)
                {
                    rowsAffected = 0;
                    throw;
                }
            });

            return rowsAffected;
        }
        public virtual List<T> ExecuteQuery<T>(IRawSqlOptions opts, DataReaderFillEntityHandler<T> converter)
        {
            List<T> rs = new List<T>();
            if (opts == null || string.IsNullOrWhiteSpace(opts.Text) || converter == null) { return rs; }
            UseDbCommand((cmd) =>
            {
                cmd.CommandText = opts.Text;
                cmd.CommandType = opts.CmdType;
                if (opts.Params != null)
                {
                    for (int i = 0; i < opts.Params.Length; i++)
                    {
                        cmd.Parameters.Add(CreateParameter(i, opts.Params[i]));
                    }
                }
                using (var reader = cmd.ExecuteReader())
                {
                    DataReaderWrapper dr = new(reader);
                    while (reader.Read())
                    {
                        rs.Add(converter(dr));
                    }
                }
            });
            return rs;
        }
        public virtual void ExecuteQueryEach<T>(IRawSqlOptions opts, DataReaderFillEntityHandler<T> converter, Action<T> callback)
        {
            if (opts == null || string.IsNullOrWhiteSpace(opts.Text) || converter == null || callback == null) { return; }
            UseDbCommand((cmd) =>
            {
                cmd.CommandText = opts.Text;
                cmd.CommandType = opts.CmdType;
                if (opts.Params != null)
                {
                    for (int i = 0; i < opts.Params.Length; i++)
                    {
                        cmd.Parameters.Add(CreateParameter(i, opts.Params[i]));
                    }
                }
                using (var reader = cmd.ExecuteReader())
                {
                    DataReaderWrapper dr = new(reader);
                    while (reader.Read())
                    {
                        callback(converter(dr));
                    }
                }
            });
        }
        public virtual int ExecuteNonQuery(IRawSqlOptions opts)
        {
            if (opts == null || string.IsNullOrWhiteSpace(opts.Text)) { return 0; }
            int rowsAffected = 0;
            UseDbCommand((cmd) =>
            {
                cmd.CommandText = opts.Text;
                cmd.CommandType = opts.CmdType;
                if (opts.Params != null)
                {
                    for (int i = 0; i < opts.Params.Length; i++)
                    {
                        cmd.Parameters.Add(CreateParameter(i, opts.Params[i]));
                    }
                }
                rowsAffected = cmd.ExecuteNonQuery();
            });
            return rowsAffected;
        }
        public virtual object? ExecuteScalar(IRawSqlOptions opts)
        {
            if (opts == null || string.IsNullOrWhiteSpace(opts.Text)) { return null; }
            object? obj = null;
            UseDbCommand((cmd) =>
            {
                cmd.CommandText = opts.Text;
                cmd.CommandType = opts.CmdType;
                if (opts.Params != null)
                {
                    for (int i = 0; i < opts.Params.Length; i++)
                    {
                        cmd.Parameters.Add(CreateParameter(i, opts.Params[i]));
                    }
                }
                obj = cmd.ExecuteScalar();
                if (obj == DBNull.Value)
                {
                    obj = null;
                }
            });
            return obj;
        }
        public IExecuteResult Execute(IRawSqlOptions opts)
        {
            if (opts == null || string.IsNullOrWhiteSpace(opts.Text)) { throw new ArgumentNullException(nameof(opts)); }
            var tables = new List<DataTable>();
            int recordsAffected = 0;
            UseDbCommand((cmd) =>
            {
                cmd.CommandText = opts.Text;
                cmd.CommandType = opts.CmdType;
                if (opts.Params != null)
                {
                    for (int i = 0; i < opts.Params.Length; i++)
                    {
                        cmd.Parameters.Add(CreateParameter(i, opts.Params[i]));
                    }
                }
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    do
                    {
                        var table = new DataTable();
                        table.Load(reader);
                        tables.Add(table);
                    }
                    while (reader.NextResult());
                    recordsAffected = reader.RecordsAffected;
                }
            });
            return new ExecuteResult { Tables = tables, RecordsAffected = recordsAffected };
        }
        #endregion
    }
}
