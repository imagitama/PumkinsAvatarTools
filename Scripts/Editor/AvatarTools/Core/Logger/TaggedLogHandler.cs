using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pumkin.Core.Logger
{
    internal class TaggedLogHandler : ILogHandler
    {
        string color;
        string tag;

        TaggedLogHandler() {}

        public TaggedLogHandler(Color color, string tag)
        {
            this.color = color.ToString();
            this.tag = tag;
        }

        public TaggedLogHandler(string colorHexOrName, string tag)
        {
            color = colorHexOrName;
            this.tag = tag;
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            string taggedFormat = $"<color={color}>{tag}</color>: {format}";
            switch(logType)
            {
                case LogType.Error:
                    Debug.LogErrorFormat(context, taggedFormat, args);
                    break;
                case LogType.Warning:
                    Debug.LogWarningFormat(context, taggedFormat, args);
                    break;
                case LogType.Assert:
                    Debug.LogAssertionFormat(context, taggedFormat, args);
                    break;
                default:
                    Debug.LogFormat(context, taggedFormat, args);
                    break;
            }
        }

        public void LogException(Exception exception, Object context)
        {
            Debug.LogException(exception, context);
        }
    }
}