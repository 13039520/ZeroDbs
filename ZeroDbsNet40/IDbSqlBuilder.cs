using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbSqlBuilder
    {
        IDb ZeroDb { get; }
        string Page<T>(long page, long size, string where, string orderby, string[] returnFieldNames, string uniqueFieldName = "") where T : class, new();
        string Page<T>(long page, long size, string where, string orderby, int lengthThreshold, string uniqueFieldName = "") where T : class, new();
        string Insert<T>(T sourceEntity, string[] skipFieldNames) where T : class, new();
        List<string> Insert<T>(List<T> sourceEntityList, string[] skipFieldNames) where T : class, new();
        string Insert<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new();
        string Insert<T>(System.Collections.Specialized.NameValueCollection nvc, string appendWhere) where T : class, new();
        List<string> Insert<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new();
        List<string> Insert<T>(List<System.Collections.Specialized.NameValueCollection> nvcList, string appendWhere) where T : class, new();
        string Update<T>(T sourceEntity, string[] setFieldNames, string[] whereFieldNames) where T : class, new();
        List<string> Update<T>(List<T> sourceEntityList, string[] setFieldNames, string[] whereFieldNames) where T : class, new();
        string Update<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new();
        string Update<T>(System.Collections.Specialized.NameValueCollection nvc, string appendWhere) where T : class, new();
        List<string> Update<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new();
        List<string> Update<T>(List<System.Collections.Specialized.NameValueCollection> nvcList, string appendWhere) where T : class, new();
        string Delete<T>(string sqlWhere) where T : class, new();
        string Delete<T>(T sourceEntity, string[] useFiled) where T : class, new();
        List<string> Delete<T>(List<T> sourceEntityList, string[] useFiled) where T : class, new();
        string Delete<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new();
        string Delete<T>(System.Collections.Specialized.NameValueCollection nvc, string appendWhere) where T : class, new();
        List<string> Delete<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new();
        List<string> Delete<T>(List<System.Collections.Specialized.NameValueCollection> nvcList, string appendWhere) where T : class, new();
        string Select<T>(T sourceEntity, string[] whereField, string orderby) where T : class, new();
        string Select<T>(string where, string orderby) where T : class, new();
        string Select<T>(string where, string orderby, string[] returnFieldNames) where T : class, new();
        string Select<T>(string where, string orderby, int top) where T : class, new();
        string Select<T>(string where, string orderby, int top, string[] returnFieldNames) where T : class, new();
        string Select<T>(string where, string orderby, int top, int lengthThreshold) where T : class, new();
        string Select<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new();
        string Select<T>(System.Collections.Specialized.NameValueCollection nvc, string appendWhere) where T : class, new();
        List<string> Select<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new();
        List<string> Select<T>(List<System.Collections.Specialized.NameValueCollection> nvcList, string appendWhere) where T : class, new();
        string Count<T>(string where) where T : class, new();
        string SelectByKey<T>(object key) where T : class, new();
    }
}
