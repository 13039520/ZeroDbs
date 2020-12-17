using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbOperator
    {
        T Get<T>(object key) where T : class, new();
        List<T> Select<T>(string where) where T : class, new();
        List<T> Select<T>(string where, string orderby) where T : class, new();
        List<T> Select<T>(string where, string orderby, int top) where T : class, new();
        List<T> Select<T>(string where, string orderby, int top, int threshold) where T : class, new();
        List<T> Select<T>(string where, string orderby, int top, string[] fieldNames) where T : class, new();

        Common.PageData<T> Page<T>(long page, long size,string where) where T : class, new();
        Common.PageData<T> Page<T>(long page, long size,string where, string orderby) where T : class, new();
        Common.PageData<T> Page<T>(long page, long size,string where, string orderby, int threshold) where T : class, new();
        Common.PageData<T> Page<T>(long page, long size,string where, string orderby, string[] fieldNames) where T : class, new();
        Common.PageData<T> Page<T>(long page, long size,string where, string orderby, int threshold, string uniqueFieldName) where T : class, new();
        Common.PageData<T> Page<T>(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName) where T : class, new();

        long Count<T>(string where) where T : class, new();

        int Insert<T>(T entity) where T : class, new();
        int Insert<T>(List<T> entityList) where T : class, new();
        int Insert<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new();
        int Insert<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new();

        int Update<T>(T entity) where T : class, new();
        int Update<T>(List<T> entityList) where T : class, new();
        int Update<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new();
        int Update<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new();

        int Delete<T>(T entity) where T : class, new();
        int Delete<T>(List<T> entityList) where T : class, new();
        int Delete<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new();
        int Delete<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new();
    }
}
