using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces.Common
{
    public class DbOperator:IDbOperator
    {
        ZeroDbs.Interfaces.IDbService _service = null;
        public DbOperator(ZeroDbs.Interfaces.IDbService service)
        {
            this._service = service;
        }
        ZeroDbs.Interfaces.IDb _GetZeroDb<T>() where T : class, new()
        {
            var reval = _service.GetDb<T>();
            if (reval == null)
            {
                throw new Exception("缺少数据库配置");
            }
            return reval;
        }
        public ZeroDbs.Interfaces.IDbCommand GetDbCommand<T>() where T : class, new()
        {
            return _GetZeroDb<T>().GetDbCommand<T>();
        }
        public ZeroDbs.Interfaces.IDbTransactionScope GetDbTransactionScope<T>(System.Data.IsolationLevel level, string identification = "", string groupId = "") where T : class, new()
        {
            return _GetZeroDb<T>().GetDbTransactionScope<T>(level, identification, groupId);
        }
        public ZeroDbs.Interfaces.IDbTransactionScopeCollection GetDbTransactionScopeCollection()
        {
            return new ZeroDbs.Interfaces.Common.DbTransactionScopeCollection();
        }
        public T Get<T>(object key) where T : class, new()
        {
            return _GetZeroDb<T>().Get<T>(key);
        }
        public List<T> Select<T>(string where) where T : class, new()
        {
            return _GetZeroDb<T>().Select<T>(where);
        }
        public List<T> Select<T>(string where, string orderby) where T : class, new()
        {
            return _GetZeroDb<T>().Select<T>(where, orderby);
        }
        public List<T> Select<T>(string where, string orderby, int top) where T : class, new()
        {
            return _GetZeroDb<T>().Select<T>(where, orderby, top);
        }
        public List<T> Select<T>(string where, string orderby, int top, int threshold) where T : class, new()
        {
            return _GetZeroDb<T>().Select<T>(where, orderby, top, threshold);
        }
        public List<T> Select<T>(string where, string orderby, int top, string[] fieldNames) where T : class, new()
        {
            return _GetZeroDb<T>().Select<T>(where, orderby, top, fieldNames);
        }

        public Common.PageData<T> Page<T>(long page, long size, string where) where T : class, new()
        {
            return _GetZeroDb<T>().Page<T>(page, size, where);
        }
        public Common.PageData<T> Page<T>(long page, long size, string where, string orderby) where T : class, new()
        {
            return _GetZeroDb<T>().Page<T>(page, size, where, orderby);
        }
        public Common.PageData<T> Page<T>(long page, long size, string where, string orderby, int threshold) where T : class, new()
        {
            return _GetZeroDb<T>().Page<T>(page, size, where, orderby, threshold);
        }
        public Common.PageData<T> Page<T>(long page, long size,string where, string orderby, string[] fieldNames) where T : class, new()
        {
            return _GetZeroDb<T>().Page<T>(page, size, where, orderby, fieldNames);
        }
        public Common.PageData<T> Page<T>(long page, long size, string where, string orderby, int threshold, string uniqueFieldName) where T : class, new()
        {
            return _GetZeroDb<T>().Page<T>(page, size, where, orderby, threshold, uniqueFieldName);
        }
        public Common.PageData<T> Page<T>(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName) where T : class, new()
        {
            return _GetZeroDb<T>().Page<T>(page, size, where, orderby, fieldNames, uniqueFieldName);
        }

        public long Count<T>(string where) where T : class, new()
        {
            return _GetZeroDb<T>().Count<T>(where);
        }

        public int Insert<T>(T entity) where T : class, new()
        {
            return _GetZeroDb<T>().Insert<T>(entity);
        }
        public int Insert<T>(List<T> entityList) where T : class, new()
        {
            return _GetZeroDb<T>().Insert<T>(entityList);
        }
        public int Insert<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new()
        {
            return _GetZeroDb<T>().Insert<T>(nvc);
        }
        public int Insert<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new()
        {
            return _GetZeroDb<T>().Insert<T>(nvcList);
        }

        public int Update<T>(T entity) where T : class, new()
        {
            return _GetZeroDb<T>().Update<T>(entity);
        }
        public int Update<T>(List<T> entityList) where T : class, new()
        {
            return _GetZeroDb<T>().Update<T>(entityList);
        }
        public int Update<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new()
        {
            return _GetZeroDb<T>().Update<T>(nvc);
        }
        public int Update<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new()
        {
            return _GetZeroDb<T>().Update<T>(nvcList);
        }

        public int Delete<T>(T entity) where T : class, new()
        {
            return _GetZeroDb<T>().Delete<T>(entity);
        }
        public int Delete<T>(List<T> entityList) where T : class, new()
        {
            return _GetZeroDb<T>().Delete<T>(entityList);
        }
        public int Delete<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new()
        {
            return _GetZeroDb<T>().Delete<T>(nvc);
        }
        public int Delete<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new()
        {
            return _GetZeroDb<T>().Delete<T>(nvcList);
        }
    }
}
