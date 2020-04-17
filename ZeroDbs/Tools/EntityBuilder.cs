using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Tools
{
    public static class EntityBuilder
    {
        static readonly string template = @"using System;
using System.Collections.Generic;
using System.Text;
namespace #NameSpace#
{
    /// <summary>
    /// #TableOrViewDescription#
    /// </summary>
    [Serializable]
    public partial class #TableOrViewClassName#
    {
        #region --标准字段--#ColumnLoopBegin#
        private #ColumnDotNetDataType# _#ColumnName##ColumnDotNetDefaultValue#;
        /// <summary>
        /// #ColumnDescription#
        /// </summary>
        public #ColumnDotNetDataType# #ColumnName#
        {
            get { return _#ColumnName#; }
            set { _#ColumnName# = value; }
        }#ColumnLoopEnd#
        #endregion

    }
}";
        public static void Builder(ZeroDbs.Interfaces.Common.DbConfigDatabaseInfo config, string entityProjectRootDir, string targetProjectRootDir, string entityProjectNameSpace, string entityTemplate = "")
        {
            if (config == null) { throw new Exception("config is null"); }
            List<ZeroDbs.Interfaces.Common.DbConfigDatabaseInfo> configList = new List<Interfaces.Common.DbConfigDatabaseInfo>(1);
            configList.Add(config);
            Builder(configList, entityProjectRootDir, targetProjectRootDir, entityProjectNameSpace, entityTemplate);
        }
        public static void Builder(List<ZeroDbs.Interfaces.Common.DbConfigDatabaseInfo> config, string entityProjectRootDir, string targetProjectRootDir, string entityProjectNameSpace, string entityTemplate = "")
        {
            var dbs = new DataAccess.DbSearcher(null).GetDbs(config);
            foreach (var key in dbs.Keys)
            {
                var db = dbs[key];
                var tables = db.GetDbDataTableInfoAll();
                Builder(tables, db.DbConfigDatabaseInfo, entityProjectRootDir, targetProjectRootDir, entityProjectNameSpace, entityTemplate);
            }
        }
        public static void Builder(List<ZeroDbs.Interfaces.Common.DbDataTableInfo> tables, ZeroDbs.Interfaces.Common.DbConfigDatabaseInfo dbInfo, string entityProjectRootDir, string targetProjectRootDir, string nameSpace, string entityTemplate = "")
        {

            string entityFileSaveDir = System.IO.Path.Combine(entityProjectRootDir, dbInfo.dbKey);
            string dbConfigFilePath = System.IO.Path.Combine(targetProjectRootDir, "ZeroDbConfig.xml");
            if (!System.IO.Directory.Exists(entityFileSaveDir))
            {
                System.IO.Directory.CreateDirectory(entityFileSaveDir);
            }
            if (!System.IO.Directory.Exists(targetProjectRootDir))
            {
                System.IO.Directory.CreateDirectory(targetProjectRootDir);
            }
            if (!nameSpace.EndsWith("." + dbInfo.dbKey))
            {
                nameSpace += "." + dbInfo.dbKey;
            }
            entityTemplate = string.IsNullOrEmpty(entityTemplate) ? template : entityTemplate;
            if (string.IsNullOrEmpty(entityTemplate))
            {
                throw new Exception("template Is Null Or Empty");
            }
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(
                entityTemplate,
                @"#ColumnLoopBegin#(?<temp>[\s\S]+)#ColumnLoopEnd#",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                throw new Exception("template 缺少 ColumnLoop 片段");
            }
            string ColumnLoopTemplate = match.Groups["temp"].Value;
            if (string.IsNullOrEmpty(ColumnLoopTemplate))
            {
                throw new Exception("template 的 ColumnLoop 片段为空");
            }

            #region -- ZeroDbConfig.xml --
            if (!System.IO.File.Exists(dbConfigFilePath))
            {
                string s = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + System.Environment.NewLine
                    + "<zero></zero>";
                System.IO.File.AppendAllText(dbConfigFilePath, s, Encoding.UTF8);
            }
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(dbConfigFilePath);
            System.Xml.XmlNode zeroNode = doc.SelectSingleNode(@"/zero");
            System.Xml.XmlNode nodeDbs = doc.SelectSingleNode(@"/zero/dbs");
            bool hasNodeDbs = nodeDbs != null;
            if (!hasNodeDbs)
            {
                nodeDbs = doc.CreateElement("dbs");
            }
            System.Xml.XmlNode nodeDvs = doc.SelectSingleNode(@"/zero/dvs");
            bool hasNodeDvs = nodeDvs != null;
            if (!hasNodeDvs)
            {
                nodeDvs = doc.CreateElement("dvs");
            }
            System.Xml.XmlNode nodeDb = doc.SelectSingleNode(@"/zero/dbs/db[@dbKey='" + dbInfo.dbKey + "']");
            if (nodeDb == null)
            {
                nodeDb = doc.CreateElement("db");
                System.Xml.XmlAttribute attribute = doc.CreateAttribute("dbKey");
                attribute.Value = dbInfo.dbKey;
                nodeDb.Attributes.Append(attribute);
                attribute = doc.CreateAttribute("dbConnectionString");
                attribute.Value = dbInfo.dbConnectionString;
                nodeDb.Attributes.Append(attribute);
                attribute = doc.CreateAttribute("dbType");
                attribute.Value = dbInfo.dbType;
                nodeDb.Attributes.Append(attribute);
                nodeDbs.AppendChild(nodeDb);
            }
            #endregion

            StringBuilder htmlLeft = new StringBuilder();
            StringBuilder htmlRight = new StringBuilder();
            int tableIndex = 0;
            int tableCount = 0;
            int viewCount = 0;

            #region -- tables foreach --
            foreach (var table in tables)
            {
                if (table.IsView)
                {
                    viewCount++;
                }
                else
                {
                    tableCount++;
                }
                string entityStr = entityTemplate;
                string tableOrViewDescription = table.Description;
                if (string.IsNullOrEmpty(tableOrViewDescription))
                {
                    if (table.IsView)
                    {
                        tableOrViewDescription = "VIEW:" + table.Name;
                    }
                    else
                    {
                        tableOrViewDescription = "TABLE:" + table.Name;
                    }
                }
                entityStr = entityStr.Replace("#NameSpace#", nameSpace);
                entityStr = entityStr.Replace("#TableOrViewDescription#", tableOrViewDescription);
                entityStr = entityStr.Replace("#TableOrViewName#", table.Name);
                string className = System.Text.RegularExpressions.Regex.Replace(table.Name, @"^(t|v|tb|view)_", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                className = FirstLetterToUpper(className);
                className = (table.IsView ? "v" : "t") + className;
                entityStr = entityStr.Replace("#TableOrViewClassName#", className);
                entityStr = entityStr.Replace("#DatabaseConnectionUseKey#", dbInfo.dbKey);

                htmlLeft.Append("<p" + (tableIndex < 1 ? " class=\"selected\"" : "") + ">" + nameSpace + "." + className + "<span>(" + tableOrViewDescription + ")</span></p>");
                htmlRight.Append("<div" + (tableIndex < 1 ? " class=\"selected\"" : "") + "><p>" + nameSpace + "." + className + "<span>(" + tableOrViewDescription + ")</span></p>");
                htmlRight.AppendFormat("<table><thead><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th><th>{4}</th><th>{5}</th><th>{6}</th><th>{7}</th><th>{8}</th><th>{9}</th></thead><tbody>",
                    "列名",
                    "数据类型",
                    "可空",
                    "主键",
                    "自增长",
                    "字节",
                    "长度",
                    "小数",
                    "默认值",
                    "描述"
                    );

                StringBuilder columnLoop = new StringBuilder();
                foreach (var column in table.Colunms)
                {
                    #region --循环列--
                    string ColumnLoopTemplateStr = "" + ColumnLoopTemplate;
                    string Description = column.Description;
                    string DefaultValue = column.DefaultValue;
                    string ColumnDotNetDataType = column.Type;
                    if (string.IsNullOrEmpty(Description))
                    {
                        Description = column.Name;
                    }
                    if (column.IsIdentity)
                    {
                        Description = "[自增]" + Description;
                    }
                    if (column.IsPrimaryKey)
                    {
                        Description = "[主键]" + Description;
                    }
                    if (string.IsNullOrEmpty(DefaultValue)) { DefaultValue = ""; }
                    if (DefaultValue.Length > 0)
                    {
                        DefaultValue = " = " + DefaultValue;
                    }
                    if (column.IsNullable)
                    {
                        if (ColumnDotNetDataType != "string" &&
                            ColumnDotNetDataType != "byte[]" &&
                            ColumnDotNetDataType != "object" &&
                            ColumnDotNetDataType != "object*" &&
                            ColumnDotNetDataType != "Xml")
                        {
                            ColumnDotNetDataType = ColumnDotNetDataType + "?";
                        }
                    }
                    else
                    {
                        if (ColumnDotNetDataType == "string")
                        {
                            if (DefaultValue.Length < 1)
                            {
                                DefaultValue = " = \"\"";
                            }
                        }
                    }

                    ColumnLoopTemplateStr = ColumnLoopTemplateStr.Replace("#ColumnDotNetDataType#", ColumnDotNetDataType);
                    ColumnLoopTemplateStr = ColumnLoopTemplateStr.Replace("#ColumnName#", FirstLetterToUpper(column.Name));
                    ColumnLoopTemplateStr = ColumnLoopTemplateStr.Replace("#ColumnDotNetDefaultValue#", DefaultValue);
                    ColumnLoopTemplateStr = ColumnLoopTemplateStr.Replace("#ColumnDescription#", Description);

                    columnLoop.Append(ColumnLoopTemplateStr);

                    htmlRight.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td></tr>",
                        column.Name,//"列名",
                        ColumnDotNetDataType,//"数据类型",
                        column.IsNullable,//"是否允许为空",
                        column.IsPrimaryKey,//"是否主键",
                        column.IsIdentity, //"是否自动增长",
                        column.Byte,//"占用字节",
                        column.MaxLength,//"长度限制",
                        column.DecimalDigits,//"小数位数",
                        DefaultValue,//"默认值",
                        Description//"描述"
                    );
                    #endregion
                }

                htmlRight.Append("</tbody><tfoot><tr>");
                htmlRight.AppendFormat("<td colspan=\"10\">共 {0} 个字段</td>", table.Colunms.Count);
                htmlRight.Append("</tr></tfoot></table></div>");

                entityStr = System.Text.RegularExpressions.Regex.Replace(
                    entityStr,
                    @"#ColumnLoopBegin#(?<temp>[\s\S]+)#ColumnLoopEnd#",
                    columnLoop.ToString());

                string filePath = System.IO.Path.Combine(entityFileSaveDir, className + ".cs");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                System.IO.File.AppendAllText(filePath, entityStr, Encoding.UTF8);

                #region -- ZeroDbConfig.xml: /zero/dvs/dv --
                string xpath = @"/zero/dvs/dv[@entityKey='" + nameSpace + "." + className + "' and @dbKey='" + dbInfo.dbKey + "']";
                System.Xml.XmlNode nodeDv = doc.SelectSingleNode(xpath);
                if (nodeDv == null)
                {
                    nodeDv = doc.CreateElement("dv");
                    System.Xml.XmlAttribute attribute = doc.CreateAttribute("dbKey");
                    attribute.Value = dbInfo.dbKey;
                    nodeDv.Attributes.Append(attribute);

                    attribute = doc.CreateAttribute("tableName");
                    attribute.Value = tables[tableIndex].Name;
                    nodeDv.Attributes.Append(attribute);

                    attribute = doc.CreateAttribute("entityKey");
                    attribute.Value = nameSpace + "." + className;
                    nodeDv.Attributes.Append(attribute);

                    nodeDvs.AppendChild(nodeDv);
                }
                #endregion

                tableIndex++;

            }
            #endregion

            #region -- html file --
            StringBuilder html = new StringBuilder();
            html.Append("<!DOCTYPE html><html><head><meta charset=\"utf-8\" />");
            html.Append("<title>" + dbInfo.dbKey + "表(视图)字段参考</title>");
            html.Append("<style type=\"text/css\">body,html{padding:0;margin:0;width:100%;height:100%;overflow:hidden;font-size:16px}#left{width:20%;height:100%;padding:0;margin:0;float:left;overflow:hidden;overflow-y:scroll;background:#f2f2f2}#right{width:80%;height:100%;padding:0;margin:0;float:left;overflow:hidden;overflow-y:scroll;background:#dedede}.box{margin:5px 5px 50px 5px;border:solid 1px #fff;background:#f8f8f8;padding:10px;font-size:14px;}.box p{margin:0;padding:5px;overflow:hidden;border-bottom:solid 1px #dedede;cursor:pointer;color:#666;word-break:break-all;word-wrap:break-word}.box p span{display:inline-block;font-size:12px;}.box p.selected,.box p:hover{text-align:right;color:#00f;background:#dedede}#right div{padding:5px;margin:5px 5px 50px 5px;overflow:hidden;border:solid 1px #dedede;display:none}#right p{padding:0;margin:0;overflow:hidden;font-weight:600;color:#00f}#right p span{display:inline-block;font-size:12px;}#right .selected{display:block}table{width:100%;border-spacing:0;border-collapse:collapse;background:#f7f7f7}table td,table th{padding:2px;border:solid 1px #dedede}table thead th{text-align:left;background:#333;border:solid 1px #666;white-space:nowrap;color:#ccc;height:20px;line-height:20px;font-size:14px}table tbody{color:#666;font-size:12px}table tbody tr:hover{background:#ff9;color:red}table tfoot{text-align:right}#bottom{width:100%;overflow:hidden;position:fixed;height:30px;line-height:30px;overflow:hidden;background:#fff;border-top:solid 2px #00f;bottom:0;left:0;color:#00f}</style>");
            html.Append("</head><body>");
            html.AppendFormat("<div id=\"left\"><div class=\"box\">{0}</div></div>", htmlLeft);
            html.AppendFormat("<div id=\"right\">{0}</div>", htmlRight);
            html.Append("<div id=\"bottom\">" + dbInfo.dbKey + "表(视图)字段参考，计" + tableCount + "个表" + viewCount + "个视图，命名空间" + nameSpace + "</div>");
            html.Append("<script type=\"text/javascript\">(function(){var ps=document.getElementById('left').getElementsByTagName('p');var divs=document.getElementById('right').getElementsByTagName('div');var click=function(index){for(var i=0;i<ps.length;i++){ps[i].className=divs[i].className=''}ps[index].className=divs[index].className='selected'};for(var i=0;i<ps.length;i++){ps[i].onclick=(function(n){return function(){click(n)}})(i)}})();</script>");
            html.Append("</body></html>");

            string htmlFilePath = System.IO.Path.Combine(entityProjectRootDir, dbInfo.dbKey + ".html");
            if (System.IO.File.Exists(htmlFilePath))
            {
                System.IO.File.Delete(htmlFilePath);
            }
            System.IO.File.AppendAllText(htmlFilePath, html.ToString(), Encoding.UTF8);
            #endregion

            if (!hasNodeDbs)
            {
                zeroNode.AppendChild(nodeDbs);
            }
            if (!hasNodeDvs)
            {
                zeroNode.AppendChild(nodeDvs);
            }
            doc.Save(dbConfigFilePath);

        }
        static string FirstLetterToUpper(string str)
        {
            if (string.IsNullOrEmpty(str)) { return str; }
            if (char.IsLetter(str[0]))
            {
                return str[0].ToString().ToUpper() + str.Substring(1);
            }
            return str;
        }
    }
}
