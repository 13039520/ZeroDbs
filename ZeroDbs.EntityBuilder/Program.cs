using System;
using System.Text;
using System.Collections.Generic;

namespace ZeroDbs.EntityBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            EntityBuilder();
            string s = "疑是银河落九天";
            Console.WriteLine("原始字符串："+s);
            s = ZeroDbs.Tools.DES.Encrypt(s);
            Console.WriteLine("加密字符串：" + s);
            s = ZeroDbs.Tools.DES.Decrypt(s);
            Console.WriteLine("解密字符串：" + s);
        }
        static void EntityBuilder()
        {
            string entityProjectNameSpace = System.Configuration.ConfigurationManager.AppSettings["EntityProjectNameSpace"];
            if (string.IsNullOrEmpty(entityProjectNameSpace))
            {
                entityProjectNameSpace = "ZeroDbs.Entity";
            }
            string entityProjectRootDir = System.Configuration.ConfigurationManager.AppSettings["EntityProjectRootDir"];
            if (string.IsNullOrEmpty(entityProjectRootDir))
            {
                entityProjectRootDir = System.IO.Path.Combine(AppContext.BaseDirectory, "Entitys");
            }
            string targetProjectRootDir = System.Configuration.ConfigurationManager.AppSettings["TargetProjectRootDir"];
            if (string.IsNullOrEmpty(targetProjectRootDir))
            {
                targetProjectRootDir = entityProjectRootDir;
            }

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\tbegin......");

            ZeroDbs.Tools.EntityBuilder.Builder(new Interfaces.Common.DbConfigDatabaseInfo
            {
                dbConnectionString = "Data Source=D:\\Program Files\\SQLiteStudio311\\database\\test.db;Version=3;",
                dbKey = "Test",
                dbType = "Sqlite"
            }, entityProjectRootDir, targetProjectRootDir, entityProjectNameSpace);

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\tend......");
        }

    }
}

