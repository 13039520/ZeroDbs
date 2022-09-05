using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Logs
{
    public static class Factory
    {
        private static object setFileThreadCreateLockObj = new object();
        private static object loggerCreateLockObj = new object();
        public static readonly string logsDirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        internal static readonly System.Collections.Generic.Dictionary<string, ILog> loggerDic = new System.Collections.Generic.Dictionary<string, ILog>();
        internal static System.Threading.Thread setFileThread = null;
        internal static void SetFileThreadStartFunc(object obj)
        {
            while (true)
            {
                try
                {
                    foreach (string key in loggerDic.Keys)
                    {
                        loggerDic[key].SaveLogToFile();
                        loggerDic[key].ClearLogFile();
                    }
                    System.Threading.Thread.Sleep(1);
                }
                catch { }
            }
        }
        public static void SetFileThreadStart()
        {
            if (setFileThread == null)
            {
                lock (setFileThreadCreateLockObj)
                {
                    if (setFileThread == null)
                    {
                        setFileThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(SetFileThreadStartFunc));
                        setFileThread.IsBackground = true;
                        setFileThread.Start(null);
                    }
                }
            }
        }
        public static ILog GetLogger()
        {
            return GetLogger("Trace");
        }
        public static ILog GetLogger(string logFilePre)
        {
            return GetLogger(logFilePre, 90);
        }
        public static ILog GetLogger(string logFilePre, int logFileRetentionDays)
        {
            logFilePre = GetLogFilePre(logFilePre);
            if (loggerDic.ContainsKey(logFilePre))
            {
                return loggerDic[logFilePre];
            }
            else
            {
                lock (loggerCreateLockObj)
                {
                    if (loggerDic.ContainsKey(logFilePre))
                    {
                        return loggerDic[logFilePre];
                    }
                    else
                    {
                        ILog _logger = new Logger(logFilePre, logFileRetentionDays);
                        loggerDic.Add(logFilePre, _logger);
                        return _logger;
                    }
                }
            }
        }
        public static string GetLogFilePre(string logFilePre)
        {
            if (string.IsNullOrEmpty(logFilePre))

            {
                logFilePre = "Trace";
            }
            //logFilePre = logFilePre.ToLower();
            if (!logFilePre.EndsWith("-"))
            {
                logFilePre = logFilePre + "-";
            }
            //logFilePre = logFilePre.Substring(0, 1).ToUpper() + logFilePre.Substring(1);
            return logFilePre;
        }
        public static System.Collections.Generic.List<string> GetLogFilePreList()
        {
            System.Collections.Generic.List<string> reval = new System.Collections.Generic.List<string>();
            foreach (string key in loggerDic.Keys)
            {
                reval.Add(key);
            }
            return reval;
        }

    }
}
