using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs
{
    public interface IDbDataTypeMaping
    {
        Type GetDotNetType(string sqlDbTypeName, long maxLength);
        string GetDotNetTypeFullName(string sqlDbTypeName, long maxLength);
        string GetDotNetDefaultValueText(string defaultVal, string dbDataTypeName, long maxLength);
    }
}
