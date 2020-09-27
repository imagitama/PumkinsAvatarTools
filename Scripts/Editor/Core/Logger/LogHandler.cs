#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Logger
{
    class LogHandler : ILogHandler  //TODO: Implement ILogger properly
    {
        string prefix = "PumkinsAvatarTools";
        Color color = Color.yellow;

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            Debug.LogException(exception, context);
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            Debug.LogFormat(context, $"{prefix}: {format}", args);
        }

        //public ILogHandler logHandler { get; } = Debug.unityLogger.logHandler;
        //public bool logEnabled { get; set; } = Debug.unityLogger.logEnabled;
        //public LogType filterLogType { get; set; } = Debug.unityLogger.filterLogType;

        //public bool IsLogTypeAllowed(LogType logType)
        //{
        //    retun true;
        //}

        //public void Log(LogType logType, object message)
        //{
        //    string msg = $"{prefix}: {message}";
        //    switch(logType)
        //    {
        //        case LogType.Warning:
        //            Debug.LogWarning(msg);
        //            break;
        //        case LogType.Assert:
        //            Debug.LogAssertion(msg);
        //            break;
        //        case LogType.Error:
        //            Debug.LogError(msg);
        //            break;
        //        case LogType.Log:
        //        default:
        //            PumkinTools.Log(msg);
        //            break;
        //    }
        //}

        //public void Log(LogType logType, object message, UnityEngine.Object context)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Log(LogType logType, string tag, object message)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Log(LogType logType, string tag, object message, UnityEngine.Object context)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Log(object message)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Log(string tag, object message)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Log(string tag, object message, UnityEngine.Object context)
        //{
        //    throw new NotImplementedException();
        //}

        //public void LogError(string tag, object message)
        //{
        //    throw new NotImplementedException();
        //}

        //public void LogError(string tag, object message, UnityEngine.Object context)
        //{
        //    throw new NotImplementedException();
        //}

        //public void LogException(Exception exception)
        //{
        //    throw new NotImplementedException();
        //}

        //public void LogException(Exception exception, UnityEngine.Object context)
        //{
        //    throw new NotImplementedException();
        //}

        //public void LogFormat(LogType logType, string format, params object[] args)
        //{
        //    throw new NotImplementedException();
        //}

        //public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        //{
        //    throw new NotImplementedException();
        //}

        //public void LogWarning(string tag, object message)
        //{
        //    throw new NotImplementedException();
        //}

        //public void LogWarning(string tag, object message, UnityEngine.Object context)
        //{
        //    PumkinTools.Log();
        //}
    }
}
#endif