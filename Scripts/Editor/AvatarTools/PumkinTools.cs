#if UNITY_EDITOR
using System;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using UnityEditor;
using UnityEngine;
using Pumkin.Core.Logger;

namespace Pumkin.AvatarTools2
{
    public static class PumkinTools
    {
        public static event Delegates.SelectedChangeHandler OnAvatarSelectionChanged;

        static GameObject _selectedAvatar;

        static PumkinTools()
        {
            VerboseLogger.logEnabled = true;
        }

        public static GameObject SelectedAvatar
        {
            get => _selectedAvatar;
            set
            {
                GameObject newAvatar = value;
                if(_selectedAvatar != value)
                {
                    if(newAvatar != null)
                        newAvatar = newAvatar.transform.root.gameObject;
                    try
                    {
                        OnAvatarSelectionChanged?.Invoke(newAvatar);
                    }
                    catch(Exception e)
                    {
                        PumkinTools.LogException(e);
                    }
                }
                _selectedAvatar = newAvatar;
            }
        }

        public static void AvatarSelectionChanged(GameObject newSelection)
        {
            OnAvatarSelectionChanged?.Invoke(newSelection);
        }

        //Version
        public static Version version { get; } = new Version(2, 0);
        static bool isWipVersion = true;

        public static string versionSuffix
        {
            get
            {
                if(_versionSuffix == null)
                    _versionSuffix = isWipVersion ? " - WIP" : "";
                return _versionSuffix;
            }
        }
        static string _versionSuffix = null;

        //Logger stuff
        public static PumkinLogger Logger { get; } = new PumkinLogger("blue", "Pumkin Tools");
        public static PumkinLogger VerboseLogger { get; } = new PumkinLogger("red", "Pumkin Tools Verbose");

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

        public static void LogException(Exception e)
        {
            Logger.LogException(e);
        }
    }
}
#endif