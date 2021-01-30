using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Pumkin.Core.Logger
{
    public class PumkinLogger : ILogger
    {
        public ILogHandler logHandler { get; set; }
        public bool logEnabled { get; set; } = true;
        public LogType filterLogType { get; set; }

        PumkinLogger() {}

        public PumkinLogger(Color color, string tag)
        {
            if(logEnabled)
                logHandler = new TaggedLogHandler(color, tag);
        }
        public bool IsLogTypeAllowed(LogType logType)
        {
            return Debug.unityLogger.IsLogTypeAllowed(logType);
        }

        public PumkinLogger(string colorHexOrName, string tag)
        {
            if(logEnabled)
                logHandler = new TaggedLogHandler(colorHexOrName, tag);
        }


        public void Log(LogType logType, object message)
        {
            if(logEnabled)
                logHandler.LogFormat(logType, null, message as string, null);
        }

        public void Log(LogType logType, object message, Object context)
        {
            if(logEnabled)
                logHandler.LogFormat(logType, context, message as string, null);
        }

        public void Log(LogType logType, string tag, object message)
        {
            if(logEnabled)
                Debug.unityLogger.Log(logType, tag, message);
        }

        public void Log(LogType logType, string tag, object message, Object context)
        {
            if(logEnabled)
                Debug.unityLogger.Log(logType, tag, message, context);
        }

        public void Log(object message)
        {
            if(logEnabled)
                logHandler.LogFormat(LogType.Log, null, message as string, null);
        }

        public void Log(string tag, object message)
        {
            if(logEnabled)
                Debug.unityLogger.Log(LogType.Log, tag, message);
        }

        public void Log(string tag, object message, Object context)
        {
            if(logEnabled)
                Debug.unityLogger.Log(LogType.Log, tag, message, context);
        }

        public void LogWarning(string tag, object message)
        {
            if(logEnabled)
                Debug.unityLogger.Log(LogType.Warning, tag, message);
        }

        public void LogWarning(string tag, object message, Object context)
        {
            if(logEnabled)
                Debug.unityLogger.Log(LogType.Warning, tag, message, context);
        }

        public void LogError(string tag, object message)
        {
            if(logEnabled)
                Debug.unityLogger.Log(LogType.Error, tag, message);
        }

        public void LogError(string tag, object message, Object context)
        {
            if(logEnabled)
                Debug.unityLogger.Log(LogType.Error, tag, message, context);
        }

        public void LogFormat(LogType logType, string format, params object[] args)
        {
            if(logEnabled)
                logHandler.LogFormat(logType, null, format, args);
        }

        public void LogException(Exception exception)
        {
            if(logEnabled)
                logHandler.LogException(exception, null);
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            if(logEnabled)
                logHandler.LogFormat(logType, context, format, args);
        }

        public void LogException(Exception exception, Object context)
        {
            if(logEnabled)
                logHandler.LogException(exception, context);
        }
    }
}