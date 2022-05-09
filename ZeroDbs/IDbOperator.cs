using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbOperator
    {
        List<OutType> Select<DbEntity, OutType>(Common.ListQuery query) where DbEntity : class, new() where OutType : class, new();
        List<DbEntity> Select<DbEntity>(Common.ListQuery query) where DbEntity : class, new();
        List<OutType> Select<DbEntity, OutType>(string where, string orderby, int top, params object[] paras) where DbEntity : class, new() where OutType : class, new();
        List<DbEntity> Select<DbEntity>(string where, string orderby, int top, string[] fields, params object[] paras) where DbEntity : class, new();

        Common.PageData<DbEntity> Page<DbEntity>(Common.PageQuery query) where DbEntity : class, new();
        Common.PageData<OutType> Page<DbEntity, OutType>(Common.PageQuery query) where DbEntity : class, new() where OutType : class, new();
        Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField, params object[] paras) where DbEntity : class, new();
        Common.PageData<OutType> Page<DbEntity, OutType>(long page, long size, string where, string orderby, string uniqueField, params object[] paras) where DbEntity : class, new() where OutType : class, new();

        long Count<DbEntity>(string where, params object[] paras) where DbEntity : class, new();
        long MaxIdentityPrimaryKeyValue<DbEntity>() where DbEntity : class, new();
        long MaxIdentityPrimaryKeyValue<DbEntity>(string where, params object[] paras) where DbEntity : class, new();

        int Insert<DbEntity>(DbEntity entity) where DbEntity : class, new();
        int Insert<DbEntity>(List<DbEntity> entities) where DbEntity : class, new();
        int InsertFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source) where DbEntity : class, new();
        int InsertFromCustomEntity<DbEntity>(object source) where DbEntity : class, new();
        int InsertFromDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new();

        int Update<DbEntity>(DbEntity entity) where DbEntity : class, new();
        int Update<DbEntity>(DbEntity entity, string appendWhere, params object[] paras) where DbEntity : class, new();
        int Update<DbEntity>(List<DbEntity> entities) where DbEntity : class, new();
        int Update<DbEntity>(List<DbEntity> entities, string appendWhere, params object[] paras) where DbEntity : class, new();
        int UpdateFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source) where DbEntity : class, new();
        int UpdateFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection source, string appendWhere, params object[] paras) where DbEntity : class, new();
        int UpdateFromCustomEntity<DbEntity>(object source) where DbEntity : class, new();
        int UpdateFromCustomEntity<DbEntity>(object source, string appendWhere, params object[] paras) where DbEntity : class, new();
        int UpdateFromDictionary<DbEntity>(Dictionary<string, object> source) where DbEntity : class, new();
        int UpdateFromDictionary<DbEntity>(Dictionary<string, object> source, string appendWhere, params object[] paras) where DbEntity : class, new();

        int Delete<DbEntity>(string where, params object[] paras) where DbEntity : class, new();

    }
}
