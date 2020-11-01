#if UNITY_EDITOR
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
    class PumkinLogger : ILogger
    {
        public ILogHandler logHandler { get; set; }
        public bool logEnabled { get; set; } = true;
        public LogType filterLogType { get; set; }

        PumkinLogger() {}

        public PumkinLogger(Color color, string tag)
        {
            logHandler = new TaggedLogHandler(color, tag);
        }

        public PumkinLogger(string colorHexOrName, string tag)
        {
            logHandler = new TaggedLogHandler(colorHexOrName, tag);
        }

        public bool IsLogTypeAllowed(LogType logType)
        {
            return Debug.unityLogger.IsLogTypeAllowed(logType);
        }

        public void Log(LogType logType, object message)
        {
            logHandler.LogFormat(logType, null, message as string, null);
        }

        public void Log(LogType logType, object message, Object context)
        {
            logHandler.LogFormat(logType, context, message as string, null);
        }

        public void Log(LogType logType, string tag, object message)
        {
            Debug.unityLogger.Log(logType, tag, message);
        }

        public void Log(LogType logType, string tag, object message, Object context)
        {
            Debug.unityLogger.Log(logType, tag, message, context);
        }

        public void Log(object message)
        {
            logHandler.LogFormat(LogType.Log, null, message as string, null);
        }

        public void Log(string tag, object message)
        {
            Debug.unityLogger.Log(LogType.Log, tag, message);
        }

        public void Log(string tag, object message, Object context)
        {
            Debug.unityLogger.Log(LogType.Log, tag, message, context);
        }

        public void LogWarning(string tag, object message)
        {
            Debug.unityLogger.Log(LogType.Warning, tag, message);
        }

        public void LogWarning(string tag, object message, Object context)
        {
            Debug.unityLogger.Log(LogType.Warning, tag, message, context);
        }

        public void LogError(string tag, object message)
        {
            Debug.unityLogger.Log(LogType.Error, tag, message);
        }

        public void LogError(string tag, object message, Object context)
        {
            Debug.unityLogger.Log(LogType.Error, tag, message, context);
        }

        public void LogFormat(LogType logType, string format, params object[] args)
        {
            logHandler.LogFormat(logType, null, format, args);
        }

        public void LogException(Exception exception)
        {
            logHandler.LogException(exception, null);
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            logHandler.LogFormat(logType, context, format, args);
        }

        public void LogException(Exception exception, Object context)
        {
            logHandler.LogException(exception, context);
        }
    }
}
#endif