#if UNITY_EDITOR
using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Helpers;
using Pumkin.UnityTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.UnityTools
{
    static class PumkinTools
    {
        public delegate void AvatarChangeHandler(GameObject selection);
        public static event AvatarChangeHandler AvatarSelectionChanged;

        static GameObject _selectedAvatar;
        static string _configurationString = "generic";
        static string[] _configurations;

        static PumkinTools()
        {
            RefreshConfigurations();
        }

        public static string[] Configurations 
        { 
            get
            {
                if(_configurations == null)
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
            var cache = TypeHelpers.GetTypesWithAttribute<AutoLoadAttribute>()
                .Select(t => t.GetCustomAttribute<AutoLoadAttribute>().ConfigurationString);
            Configurations = new HashSet<string>(cache).ToArray();
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

        public static void Log(string msg, UnityEngine.Object context)
        {
            LogHandler.LogFormat(LogType.Log, context, msg, new string[0]);
        }        
    }
}
#endif