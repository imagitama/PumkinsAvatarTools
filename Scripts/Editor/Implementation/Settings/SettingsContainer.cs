using Pumkin.UnityTools.Interfaces.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Settings
{
    class SettingsContainer : ISettingContainer
    {        
        //ISettingsContainer
        HashSet<ISetting> Settings { get; set; } = new HashSet<ISetting>();
        public int Count => Settings.Count;
        
        public SettingsContainer() {}

        public SettingsContainer(IEnumerable<ISetting> settings) =>
            Settings = new HashSet<ISetting>(settings);

        public SettingsContainer(string configFilePath) =>
             LoadFromConfigFile(configFilePath);
        

        public T GetSettingValue<T>(string settingName)
        {
            var set = Settings?.FirstOrDefault(s => s.Name.Equals(settingName, StringComparison.InvariantCultureIgnoreCase)) ?? null;
            return (T)set?.Value;            
        }

        public void RegisterSetting(ISetting setting)
        {
            if(Settings.FirstOrDefault(s => s.Name.Equals(setting.Name, StringComparison.InvariantCultureIgnoreCase)) == default)
                Settings.Add(setting);
            else
                Debug.LogWarning($"Setting {setting.Name} already exists");
        }

        public bool SaveToConfigFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public bool LoadFromConfigFile(string filePath)
        {            
            throw new NotImplementedException();
        }        

        public static explicit operator Dictionary<string, object>(SettingsContainer container) =>        
            container.Settings.ToDictionary(k => k.Name, v => v.Value);

        //IEnumerator
        public IEnumerator<ISetting> GetEnumerator()
        {
            return Settings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Settings.GetEnumerator();
        }

    }
}
