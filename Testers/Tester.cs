using System;
using System.Collections.Generic;
using NLog;
using Crawler.Logging;

namespace Crawler.Testers
{
    public abstract class Tester
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private IDictionary<string, object> LogAttribures = new Dictionary<string, object>();
        public string Host;
        public Tester() { }

        public abstract void Run();

        public void LogInfo(IDictionary<string, object> attributes, string message)
        {
            IDictionary<string, object> TempAttributes = new Dictionary<string, object>();
            foreach (var attr in LogAttribures) TempAttributes.Add(attr);
            foreach (var attr in attributes) TempAttributes.Add(attr);
            Log.Write(LogLevel.Info, null, TempAttributes, message);
        }

        public void LogInfo(string message)
        {
            Log.Write(LogLevel.Info, null, LogAttribures, message);
        }

        public void LogDebug(IDictionary<string, object> attributes, string message)
        {
            IDictionary<string, object> TempAttributes = new Dictionary<string, object>();
            foreach (var attr in LogAttribures) TempAttributes.Add(attr);
            foreach (var attr in attributes) TempAttributes.Add(attr);
            Log.Write(LogLevel.Debug, null, TempAttributes, message);
        }

        public void LogDebug(string message)
        {
            Log.Write(LogLevel.Debug, null, LogAttribures, message);
        }

        public void LogError(IDictionary<string, object> attributes, string message)
        {
            IDictionary<string, object> TempAttributes = new Dictionary<string, object>();
            foreach (var attr in LogAttribures) TempAttributes.Add(attr);
            foreach (var attr in attributes) TempAttributes.Add(attr);
            Log.Write(LogLevel.Error, null, TempAttributes, message);
        }

        public void LogError(string message)
        {
            Log.Write(LogLevel.Error, null, LogAttribures, message);
        }

        public void LogException(Exception exception, IDictionary<string, object> attributes, string message)
        {
            IDictionary<string, object> TempAttributes = new Dictionary<string, object>();
            foreach (var attr in LogAttribures) TempAttributes.Add(attr);
            foreach (var attr in attributes) TempAttributes.Add(attr);
            Log.Write(LogLevel.Error, exception, TempAttributes, message);
        }

        public void SetLogAttributes(Dictionary<string, object> attributes)
        {
            LogAttribures = attributes;
        }
    }
}