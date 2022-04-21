using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Tools
{
    public class CodeGenerator
    {
        static readonly string template = @"using System;
using System.Collections.Generic;
using System.Text;
namespace %NameSpace%
{
    /// <summary>
    /// %TableDescription%
    /// </summary>
    [Serializable]
    public partial class %ClassName%
    {
        %ColumnRepeatBegin%
        private %ColumnDotNetType% _%ColumnName%%ColumnDotNetDefaultValue%;
        /// <summary>
        /// %ColumnDescription%
        /// </summary>
        public %ColumnDotNetType% %ColumnName%
        {
            get { return _%ColumnName%; }
            set { _%ColumnName% = value; }
        }%ColumnRepeatEnd%

    }
}";
        public class SingleTableGeneratedEventArgs : EventArgs
        {
            public Common.DatabaseInfo db { get; }
            public Common.DbDataTableInfo table { get; }
            public string entityClassFullName { get; } 
            public string entityClassPath { get; }
            public int tableCount { get; }
            public int tableNum { get; }
            public Config generatorConfig { get; }

            public SingleTableGeneratedEventArgs(Common.DatabaseInfo db, Common.DbDataTableInfo table, Config generatorConfig, string entityClassFullName, string entityClassPath, int tableCount, int tableNum)
            {
                this.db = db;
                this.table = table;
                this.generatorConfig = generatorConfig;
                this.entityClassFullName = entityClassFullName;
                this.entityClassPath = entityClassPath;
                this.tableCount = tableCount;
                this.tableNum = tableNum;
            }
        }

        public class Config
        {
            public string AppProjectName { get; set; }
            public string AppProjectNamespace { get; set; }
            public string AppProjectDir { get; set; }
            public string EntityProjectName { get; set; }
            public string EntityNamespace { get; set; }
            public string EntityDir { get; set; }
            public string EntityTemplate { get; set; }
        }

        public delegate void SingleTableGeneratedHandler(SingleTableGeneratedEventArgs e);

        public event SingleTableGeneratedHandler OnSingleTableGenerated;
        public Config GeneratorConfig { get; set; }
        private List<Common.DatabaseInfo> _Dbs;
        public List<Common.DatabaseInfo> Dbs { get { return _Dbs; } }

        public CodeGenerator() {
            _Dbs = new List<Common.DatabaseInfo>();
        }

        public void Run()
        {
            if (this.GeneratorConfig == null)
            {
                throw new Exception("GeneratorConfig is null");
            }
            if (string.IsNullOrEmpty(this.GeneratorConfig.EntityProjectName))
            {
                throw new Exception("GeneratorConfig.EntityProjectName is null or empty");
            }
            this.GeneratorConfig.EntityProjectName = this.GeneratorConfig.EntityProjectName.Trim();
            if (string.IsNullOrEmpty(this.GeneratorConfig.EntityNamespace))
            {
                throw new Exception("GeneratorConfig.EntityNamespace is null or empty");
            }
            this.GeneratorConfig.EntityNamespace = this.GeneratorConfig.EntityNamespace.Trim();
            if (string.IsNullOrEmpty(this.GeneratorConfig.EntityDir))
            {
                throw new Exception("GeneratorConfig.EntityDir is null or empty");
            }
            this.GeneratorConfig.EntityDir = this.GeneratorConfig.EntityDir.Trim();

            if (string.IsNullOrEmpty(this.GeneratorConfig.AppProjectDir))
            {
                throw new Exception("GeneratorConfig.AppProjectDir is null or empty");
            }
            this.GeneratorConfig.AppProjectDir = this.GeneratorConfig.AppProjectDir.Trim();
            if (string.IsNullOrEmpty(GeneratorConfig.EntityTemplate))
            {
                GeneratorConfig.EntityTemplate = template;
            }

            if (Dbs.Count < 1|| Dbs.Contains(null))
            {
                throw new Exception("Dbs error");
            }

            Builder(Dbs, GeneratorConfig);

        }

        private void Builder(List<Common.DatabaseInfo> dbsList, Config generatorConfig)
        {
            var dbs = new Common.DbService().GetDbs(dbsList);
            foreach (var key in dbs.Keys)
            {
                var db = dbs[key];
                var tables = db.GetTables();
                Builder(db.Database, tables, db.Database, generatorConfig.EntityDir, generatorConfig.AppProjectDir, generatorConfig.EntityNamespace, generatorConfig.EntityTemplate);
            }
        }
        private void Builder(Common.DatabaseInfo db, List<Common.DbDataTableInfo> tables, Common.DatabaseInfo dbInfo, string entityProjectRootDir, string targetProjectRootDir, string nameSpace, string entityTemplate = "")
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
                @"%ColumnRepeatBegin%(?<temp>[\s\S]+)%ColumnRepeatEnd%",
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

            int tableIndex = 0;
            int myTablesCount = tables.Count;

            #region -- tables foreach --
            foreach (var table in tables)
            {
                string claText = entityTemplate;
                string tDescription = table.Description;
                if (string.IsNullOrEmpty(tDescription))
                {
                    if (table.IsView)
                    {
                        tDescription = "VIEW:" + table.Name;
                    }
                    else
                    {
                        tDescription = "TABLE:" + table.Name;
                    }
                }
                claText = claText.Replace("%NameSpace%", nameSpace);
                claText = claText.Replace("%TableDescription%", tDescription);
                string className = System.Text.RegularExpressions.Regex.Replace(table.Name, @"^(t|v|tb|view)_", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                className = FirstLetterToUpper(className);
                className = (table.IsView ? "v" : "t") + className;
                claText = claText.Replace("%ClassName%", className);

                StringBuilder colsText = new StringBuilder();
                foreach (var column in table.Colunms)
                {
                    #region -- code --
                    string colText = ColumnLoopTemplate;
                    string colDescription = column.Description;
                    string colDefaultValue = column.DefaultValue;
                    string colDotNetDataType = column.Type;
                    if (string.IsNullOrEmpty(colDescription))
                    {
                        colDescription = column.Name;
                    }
                    if (column.IsPrimaryKey)
                    {
                        colDescription = "[PrimaryKey]" + colDescription;
                    }
                    if (column.IsIdentity)
                    {
                        colDescription = "[Identity]" + colDescription;
                    }
                    if (string.IsNullOrEmpty(colDefaultValue)) { colDefaultValue = ""; }
                    if (colDefaultValue.Length > 0)
                    {
                        colDefaultValue = " = " + colDefaultValue;
                    }
                    if (column.IsNullable)
                    {
                        if (colDotNetDataType != "string" &&
                            colDotNetDataType != "byte[]" &&
                            colDotNetDataType != "object" &&
                            colDotNetDataType != "object*" &&
                            colDotNetDataType != "Xml")
                        {
                            colDotNetDataType = colDotNetDataType + "?";
                        }
                    }
                    else
                    {
                        if (colDotNetDataType == "string")
                        {
                            if (colDefaultValue.Length < 1)
                            {
                                colDefaultValue = " = \"\"";
                            }
                        }
                    }

                    colText = colText.Replace("%ColumnDotNetType%", colDotNetDataType);
                    colText = colText.Replace("%ColumnName%", FirstLetterToUpper(column.Name));
                    colText = colText.Replace("%ColumnDotNetDefaultValue%", colDefaultValue);
                    colText = colText.Replace("%ColumnDescription%", colDescription);

                    colsText.Append(colText);

                    #endregion
                }

                claText = System.Text.RegularExpressions.Regex.Replace(
                    claText,
                    @"%ColumnRepeatBegin%(?<temp>[\s\S]+)%ColumnRepeatEnd%",
                    colsText.ToString());

                string filePath = System.IO.Path.Combine(entityFileSaveDir, className + ".cs");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                System.IO.File.AppendAllText(filePath, claText, Encoding.UTF8);

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

                if (OnSingleTableGenerated != null)
                {
                    OnSingleTableGenerated(new SingleTableGeneratedEventArgs(db, table, GeneratorConfig, nameSpace + "." + className, filePath, myTablesCount, tableIndex));
                }

            }
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
        private string FirstLetterToUpper(string str)
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
