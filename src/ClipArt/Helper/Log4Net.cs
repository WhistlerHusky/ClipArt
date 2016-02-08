using System;
using System.Diagnostics;
using System.IO;

namespace ClipArt
{
    /// <summary>
    /// 
    /// </summary>
    public static class Log4Net
    {
        /// <summary>
        /// 
        /// </summary>
        public static void InitLog4Net()
        {
            var hierarchy = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();

            hierarchy.Configured = true;

            var rollingAppender = new log4net.Appender.RollingFileAppender
            {
                File = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" +
                string.Format("{0:yyyy-MM-dd}", DateTime.Now) + ".log",
                AppendToFile = true,
                RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Date,
                LockingModel = new log4net.Appender.FileAppender.MinimalLock(),
                DatePattern = "_yyyyMMdd-hh\".log\"",
              
                Layout = new log4net.Layout.PatternLayout("[%thread] %level %logger - %message [Extra Info: %property{testProperty} %exception]%n")
            };

            var traceAppender = new log4net.Appender.TraceAppender()
            {
                Layout = new log4net.Layout.PatternLayout("[%thread] %level %logger - %message [Extra Info: %property{testProperty} %exception]%n")
            };

            hierarchy.Root.AddAppender(rollingAppender);
            hierarchy.Root.AddAppender(traceAppender);
            rollingAppender.ActivateOptions();

            hierarchy.Root.Level = log4net.Core.Level.All;  
        }
    }
}
