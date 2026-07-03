using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// 原始SQL配置项
    /// </summary>
    internal class RawSqlOptions: IRawSqlOptions
    {
        public string Text { get; set; } =string.Empty;
        public System.Data.CommandType CmdType { get; set; } = System.Data.CommandType.Text;
        public object[]? Params { get; set; }

        public IRawSqlOptions SetCmdText(string cmdText, params object[]? cmdParams)
        {
            this.Text = cmdText;
            this.Params = cmdParams;
            return this;
        }
        public IRawSqlOptions SetCmdText(ISql sql)
        {
            this.Text = sql.Text;
            this.Params = sql.Params;
            return this;
        }
        public IRawSqlOptions SetCmdType(System.Data.CommandType commandType)
        {
            this.CmdType = commandType;
            return this;
        }
    }
}
