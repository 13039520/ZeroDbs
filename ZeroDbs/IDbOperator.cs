using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbOperator
    {
        List<OutType> Select<DbEntity, OutType>(Common.ListQuery query) where DbEntity : class, new() where OutType : class, new();
        List<DbEntity> Select<DbEntity>(Common.ListQuery query) where DbEntity : class, new();
        List<OutType> Select<DbEntity, OutType>() where DbEntity : class, new() where OutType : class, new();
        List<OutType> Select<DbEntity, OutType>(string where) where DbEntity : class, new() where OutType : class, new();
        List<OutType> Select<DbEntity, OutType>(string where, string orderby) where DbEntity : class, new() where OutType : class, new();
        List<OutType> Select<DbEntity, OutType>(string where, string orderby, int top) where DbEntity : class, new() where OutType : class, new();
        List<OutType> Select<DbEntity, OutType>(string where, string orderby, int top, params object[] paras) where DbEntity : class, new() where OutType : class, new();
        List<DbEntity> Select<DbEntity>() where DbEntity : class, new();
        List<DbEntity> Select<DbEntity>(string where) where DbEntity : class, new();
        List<DbEntity> Select<DbEntity>(string where, string orderby) where DbEntity : class, new();
        List<DbEntity> Select<DbEntity>(string where, string orderby, int top) where DbEntity : class, new();
        List<DbEntity> Select<DbEntity>(string where, string orderby, int top, string[] fields) where DbEntity : class, new();
        List<DbEntity> Select<DbEntity>(string where, string orderby, int top, string[] fields, params object[] paras) where DbEntity : class, new();
        DbEntity SelectByPrimaryKey<DbEntity>(object key) where DbEntity : class, new();

        Common.PageData<DbEntity> Page<DbEntity>(Common.PageQuery query) where DbEntity : class, new();
        Common.PageData<OutType> Page<DbEntity, OutType>(Common.PageQuery query) where DbEntity : class, new() where OutType : class, new();
        Common.PageData<DbEntity> Page<DbEntity>(long page, long size) where DbEntity : class, new();
        Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where) where DbEntity : class, new();
        Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby) where DbEntity : class, new();
        Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fields) where DbEntity : class, new();
        Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField) where DbEntity : class, new();
        Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField, params object[] paras) where DbEntity : class, new();
        Common.PageData<OutType> Page<DbEntity, OutType>(long page, long size) where DbEntity : class, new() where OutType : class, new();
        Common.PageData<OutType> Page<DbEntity, OutType>(long page, long size, string where) where DbEntity : class, new() where OutType : class, new();
        Common.PageData<OutType> Page<DbEntity, OutType>(long page, long size, string where, string orderby) where DbEntity : class, new() where OutType : class, new();
        Common.PageData<OutType> Page<DbEntity, OutType>(long page, long size, string where, string orderby, string uniqueField) where DbEntity : class, new() where OutType : class, new();
        Common.PageData<OutType> Page<DbEntity, OutType>(long page, long size, string where, string orderby, string uniqueField, params object[] paras) where DbEntity : class, new() where OutType : class, new();

        long Count<DbEntity>(string where, params object[] paras) where DbEntity : class, new();
        long Count(Type entityType, string where, params object[] paras);
        long MaxIdentityPrimaryKeyValue<DbEntity>() where DbEntity : class, new();
        long MaxIdentityPrimaryKeyValue<DbEntity>(string where, params object[] paras) where DbEntity : class, new();
        long MaxIdentityPrimaryKeyValue(Type entityType);
        long MaxIdentityPrimaryKeyValue(Type entityType, string where, params object[] paras);

        int Insert<DbEntity>(DbEntity entity) where DbEntity : class, new();
        int Insert<DbEntity>(List<DbEntity> entities) where DbEntity : class, new();
        int InsertByNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source) where DbEntity : class, new();
        int InsertByNameValueCollection(Type entityFullName, System.Collections.Specialized.NameValueCollection source);
        int InsertByCustomEntity<DbEntity>(object source) where DbEntity : class, new();
        int InsertByCustomEntity(Type entityFullName, object source);
        int InsertByDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new();
        int InsertByDictionary(Type entityFullName, Dictionary<string, object> source);

        int Update<DbEntity>(DbEntity entity) where DbEntity : class, new();
        int Update<DbEntity>(DbEntity entity, string appendWhere, params object[] paras) where DbEntity : class, new();
        int Update<DbEntity>(List<DbEntity> entities) where DbEntity : class, new();
        int Update<DbEntity>(List<DbEntity> entities, string appendWhere, params object[] paras) where DbEntity : class, new();
        int UpdateByNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source) where DbEntity : class, new();
        int UpdateByNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source, string appendWhere, params object[] paras) where DbEntity : class, new();
        int UpdateByNameValueCollection(Type entityFullName, System.Collections.Specialized.NameValueCollection source, string appendWhere, params object[] paras);
        int UpdateByCustomEntity<DbEntity>(object source) where DbEntity : class, new();
        int UpdateByCustomEntity<DbEntity>(object source, string appendWhere, params object[] paras) where DbEntity : class, new();
        int UpdateByCustomEntity(Type entityFullName, object source, string appendWhere, params object[] paras);
        int UpdateByDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new();
        int UpdateByDictionary<DbEntity>(Dictionary<string, object> source, string appendWhere, params object[] paras) where DbEntity : class, new();
        int UpdateByDictionary(Type entityFullName, Dictionary<string, object> source, string appendWhere, params object[] paras);

        int Delete<DbEntity>(string where, params object[] paras) where DbEntity : class, new();
        int Delete<DbEntity>(DbEntity source) where DbEntity : class, new();
        int Delete(Type entityType, string where, params object[] paras);
        int DeleteByPrimaryKey<DbEntity>(object key) where DbEntity : class, new();
        int DeleteByPrimaryKey(Type entityType, object key);
        int DeleteByCustomEntity<DbEntity>(object source) where DbEntity : class, new();
        int DeleteByCustomEntity(Type entityType, object source);
        int DeleteByDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new();
        int DeleteByDictionary(Type entityType, Dictionary<string, object> source);
        int DeleteByNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source) where DbEntity : class, new();
        int DeleteByNameValueCollection(Type entityType, System.Collections.Specialized.NameValueCollection source);

    }
}
