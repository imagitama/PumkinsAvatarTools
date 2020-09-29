#if UNITY_EDITOR
using Pumkin.AvatarTools.Core;
using Pumkin.AvatarTools.Logger;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

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

        public static ILogHandler LogHandler { get; set; } = new LogHandler();

        //Temporary logger stuff
        public static void Log(string msg, UnityEngine.Object context)
        {
            LogHandler.LogFormat(LogType.Log, context, msg, new string[0]);
        }

        public static void Log(string msg)
        {
            LogHandler.LogFormat(LogType.Log, null, msg, new string[0]);
        }

        public static void LogWarning(string msg)
        {
            LogHandler.LogFormat(LogType.Warning, null, msg, new string[0]);
        }

        public static void LogWarning(string msg, UnityEngine.Object context)
        {
            LogHandler.LogFormat(LogType.Warning, context, msg, new string[0]);
        }

        public static void LogError(string msg)
        {
            LogHandler.LogFormat(LogType.Error, null, msg, new string[0]);
        }

        public static void LogError(string msg, UnityEngine.Object context)
        {
            LogHandler.LogFormat(LogType.Error, context, msg, new string[0]);
        }
    }
}
#endif