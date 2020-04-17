using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces
{
    public interface ILog
    {
        void Writer(string logInfo);
        void Writer(string format, params object[] args);
        void Writer(Exception exception);

        void SaveLogToFile();
        void ClearLogFile();
    }
}
