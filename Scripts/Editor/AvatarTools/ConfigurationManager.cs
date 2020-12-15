using Pumkin.AvatarTools2.Core;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools2
{
    static class ConfigurationManager
    {
        public const string DEFAULT_CONFIGURATION = "General";
        static string[] _configurations;
        static string _configurationString;

        public static event Delegates.StringChangeHandler OnConfigurationChanged;

        public static string[] Configurations
        {
            get
            {
                RefreshConfigurations();
                return _configurations;
            }
            set => _configurations = value;
        }
        public static string CurrentConfigurationString
        {
            get => _configurationString;
            set
            {
                string newValue = string.IsNullOrWhiteSpace(value) ? DEFAULT_CONFIGURATION : value;
                if(_configurationString != newValue)
                {
                    _configurationString = newValue;
                    OnConfigurationChanged?.Invoke(newValue);
                }
            }
        }
        public static int CurrentConfigurationIndex
        {
            get
            {
                int index = Array.IndexOf(Configurations, CurrentConfigurationString);
                return index > 0 ? index : 0;
            }
        }

        static ConfigurationManager()
        {
            RefreshConfigurations();

            PumkinToolsWindow.OnWindowEnabled -= OnEnable;
            PumkinToolsWindow.OnWindowEnabled += OnEnable;

            PumkinToolsWindow.OnWindowDisabled -= OnDisabled;
            PumkinToolsWindow.OnWindowDisabled += OnDisabled;
        }

        private static void OnDisabled()
        {
            PrefManager.SetString("configurationString", CurrentConfigurationString);
        }

        private static void OnEnable()
        {
            CurrentConfigurationString = PrefManager.GetString("configurationString", DEFAULT_CONFIGURATION);
        }

        public static void RefreshConfigurations()
        {
            var conf = new HashSet<string>();
            var cache = TypeHelpers.GetTypesWithAttribute<AutoLoadAttribute>().ToList();

            var configCache = cache
                .SelectMany(t => t.GetCustomAttribute<AutoLoadAttribute>().ConfigurationStrings)
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToArray();

            Configurations = configCache;
        }
    }
}
