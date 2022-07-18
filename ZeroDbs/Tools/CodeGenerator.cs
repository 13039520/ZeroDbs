using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace ZeroDbs.Tools
{
    public class CodeGenerator
    {
        public class SingleTableGeneratedEventArgs : EventArgs
        {
            public IDbInfo db { get; }
            public ITableInfo table { get; }
            public string entityClassFullName { get; } 
            public string entityClassPath { get; }
            public int tableCount { get; }
            public int tableNum { get; }
            public Config generatorConfig { get; }

            public SingleTableGeneratedEventArgs(IDbInfo db, ITableInfo table, Config generatorConfig, string entityClassFullName, string entityClassPath, int tableCount, int tableNum)
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
            public string GenerateRemark { get; set; }
            private bool _IsPartialClass = true;
            public bool IsPartialClass { get { return _IsPartialClass; } set { _IsPartialClass = value; } }
        }

        public delegate void SingleTableGeneratedHandler(SingleTableGeneratedEventArgs e);

        public event SingleTableGeneratedHandler OnSingleTableGenerated;
        public Config GeneratorConfig { get; set; }
        private List<IDbInfo> _Dbs;
        public List<IDbInfo> Dbs { get { return _Dbs; } }

        public CodeGenerator() {
            _Dbs = new List<IDbInfo>();
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

            if (Dbs.Count < 1|| Dbs.Contains(null))
            {
                throw new Exception("Dbs error");
            }

            Builder(Dbs, GeneratorConfig);

        }

        private void Builder(List<IDbInfo> dbsList, Config config)
        {
            var dbs = new Common.DbService().GetDbs(dbsList);
            foreach (var key in dbs.Keys)
            {
                var db = dbs[key];
                var tables = db.GetTables();
                Builder(db.Database, tables, db.Database, config.EntityDir, config.AppProjectDir, config.EntityNamespace, config.GenerateRemark, config.IsPartialClass);
            }
        }
        private void Builder(IDbInfo db, List<ITableInfo> tables, IDbInfo dbInfo, string entityProjectRootDir, string targetProjectRootDir, string nameSpace, string generateRemark, bool isPartialClass)
        {

            string entityFileSaveDir = System.IO.Path.Combine(entityProjectRootDir, dbInfo.Key);
            string dbConfigFilePath = System.IO.Path.Combine(targetProjectRootDir, "ZeroDbConfig.xml");
            if (!System.IO.Directory.Exists(entityFileSaveDir))
            {
                System.IO.Directory.CreateDirectory(entityFileSaveDir);
            }
            if (!System.IO.Directory.Exists(targetProjectRootDir))
            {
                System.IO.Directory.CreateDirectory(targetProjectRootDir);
            }
            if (!nameSpace.EndsWith("." + dbInfo.Key))
            {
                nameSpace += "." + dbInfo.Key;
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
            System.Xml.XmlNode nodeDb = doc.SelectSingleNode(@"/zero/dbs/db[@dbKey='" + dbInfo.Key + "']");
            if (nodeDb == null)
            {
                nodeDb = doc.CreateElement("db");
                System.Xml.XmlAttribute attribute = doc.CreateAttribute("dbKey");
                attribute.Value = dbInfo.Key;
                nodeDb.Attributes.Append(attribute);
                attribute = doc.CreateAttribute("dbConnectionString");
                attribute.Value = dbInfo.ConnectionString;
                nodeDb.Attributes.Append(attribute);
                attribute = doc.CreateAttribute("dbType");
                attribute.Value = dbInfo.Type.ToString();
                nodeDb.Attributes.Append(attribute);
                nodeDbs.AppendChild(nodeDb);
            }
            #endregion

            int tableIndex = 0;
            int myTablesCount = tables.Count;

            #region -- tables foreach --
            foreach (var table in tables)
            {
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
                string className = System.Text.RegularExpressions.Regex.Replace(table.Name, @"^(t|v|tb|view)_", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                className = FirstLetterToUpper(className);
                className = (table.IsView ? "v" : "t") + className;

                EntityCodeDom codeDom = new EntityCodeDom(nameSpace, className, isPartialClass, tDescription, new string[0], new CodeAttributeDeclaration[] {
                    new CodeAttributeDeclaration("Serializable")
                });

                foreach (var column in table.Colunms)
                {
                    #region -- code --
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
                    Type type = GetRealType(colDotNetDataType, column.IsNullable);
                    if(type is null)
                    {
                        throw new Exception("unrecognized underlying type name \""+ colDotNetDataType + "\"");
                    }
                    codeDom.AddProperty(column.Name, type, GetInitExpression(colDotNetDataType, colDefaultValue), colDescription);
                    #endregion
                }

                string filePath = System.IO.Path.Combine(entityFileSaveDir, className + ".cs");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                System.IO.File.AppendAllText(filePath, codeDom.GenerateCSharpCode(generateRemark), Encoding.UTF8);

                #region -- ZeroDbConfig.xml: /zero/dvs/dv --
                string xpath = @"/zero/dvs/dv[@entityKey='" + nameSpace + "." + className + "' and @dbKey='" + dbInfo.Key + "']";
                System.Xml.XmlNode nodeDv = doc.SelectSingleNode(xpath);
                if (nodeDv == null)
                {
                    nodeDv = doc.CreateElement("dv");
                    System.Xml.XmlAttribute attribute = doc.CreateAttribute("dbKey");
                    attribute.Value = dbInfo.Key;
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

        private System.Type GetRealType(string type, bool isNullable)
        {
            switch (type)
            {
                case "long":
                    return isNullable ? typeof(long?) : typeof(long);
                case "int":
                    return isNullable ? typeof(int?) : typeof(int);
                case "short":
                    return isNullable ? typeof(short?) : typeof(short);
                case "byte":
                    return isNullable ? typeof(byte?) : typeof(byte);
                case "decimal":
                    return isNullable ? typeof(decimal?) : typeof(decimal);
                case "double":
                    return isNullable ? typeof(double?) : typeof(double);
                case "float":
                    return isNullable ? typeof(float?) : typeof(float);
                case "bool":
                    return isNullable ? typeof(bool?) : typeof(bool);
                case "DateTime":
                    return isNullable ? typeof(DateTime?) : typeof(DateTime);
                case "DateTimeOffset":
                    return isNullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset);
                case "TimeSpan":
                    return isNullable ? typeof(TimeSpan?) : typeof(TimeSpan);
                case "Guid":
                    return isNullable ? typeof(Guid?) : typeof(Guid);
                // is not value type
                case "byte[]":
                    return typeof(byte[]);
                case "string":
                    return typeof(string);
                case "object":
                    return typeof(object);
            }
            return null;
        }
        private CodeExpression GetInitExpression(string type, string dVal)
        {
            if (!string.IsNullOrEmpty(dVal))
            {
                if (type != "string")
                {
                    return new CodeSnippetExpression(dVal);
                }
            }
            return null;
        }

    }
}
