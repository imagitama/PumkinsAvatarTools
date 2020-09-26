#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Helpers;
using Pumkin.AvatarTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools
{
    static class PumkinTools
    {
        public delegate void AvatarChangeHandler(GameObject selection);
        public static event AvatarChangeHandler AvatarSelectionChanged;

        public const string DEFAULT_CONFIGURATION = "generic";

        static GameObject _selectedAvatar;
        static string _configurationString = DEFAULT_CONFIGURATION;
        static string[] _configurations;

        static PumkinTools()
        {
            RefreshConfigurations();
        }

        public static string[] Configurations
        {
            get
            {
                //if(_configurations == null)
                    RefreshConfigurations();
                return _configurations;
            }
            set => _configurations = value;
        }
        public static string ConfigurationString
        {
            get => _configurationString;
            set
            {
                _configurationString = string.IsNullOrWhiteSpace(value) ? "generic" : value;
                AvatarToolsWindow.UI = UIBuilder.BuildUI();
            }
        }

        public static void RefreshConfigurations()
        {
            var conf = new HashSet<string>();
            var cache = TypeHelpers.GetTypesWithAttribute<AutoLoadAttribute>().ToList();

            var configCache = cache.SelectMany(t => t.GetCustomAttribute<AutoLoadAttribute>().ConfigurationStrings).ToList();
            Configurations = new HashSet<string>(configCache).ToArray();
        }

        public static GameObject SelectedAvatar
        {
            get => _selectedAvatar;
            set
            {
                if(_selectedAvatar != value)
                    OnAvatarSelectionChanged(_selectedAvatar);
                _selectedAvatar = value;
            }
        }

        public static void OnAvatarSelectionChanged(GameObject newSelection)
        {
            AvatarSelectionChanged?.Invoke(newSelection);
        }

        public static ILogHandler LogHandler { get; set; } = new Implementation.Logging.LogHandler();


        //Temporary logger functions
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