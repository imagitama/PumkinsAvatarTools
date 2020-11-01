#if UNITY_EDITOR
using System;
using Pumkin.AvatarTools.Core;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using UnityEditor;
using UnityEngine;
using Pumkin.Core.Logger;

namespace Pumkin.AvatarTools
{
    static class PumkinTools
    {
        public static event Delegates.SelectedChangeHandler OnAvatarSelectionChanged;

        static GameObject _selectedAvatar;

        static PumkinTools()
        {

        }

        public static GameObject SelectedAvatar
        {
            get => _selectedAvatar;
            set
            {
                if(_selectedAvatar != value)
                    OnAvatarSelectionChanged?.Invoke(value);
                _selectedAvatar = value;
            }
        }

        public static void AvatarSelectionChanged(GameObject newSelection)
        {
            OnAvatarSelectionChanged?.Invoke(newSelection);
        }


        //Logger stuff
        static PumkinLogger Logger { get; } = new PumkinLogger("blue", "Pumkin Tools");
        static PumkinLogger VerboseLogger { get; } = new PumkinLogger("red", "Pumkin Tools Verbose");

        public static void Log(string msg, UnityEngine.Object context)
        {
            Logger.LogFormat(LogType.Log, context, msg);
        }

        public static void Log(string msg)
        {
            Logger.LogFormat(LogType.Log, null, msg);
        }

        public static void LogWarning(string msg)
        {
            Logger.LogFormat(LogType.Warning, null, msg);
        }

        public static void LogWarning(string msg, UnityEngine.Object context)
        {
            Logger.LogFormat(LogType.Warning, context, msg);
        }

        public static void LogError(string msg)
        {
            Logger.LogFormat(LogType.Error, null, msg);
        }

        public static void LogError(string msg, UnityEngine.Object context)
        {
            Logger.LogFormat(LogType.Error, context, msg);
        }

        public static void LogVerbose(string msg, LogType logType = LogType.Log, UnityEngine.Object context = null)
        {
            VerboseLogger.LogFormat(logType, context, msg);
        }
    }
}
#endif