using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces
{
    public interface IDbSqlBuilder
    {
        IDb ZeroDb { get; }
        string BuildSqlPage<T>(long page, long size, string where, string orderby, string[] returnFieldNames, string uniqueFieldName = "") where T : class, new();
        string BuildSqlPage<T>(long page, long size, string where, string orderby, int lengthThreshold, string uniqueFieldName = "") where T : class, new();
        string BuildSqlInsert<T>(T sourceEntity, string[] skipFieldNames) where T : class, new();
        List<string> BuildSqlInsert<T>(List<T> sourceEntityList, string[] skipFieldNames) where T : class, new();
        string BuildSqlInsert<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new();
        string BuildSqlInsert<T>(System.Collections.Specialized.NameValueCollection nvc, string appendWhere) where T : class, new();
        List<string> BuildSqlInsert<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new();
        List<string> BuildSqlInsert<T>(List<System.Collections.Specialized.NameValueCollection> nvcList, string appendWhere) where T : class, new();
        string BuildSqlUpdate<T>(T sourceEntity, string[] setFieldNames, string[] whereFieldNames) where T : class, new();
        List<string> BuildSqlUpdate<T>(List<T> sourceEntityList, string[] setFieldNames, string[] whereFieldNames) where T : class, new();
        string BuildSqlUpdate<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new();
        string BuildSqlUpdate<T>(System.Collections.Specialized.NameValueCollection nvc, string appendWhere) where T : class, new();
        List<string> BuildSqlUpdate<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new();
        List<string> BuildSqlUpdate<T>(List<System.Collections.Specialized.NameValueCollection> nvcList, string appendWhere) where T : class, new();
        string BuildSqlDelete<T>(string sqlWhere) where T : class, new();
        string BuildSqlDelete<T>(T sourceEntity, string[] useFiled) where T : class, new();
        List<string> BuildSqlDelete<T>(List<T> sourceEntityList, string[] useFiled) where T : class, new();
        string BuildSqlDelete<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new();
        string BuildSqlDelete<T>(System.Collections.Specialized.NameValueCollection nvc, string appendWhere) where T : class, new();
        List<string> BuildSqlDelete<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new();
        List<string> BuildSqlDelete<T>(List<System.Collections.Specialized.NameValueCollection> nvcList, string appendWhere) where T : class, new();
        string BuildSqlSelect<T>(T sourceEntity, string[] whereField, string orderby) where T : class, new();
        string BuildSqlSelect<T>(string where, string orderby) where T : class, new();
        string BuildSqlSelect<T>(string where, string orderby, string[] returnFieldNames) where T : class, new();
        string BuildSqlSelect<T>(string where, string orderby, int top) where T : class, new();
        string BuildSqlSelect<T>(string where, string orderby, int top, string[] returnFieldNames) where T : class, new();
        string BuildSqlSelect<T>(string where, string orderby, int top, int lengthThreshold) where T : class, new();
        string BuildSqlSelect<T>(System.Collections.Specialized.NameValueCollection nvc) where T : class, new();
        string BuildSqlSelect<T>(System.Collections.Specialized.NameValueCollection nvc, string appendWhere) where T : class, new();
        List<string> BuildSqlSelect<T>(List<System.Collections.Specialized.NameValueCollection> nvcList) where T : class, new();
        List<string> BuildSqlSelect<T>(List<System.Collections.Specialized.NameValueCollection> nvcList, string appendWhere) where T : class, new();
        string BuildSqlCount<T>(string where) where T : class, new();
        string BuildSqlSelectByKey<T>(object key) where T : class, new();
    }
}
