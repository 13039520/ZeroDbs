using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public static class SqlCheck
    {
        private static readonly string InsertSqlPattern = @"^INSERT INTO [a-zA-Z0-9\._\-]{3,50}\(\w+(,\w+)*\) VALUES\(.+\)";
        private static readonly string DeleteSqlPattern = @"^DELETE [a-zA-Z0-9\._\-]{3,50} WHERE(.+)";
        private static readonly string UpdateSqlPattern = @"^UPDATE [a-zA-Z0-9\._\-]{3,50} SET [a-zA-Z0-9_\-]{1,50}='?(.*?)'?(,[a-zA-Z0-9_\-]{1,50}='?(.*?)'?){0,500}( WHERE .+)?";
        private static readonly string SelectSqlPattern = @"^SELECT .+ \bFROM\b.+";


        private static bool IsBaseInsertSql(string InsertSqlBase, ref string Msg)
        {
            Msg = "INSERT命令检测不通过";
            if (System.Text.RegularExpressions.Regex.IsMatch(InsertSqlBase, InsertSqlPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                if (Step002_CheckChars(InsertSqlBase, ref Msg))
                {
                    if (Step003_CheckKeyword(InsertSqlBase, ref Msg))
                    {
                        Msg = "INSERT命令检测通过";
                        return true;
                    }
                }
            }
            return false;
        }
        private static bool IsBaseDeleteSql(string DeleteSqlBase, ref string Msg)
        {
            Msg = "DELETE命令检测不通过";
            if (System.Text.RegularExpressions.Regex.IsMatch(DeleteSqlBase, DeleteSqlPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                if (Step002_CheckChars(DeleteSqlBase, ref Msg))
                {
                    if (Step003_CheckKeyword(DeleteSqlBase, ref Msg))
                    {
                        Msg = "DELETE命令检测通过";
                        return true;
                    }
                }
            }
            return false;
        }
        private static bool IsBaseUpdateSql(string UpdateSqlBase, ref string Msg)
        {
            Msg = "UPDATE命令检测不通过";
            if (System.Text.RegularExpressions.Regex.IsMatch(UpdateSqlBase, UpdateSqlPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                if (Step002_CheckChars(UpdateSqlBase, ref Msg))
                {
                    if (Step003_CheckKeyword(UpdateSqlBase, ref Msg))
                    {
                        Msg = "UPDATE命令检测通过";
                        return true;
                    }
                }
            }
            return false;
        }
        private static bool IsBaseSelectSql(string SelectSqlBase, ref string Msg)
        {
            Msg = "SELECT命令检测不通过";
            if (System.Text.RegularExpressions.Regex.IsMatch(SelectSqlBase, SelectSqlPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                if (Step002_CheckChars(SelectSqlBase, ref Msg))
                {
                    if (Step003_CheckKeyword(SelectSqlBase, ref Msg))
                    {
                        Msg = "SELECT命令检测通过";
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool IsInsertSql(string Sql, ref string Msg)
        {
            return IsBaseInsertSql(Step001_RemoveCmdSqlIsStrVal(Sql), ref Msg);
        }
        public static bool IsInsertSql(List<string> SqlList, ref string Msg)
        {
            if (SqlList == null || SqlList.Count < 1)
            {
                Msg = "没有INSERT命令";
                return false;
            }
            bool Flag = true;
            int i = 0;
            while (i < SqlList.Count)
            {
                Flag = IsBaseInsertSql(Step001_RemoveCmdSqlIsStrVal(SqlList[i]), ref Msg);
                if (!Flag)
                {
                    Msg = "第" + (i + 1) + "条命令不是有效的INSERT命令";
                    break;
                }
                i++;
            }
            if (Flag)
            {
                Msg = "通过INSERT命令检测";
            }
            return Flag;
        }
        public static bool IsDeleteSql(string Sql, ref string Msg)
        {
            return IsBaseDeleteSql(Step001_RemoveCmdSqlIsStrVal(Sql), ref Msg);
        }
        public static bool IsDeleteSql(List<string> SqlList, ref string Msg)
        {
            if (SqlList == null || SqlList.Count < 1)
            {
                Msg = "没有DELETE命令";
                return false;
            }
            bool Flag = true;
            int i = 0;
            while (i < SqlList.Count)
            {
                Flag = IsBaseDeleteSql(Step001_RemoveCmdSqlIsStrVal(SqlList[i]), ref Msg);
                if (!Flag)
                {
                    Msg = "第" + (i + 1) + "条命令不是有效的DELETE命令";
                    break;
                }
                i++;
            }
            if (Flag)
            {
                Msg = "通过DELETE命令检测";
            }
            return Flag;
        }
        public static bool IsUpdateSql(string Sql, ref string Msg)
        {
            return IsBaseUpdateSql(Step001_RemoveCmdSqlIsStrVal(Sql), ref Msg);
        }
        public static bool IsUpdateSql(List<string> SqlList, ref string Msg)
        {
            if (SqlList == null || SqlList.Count < 1)
            {
                Msg = "没有UPDATE命令";
                return false;
            }
            bool Flag = true;
            int i = 0;
            while (i < SqlList.Count)
            {
                Flag = IsBaseUpdateSql(Step001_RemoveCmdSqlIsStrVal(SqlList[i]), ref Msg);
                if (!Flag)
                {
                    Msg = "第" + (i + 1) + "条命令不是有效的UPDATE命令";
                    break;
                }
                i++;
            }
            if (Flag)
            {
                Msg = "通过UPDATE命令检测";
            }
            return Flag;
        }
        public static bool IsSelectSql(string Sql, ref string Msg)
        {
            return IsBaseSelectSql(Step001_RemoveCmdSqlIsStrVal(Sql), ref Msg);
        }
        public static bool IsSelectSql(List<string> SqlList, ref string Msg)
        {
            if (SqlList == null || SqlList.Count < 1)
            {
                Msg = "没有SELECT命令";
                return false;
            }
            bool Flag = true;
            int i = 0;
            while (i < SqlList.Count)
            {
                Flag = IsBaseSelectSql(Step001_RemoveCmdSqlIsStrVal(SqlList[i]), ref Msg);
                if (!Flag)
                {
                    Msg = "第" + (i + 1) + "条命令不是有效的SELECT命令";
                    break;
                }
                i++;
            }
            if (Flag)
            {
                Msg = "通过SELECT命令检测";
            }
            return Flag;
        }
        public static bool IsTransSql(List<string> SqlList, ref string Msg)
        {
            Msg = "检测不通过";
            bool Flag = true;
            try
            {
                for (int i = 0; i < SqlList.Count; i++)
                {
                    string s = Step001_RemoveCmdSqlIsStrVal(SqlList[i]);
                    if (IsSelectSql(s, ref Msg))
                    {
                        throw new Exception("第" + (i + 1) + "条命令是SELECT命令，在连续的缺少其它逻辑介入的该事务场景中无意义");
                    }
                    if (
                        !IsDeleteSql(s, ref Msg) &&
                        !IsUpdateSql(s, ref Msg) &&
                        !IsInsertSql(s, ref Msg))
                    {
                        throw new Exception("第" + (i + 1) + "条命令不符合任何INSERT|DELETE|UPDATE命令模式之一");
                    }
                }
            }
            catch (Exception Ex)
            {
                Msg = Ex.Message;
                Flag = false;
            }
            return Flag;
        }
        public static bool IsTruncateSql(string Sql, ref string Msg)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(Sql.Trim().TrimEnd(';'), @"^TRUNCATE TABLE \w{2,100}(\.\w{2,100}){0,2}$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }
        public static bool IsDorpSql(string Sql, ref string Msg)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(Sql.Trim().TrimEnd(';'), @"^DORP (TABLE|VIEW) \w{2,100}(\.\w{2,100}){0,2}$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }


        private static string Step001_RemoveCmdSqlIsStrVal(string sql)
        {
            string s = sql.Trim().Replace("''", "_2douhao2_");
            s = System.Text.RegularExpressions.Regex.Replace(s, @"'[\s\S]*?'", "''");
            return s.Replace("_2douhao2_", "''").Trim().TrimEnd(';');
        }
        private static bool Step002_CheckChars(string sql, ref string Msg)
        {
            if (sql.IndexOf("--") > -1)
            {
                Msg = "注释符号导致检测不通过";
                return false;
            }
            if (sql.IndexOf(";") > -1)
            {
                Msg = "分号导致检测不通过";
                return false;
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(sql, @"(\r\n|\n)"))
            {
                Msg = "换行符导致检测不通过";
                return false;
            }
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(sql, @"'");
            if (mc.Count > 0)
            {
                if (mc.Count % 2 != 0)
                {
                    Msg = "单引号导致检测不通过";
                    return false;
                }
            }
            return true;
        }
        private static bool Step003_CheckKeyword(string sql, ref string Msg)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(sql, @"\b(Dorp|Create|Alter|Truncate|Exec|Execut)\b",System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                Msg = "敏感词导致检测不通过";
                return false;
            }
            Msg = "敏感词导致检测通过";
            return true;
        }

    }
}
