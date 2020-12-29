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
            private set
            {
                _configurationString = ValidateConfigurationString(value);

                PrefManager.SetString("configurationString", _configurationString);
                _configurationIndex = Array.IndexOf(Configurations, _configurationString);

                try
                {
                    OnConfigurationChanged?.Invoke(_configurationString);
                }
                catch(Exception ex)
                {
                    PumkinTools.LogException(ex);
                }
            }
        }

        public static int CurrentConfigurationIndex
        {
            get
            {
                if(_configurationIndex == null)
                    _configurationIndex = Array.IndexOf(Configurations, CurrentConfigurationString);
                return _configurationIndex > 0 ? (int)_configurationIndex : 0;
            }
            set
            {
                if(CurrentConfigurationIndex == value)
                    return;

                CurrentConfigurationString = Configurations[value];
                PrefManager.SetString("configurationString", CurrentConfigurationString);
                OnConfigurationChanged?.Invoke(CurrentConfigurationString);
            }
        }

        static ConfigurationManager()
        {
            RefreshConfigurations();

            PumkinToolsWindow.OnWindowEnabled -= OnEnable;
            PumkinToolsWindow.OnWindowEnabled += OnEnable;
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
                .OrderBy(s => s != DEFAULT_CONFIGURATION)
                .ThenBy(s => s)
                .ToArray();

            Configurations = configCache;
        }

        public static string ValidateConfigurationString(string newConfigString)
        {
            return string.IsNullOrWhiteSpace(newConfigString) ? DEFAULT_CONFIGURATION : newConfigString;
        }

        static string[] _configurations;
        static string _configurationString;
        static int? _configurationIndex;
    }
}
