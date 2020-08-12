using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces
{
    public interface IDataTypeMaping
    {
        string GetDotNetTypeString(int sqlDbTypeIntValue, long maxLength);
        string GetDotNetTypeString(string sqlDbTypeName, long maxLength);
        string GetDotNetDefaultValue(string defaultVal, string dbDataTypeName, long maxLength);
    }
}
