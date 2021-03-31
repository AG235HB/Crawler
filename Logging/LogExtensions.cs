using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Crawler.Logging
{
    public static class LogExtensions
    {
        private static void AddGlobalContext(LogEventInfo eventInfo)
        {
            // Глобальные параметры по умолчанию для всех отправлений
            eventInfo.Properties.Add("Global", "Global information");
        }

        //будет удалено
        public static void Write(this Logger logger, int level, Exception exception, IDictionary<string, object> eventParameters, string message)
        {
            Console.WriteLine(message);
            foreach (var item in eventParameters) Console.WriteLine(item.Key + ":\"" + item.Value + "\" ");
            Console.WriteLine();
            InnerWriteLogEvent(logger, level, exception, (eventInfoProperties) => eventParameters?.ForEach(pair => eventInfoProperties.Add(pair.Key, pair.Value)), message, null, null);
        }

        public static void Write(this Logger logger, LogLevel level, Exception exception, IDictionary<string, object> eventParameters, string message)
        {
            Console.WriteLine(message);
            foreach (var item in eventParameters) Console.WriteLine(item.Key + ":\"" + item.Value + "\" ");
            Console.WriteLine();
            InnerWriteLogEvent(logger, level, exception, (eventInfoProperties) => eventParameters?.ForEach(pair => eventInfoProperties.Add(pair.Key, pair.Value)), message, null, null);
        }

        //будет удалено
        private static void InnerWriteLogEvent(Logger logger, int level, Exception exception, Action<IDictionary<object, object>> eventPropertiesFiller, string message, DateTime? timestamp, params object[] args)
        {
            LogEventInfo eventInfo = new LogEventInfo(GetLogLevel(level), logger.Name, CultureInfo.CurrentCulture, message, args);
            if (timestamp.HasValue)
                eventInfo.TimeStamp = timestamp.Value;
            eventInfo.Exception = exception;
            // AddGlobalContext(eventInfo);
            eventPropertiesFiller?.Invoke(eventInfo.Properties);
            logger.Log(eventInfo);
        }

        private static void InnerWriteLogEvent(Logger logger, LogLevel level, Exception exception, Action<IDictionary<object, object>> eventPropertiesFiller, string message, DateTime? timestamp, params object[] args)
        {
            LogEventInfo eventInfo = new LogEventInfo(level, logger.Name, CultureInfo.CurrentCulture, message, args);
            if (timestamp.HasValue)
                eventInfo.TimeStamp = timestamp.Value;
            eventInfo.Exception = exception;
            // AddGlobalContext(eventInfo);
            eventPropertiesFiller?.Invoke(eventInfo.Properties);
            logger.Log(eventInfo);
        }

        private static LogLevel GetLogLevel(int warningLevelIndex)
        {
            switch (warningLevelIndex)
            {
                case 1:
                    return LogLevel.Fatal;
                case 2:
                    return LogLevel.Error;
                case 3:
                    return LogLevel.Warn;
                case 4:
                    return LogLevel.Info;
                case 5:
                    return LogLevel.Debug;
                case 6:
                    return LogLevel.Trace;
                default:
                    return LogLevel.Info;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var element in source)
                action(element);
        }
    }
}