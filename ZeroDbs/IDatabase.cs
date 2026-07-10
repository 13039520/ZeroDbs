using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    public interface IDatabase
    {
        #region -- 数据库标识 --
        string DbKey { get; }
        string DbType { get; }
        #endregion

        #region -- 输入辅助 --
        ISnowflakeIdGenerator Snowflake {  get; }
        Guid SequentialGuid();
        IInsertOptions InsertOptions(string tableName);
        IDeleteOptions DeleteOptions(string tableName);
        IUpdateOptions UpdateOptions(string tableName);
        ISelectOptions<T> SelectOptions<T>(string tableName);
        IPageOptions<T> PageOptions<T>(string tableName);
        IInValueOptions InValueOptions<T>(params T[] values);
        IWherePartOptions WherePartOptions(string template, bool isAnd = true);
        IWhereOptions WhereOptions(params IWherePartOptions[] parts);
        ISqlOptions SqlOptions(string template);
        IRawSqlOptions RawSqlOptions(ISqlOptions sqlOpts);
        IRawSqlOptions RawSqlOptions(ISql sql);
        IRawSqlOptions RawSqlOptions(string cmdText, params object[]? cmdParams);
        INameOptions NameOptions(string name);
        INameOptions NameOptions(IEnumerable<string> names);
        IOrderbyOptions OrderbyOptions(string field, bool isAscending = true);
        IKeyValueOptions KeyValueOptions();
        IKeyValueOptions KeyValueOptions(string key, object value);
        IKeyValueOptions KeyValueOptions(IDictionary<string, object> dictionary);
        #endregion

        #region -- 类型数据库专用 --
        void SetDbDataTypeMapping(string dbDataType, Type clrType);
        string Quote(string name);
        string Param(string name);
        string Param(int index);
        ITable GetTable(string tableName);
        List<ITable> GetTables();
        IDbDataParameter CreateParameter(string name, object value);
        IDbDataParameter CreateParameter(int index, object value);
        #endregion

        #region -- 基础操作入口 --
        void UseDbConnection(DbConnectionHandler callback);
        void UseDbTransaction(DbTransactionHandler callback);
        void UseDbCommand(DbCommandHandler callback);
        void UseDbCommandWithTransaction(DbCommandWithTransactionHandler callback);
        #endregion

        #region -- 常用方法 --
        List<T> Select<T>(ISelectOptions<T> opts);
        void SelectEach<T>(ISelectOptions<T> opts, Action<T> callback);
        IPageResult<T> Page<T>(IPageOptions<T> opts);
        int Update(string tableName, IKeyValueOptions kvs, IWhereOptions where);
        int Update(string tableName, IKeyValueOptions kvs, string where, params object[]? whereParams);
        int Update<T>(string tableName, IEnumerable<T> values, KeyValueFillHandler<T> converter, INameOptions setFields, INameOptions keyFields);
        int Delete(string tableName, IWhereOptions where);
        int Delete(string tableName, string where, params object[]? whereParams);
        int Insert(string tableName, IKeyValueOptions kvs, INameOptions? ignoreFields = null);
        int Insert<T>(string tableName, T value, KeyValueFillHandler<T> converter, INameOptions? ignoreFields = null);
        int Insert<T>(string tableName, IEnumerable<T> values, KeyValueFillHandler<T> converter, INameOptions? ignoreFields = null);
        List<T> ExecuteQuery<T>(IRawSqlOptions opts, DataReaderFillEntityHandler<T> converter);
        void ExecuteQueryEach<T>(IRawSqlOptions opts, DataReaderFillEntityHandler<T> converter, Action<T> callback);
        int ExecuteNonQuery(IRawSqlOptions opts);
        object? ExecuteScalar(IRawSqlOptions opts);
        /// <summary>
        /// 通用SQL执行方法
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        IExecuteResult Execute(IRawSqlOptions opts);
        #endregion
    }
}
