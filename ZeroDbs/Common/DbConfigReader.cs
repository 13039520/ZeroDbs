using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Common
{
    public class DbConfigReader
    {
        private static DbConfigInfo zeroConfigInfo = null;
        private static readonly string fileName = "ZeroDbConfig.xml";
        private static string filePath = string.Empty;
        public static DbConfigInfo GetDbConfigInfo()
        {
            return Read();
        }
        private static object _lock = new object();
        public static bool AddTableMapping(string entityFullName, string dbKey, string tableName)
        {
            if (string.IsNullOrEmpty(entityFullName) || string.IsNullOrEmpty(dbKey) || string.IsNullOrEmpty(tableName))
            {
                return false;
            }
            var config = GetDbConfigInfo();
            lock (_lock)
            {
                if (config.Dbs.Find(o => o.Key == dbKey) == null) { return false; }
                if (config.Dvs.Find(o => o.TableName == tableName) == null) { return false; }
                config.Dvs.Add(new DbTableEntityMap { DbKey = dbKey, EntityKey = entityFullName, TableName = tableName, IsStandardMapping = false });
            }
            return true;
        }
        private static DbConfigInfo Read()
        {
            if (zeroConfigInfo == null)
            {
                lock (_lock)
                {
                    if (zeroConfigInfo == null)
                    {
                        DbConfigInfo temp = ReadFile();
                        if (temp != null)
                        {
                            zeroConfigInfo = temp;
                        }
                    }
                }
            }
            return zeroConfigInfo;
        }
        private static DbConfigInfo ReadFile()
        {
            DbConfigInfo temp = new DbConfigInfo();
            if (string.IsNullOrEmpty(filePath))
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(dir);
                System.IO.FileInfo[] files = directoryInfo.GetFiles(fileName, System.IO.SearchOption.AllDirectories);
                if (files == null || files.Length < 1)
                {
                    //throw new Exception("缺少配置文件ZeroDbConfig.xml");
                    return temp;
                }
                filePath = files[0].FullName;
            }
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                //throw new Exception("配置文件ZeroDbConfig.xml不存在");
                return temp;
            }
            
            try
            {
                System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                xmlDocument.Load(filePath);
                System.Xml.XmlNodeList xmlNodeList = xmlDocument.SelectNodes(@"/zero/dbs/db");//GetElementsByTagName("db");
                if(xmlNodeList == null|| xmlNodeList.Count < 1)
                {
                    throw new Exception("缺少db配置");
                }
                temp.Dbs = new List<DbInfo>();
                foreach(System.Xml.XmlNode node in xmlNodeList)
                {
                    System.Xml.XmlAttribute dbKey = node.Attributes["dbKey"];
                    System.Xml.XmlAttribute dbConnectionString = node.Attributes["dbConnectionString"];
                    System.Xml.XmlAttribute dbType = node.Attributes["dbType"];
                    if (dbKey == null || dbConnectionString == null || dbType == null)
                    {
                        continue;
                    }
                    string key = dbKey.Value;
                    string conn = dbConnectionString.Value;
                    string type = dbType.Value;
                    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(conn) || string.IsNullOrEmpty(type))
                    {
                        continue;
                        
                    }
                    DbType type1 = DbType.Unknow;
                    type = type.Trim();
                    if(string.Equals(type, "SqlServer", StringComparison.OrdinalIgnoreCase))
                    {
                        type1 = DbType.SqlServer;
                    }
                    else if (string.Equals(type, "MySql", StringComparison.OrdinalIgnoreCase))
                    {
                        type1 = DbType.MySql;
                    }
                    else if (string.Equals(type, "Sqlite", StringComparison.OrdinalIgnoreCase))
                    {
                        type1 = DbType.Sqlite;
                    }
                    else
                    {
                        continue;
                    }
                    key = key.Trim();
                    conn = conn.Trim();
                    if (temp.Dbs.Find(o => string.Equals(o.Key, key, StringComparison.OrdinalIgnoreCase)) != null)
                    {
                        continue;
                    }
                    temp.Dbs.Add(new DbInfo
                    {
                        ConnectionString = conn,
                        Key = key,
                        Type = type1
                    });
                }
                if (temp.Dbs.Count < 1)
                {
                    return null;
                }
                xmlNodeList = xmlDocument.SelectNodes(@"/zero/dvs/dv");
                temp.Dvs = new List<DbTableEntityMap>();
                if (xmlNodeList == null || xmlNodeList.Count < 1)
                {
                    return temp;
                }
                foreach (System.Xml.XmlNode node in xmlNodeList)
                {
                    System.Xml.XmlAttribute dbKey = node.Attributes["dbKey"];
                    System.Xml.XmlAttribute tableName = node.Attributes["tableName"];
                    System.Xml.XmlAttribute entityKey = node.Attributes["entityKey"];
                    if (dbKey == null || tableName == null || entityKey == null)
                    {
                        continue;
                    }
                    string key = dbKey.Value;
                    string name= tableName.Value;
                    string entity= entityKey.Value;

                    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(entity))
                    {
                        continue;

                    }
                    key = key.Trim();
                    name = name.Trim();
                    entity = entity.Trim();

                    if (temp.Dbs.Find(o => string.Equals(o.Key, key, StringComparison.OrdinalIgnoreCase)) == null)
                    {
                        continue;
                    }
                    if (temp.Dvs.Find(o => string.Equals(o.EntityKey, entity, StringComparison.OrdinalIgnoreCase)) != null)
                    {
                        continue;
                    }
                    temp.Dvs.Add(new DbTableEntityMap
                    {
                        DbKey = key,
                        TableName = name,
                        EntityKey = entity,
                        IsStandardMapping = true
                    });
                }
            }
            catch {
                temp = null;
            }

            return temp;
        }
    }
}
