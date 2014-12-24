using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Repository.Hierarchy;

namespace Cosmoser.PingAnMeetingRequest.Common.Utilities
{
    public static class IosLogManager
    {
        public static void InitializeLog4Net()
        {
            var layout = new log4net.Layout.PatternLayout()
                {
                    ConversionPattern = "时间:%d %n级别:%level %n类名:%c%n文件:%F 第%L行%n日志内容:%m%n-----------------------------------------%n%n"
                };
            var appender = new log4net.Appender.RollingFileAppender()
            {
                AppendToFile = true,
                LockingModel = new log4net.Appender.FileAppender.MinimalLock(),
                StaticLogFileName = false,
                File = System.Configuration.ConfigurationManager.AppSettings["LogFolder"] + "log_PingAn_",
                RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Date,
                DatePattern = "yyyy-MM-dd\".log\"",
                Layout = layout
                
            };
            layout.ActivateOptions();
            appender.ActivateOptions();
            log4net.Config.BasicConfigurator.Configure(appender);

            //log4net.Config.XmlConfigurator.Configure();
        }

        static IosLogManager()
        {
            InitializeLog4Net();
            //log4net.Config.XmlConfigurator.Configure();
        }

        public static ILog GetLogger(Type type)
        {
            ILog logger = LogManager.GetLogger(type);
            Logger currentlogger = (Logger)logger.Logger;
            currentlogger.Level = currentlogger.Hierarchy.LevelMap[System.Configuration.ConfigurationManager.AppSettings["Level"]];

            return logger;
        }
    }
}
