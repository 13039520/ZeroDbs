using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Logs
{
    class Logger: ILog
    {
        private System.Text.StringBuilder logSource = null;
        private string logFilePre = string.Empty;
        private System.IO.FileStream fileStream = null;
        private DateTime logFileScanLastTime = DateTime.Now;
        private int logFileRetentionDays = 90;
        private bool LogPerDateTime = true;

        public Logger(string logFilePre)
            : this(logFilePre, 90)
        {

        }
        public Logger(string logFilePre, int logFileRetentionDays)
            : this(logFilePre, logFileRetentionDays, true)
        {

        }
        public Logger(string logFilePre, int logFileRetentionDays, bool logPerDateTime)
        {
            this.logFilePre = logFilePre;
            this.logSource = new System.Text.StringBuilder();
            if (logFileRetentionDays < 1)
            {
                logFileRetentionDays = 1;
            }
            logFileScanLastTime = DateTime.Now.AddMinutes(-5);
            this.logFileRetentionDays = logFileRetentionDays;
            this.LogPerDateTime = logPerDateTime;
            Factory.SetFileThreadStart();
        }


        private System.IO.FileStream GetFileStream()
        {
            if (!System.IO.Directory.Exists(Factory.logsDirPath))
            {
                System.IO.Directory.CreateDirectory(Factory.logsDirPath);
            }
            System.IO.FileStream fs;
            string FilePath = System.IO.Path.Combine(Factory.logsDirPath, this.logFilePre + DateTime.Now.ToString("yyyyMMdd") + ".log");
            if (!System.IO.File.Exists(FilePath))
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
                fileStream = fs = new System.IO.FileStream(FilePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.Read, 1024, true);
            }
            else
            {
                if (fileStream != null)
                {
                    fs = fileStream;
                }
                else
                {
                    fileStream = fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Open, System.IO.FileAccess.Write, System.IO.FileShare.Read, 1024, true);
                }
            }
            return fs;
        }
        private string GetLogText()
        {
            string s = "";
            if (logSource.Length > 0)
            {
                lock (logSource)
                {
                    s = logSource.ToString();
                    logSource.Clear();
                }
            }
            return s;
        }
        public void Writer(string logInfo)
        {
            try
            {
                if (logSource == null)
                {
                    logSource = new System.Text.StringBuilder();
                }
                lock (this)
                {
                    logSource.AppendFormat("{0}{1}{2}", this.LogPerDateTime ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff\t") : "", logInfo, System.Environment.NewLine);
                }
            }
            catch { }
        }
        public void Writer(string format, params object[] args)
        {
            try
            {
                if (logSource == null)
                {
                    logSource = new System.Text.StringBuilder();
                }
                lock (this)
                {
                    logSource.AppendFormat("{0}", this.LogPerDateTime ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff\t") : "");
                    logSource.AppendFormat(format, args);
                    logSource.Append(System.Environment.NewLine);
                }
            }
            catch { }
        }
        public void Writer(Exception ex)
        {
            try
            {
                if (logSource == null)
                {
                    logSource = new System.Text.StringBuilder();
                }
                lock (this)
                {
                    logSource.AppendFormat("{0}", this.LogPerDateTime ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff\t") : "");
                    logSource.AppendFormat("Message={0}{1}Source={2}{3}TargetSite={4}{5}StackTrace={6}", 
                        ex.Message,
                        System.Environment.NewLine,
                        ex.Source,
                        System.Environment.NewLine,
                        ex.TargetSite,
                        System.Environment.NewLine,
                        ex.StackTrace);
                    logSource.Append(System.Environment.NewLine);
                }
            }
            catch { }
        }
        public void SaveLogToFile()
        {
            try
            {
                string logInfo = GetLogText();
                if (logInfo.Length > 0)
                {
                    System.IO.FileStream fs = GetFileStream();
                    byte[] buffer = System.Text.UTF8Encoding.UTF8.GetBytes(logInfo);
                    long lockBegin = fs.Length;
                    long lockEnd = buffer.Length;
                    fs.Position = lockBegin;
                    //fs.WriteAsync(buffer, 0, buffer.Length);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Flush();
                    //fs.Close();
                }
            }
            catch { }
        }
        public void ClearLogFile()
        {
            if ((DateTime.Now - logFileScanLastTime).TotalMinutes < 5)
            {
                return;
            }
            logFileScanLastTime = DateTime.Now;
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(Factory.logsDirPath);
            if (!directoryInfo.Exists)
            {
                return;
            }
            System.IO.FileInfo[] files = directoryInfo.GetFiles(this.logFilePre + "*.log", System.IO.SearchOption.TopDirectoryOnly);
            if (files == null || files.Length < 1)
            {
                return;
            }
            DateTime time = DateTime.Now.AddDays(0 - logFileRetentionDays);
            foreach (System.IO.FileInfo file in files)
            {
                try
                {
                    if (file.CreationTime < time)
                    {
                        file.Delete();
                    }
                }
                catch { }
            }
        }
        

    }
}
