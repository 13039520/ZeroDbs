using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbOperator
    {
        List<Target> Select<DbEntity, Target>(Common.ListQuery query) where DbEntity : class, new() where Target : class, new();
        List<DbEntity> Select<DbEntity>(Common.ListQuery query) where DbEntity : class, new();
        List<Target> Select<DbEntity, Target>(string where, string orderby, int top, params object[] paras) where DbEntity : class, new() where Target : class, new();
        List<DbEntity> Select<DbEntity>(string where, string orderby, int top, string[] fields, params object[] paras) where DbEntity : class, new();

        Common.PageData<DbEntity> Page<DbEntity>(Common.PageQuery query) where DbEntity : class, new();
        Common.PageData<Target> Page<DbEntity, Target>(Common.PageQuery query) where DbEntity : class, new() where Target : class, new();
        Common.PageData<DbEntity> Page<DbEntity>(long page, long size, string where, string orderby, string[] fields, string uniqueField, params object[] paras) where DbEntity : class, new();
        Common.PageData<Target> Page<DbEntity, Target>(long page, long size, string where, string orderby, string uniqueField, params object[] paras) where DbEntity : class, new() where Target : class, new();

        long Count<DbEntity>(string where, params object[] paras) where DbEntity : class, new();

        int Insert<DbEntity>(DbEntity entity) where DbEntity : class, new();
        int Insert<DbEntity>(List<DbEntity> entityList) where DbEntity : class, new();
        int Insert<DbEntity>(System.Collections.Specialized.NameValueCollection nvc) where DbEntity : class, new();
        int Insert<DbEntity>(List<System.Collections.Specialized.NameValueCollection> nvcList) where DbEntity : class, new();

        int Update<DbEntity>(DbEntity entity) where DbEntity : class, new();
        int Update<DbEntity>(List<DbEntity> entityList) where DbEntity : class, new();
        int UpdateFromNameValueCollection<DbEntity>(System.Collections.Specialized.NameValueCollection nvc) where DbEntity : class, new();
        int UpdateFromCustomEntity<DbEntity>(object source) where DbEntity : class, new();
        int UpdateFromDictionary<DbEntity>(Dictionary<string, object> dic) where DbEntity : class, new();

        int Delete<DbEntity>(string where, params object[] paras) where DbEntity : class, new();

    }
}
