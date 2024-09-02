using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace ZeroDbs.Common
{
    public class DbConfigReader
    {
        private static DbConfigInfo config = null;
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
                if (config.Dbs.Find(o => string.Equals(o.Key, dbKey, StringComparison.OrdinalIgnoreCase)) == null) { return false; }
                var table = config.Dvs.Find(o => string.Equals(o.DbKey, dbKey, StringComparison.OrdinalIgnoreCase) && string.Equals(o.TableName, tableName, StringComparison.OrdinalIgnoreCase));
                if (table != null) { return false; }
                config.Dvs.Add(new DbTableEntityMap { DbKey = dbKey, EntityKey = entityFullName, TableName = tableName, IsStandardMapping = false });
            }
            return true;
        }
        public static bool AddDbConfig(string dbKey, string dbType, string dbConnectionString, string dbConnectionString2 = "")
        {
            if (string.IsNullOrEmpty(dbKey) || string.IsNullOrEmpty(dbType) || string.IsNullOrEmpty(dbConnectionString))
            {
                return false;
            }
            var config = GetDbConfigInfo();
            lock (_lock)
            {
                if (config.Dbs.Find(o => string.Equals(o.Key, dbKey, StringComparison.OrdinalIgnoreCase)) != null) { return false; }
                config.Dbs.Add(new DbInfo {  Key = dbKey, Type = dbType, ConnectionString = dbConnectionString, ConnectionString2 = dbConnectionString2 });
            }
            return true;
        }
        private static DbConfigInfo Read()
        {
            if (config != null) { return config; }
            lock (_lock)
            {
                if (config != null) { return config; }
                DbConfigInfo temp = ReadFile();
                if (temp != null)
                {
                    config = temp;
                }
            }
            return config;
        }
        private static DbConfigInfo ReadFile()
        {
            DbConfigInfo temp = new DbConfigInfo
            {
                Dbs = new List<DbInfo>(),
                Dvs = new List<DbTableEntityMap>()
            };
            if (string.IsNullOrEmpty(filePath))
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(dir);
                System.IO.FileInfo[] files = directoryInfo.GetFiles(fileName, System.IO.SearchOption.AllDirectories);
                if (files == null || files.Length < 1)
                {
                    return temp;
                }
                filePath = files[0].FullName;
            }
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                return temp;
            }
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.Load(filePath);
            System.Xml.XmlNodeList xmlNodeList = xmlDocument.SelectNodes(@"/zero/dbs/db");
            if (xmlNodeList == null || xmlNodeList.Count < 1)
            {
                return temp;
            }
            foreach (System.Xml.XmlNode node in xmlNodeList)
            {
                System.Xml.XmlAttribute dbKey = node.Attributes["dbKey"];
                System.Xml.XmlAttribute dbConnectionString = node.Attributes["dbConnectionString"];
                System.Xml.XmlAttribute dbConnectionString2 = node.Attributes["dbConnectionString2"];
                System.Xml.XmlAttribute dbType = node.Attributes["dbType"];
                if (dbKey == null || dbConnectionString == null || dbType == null)
                {
                    continue;
                }
                string key = dbKey.Value;
                string conn = dbConnectionString.Value;
                string conn2 = dbConnectionString2 != null ? dbConnectionString2.Value : "";
                string type = dbType.Value;
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(conn) || string.IsNullOrEmpty(type))
                {
                    continue;
                }
                type = type.Trim();
                if(string.IsNullOrEmpty(type))
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
                    ConnectionString2 = conn2,
                    Key = key,
                    Type = type
                });
            }
            if (temp.Dbs.Count < 1)
            {
                return null;
            }
            xmlNodeList = xmlDocument.SelectNodes(@"/zero/dvs/dv");
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
                string name = tableName.Value;
                string entity = entityKey.Value;

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
            return temp;
        }
    }
}
