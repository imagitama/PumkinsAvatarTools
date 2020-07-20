using System.Collections.Generic;

namespace Pumkin.UnityTools.Interfaces.Settings
{
    interface ISettingContainer : IEnumerable<ISetting>
    {
        T GetSettingValue<T>(string settingName);
        bool LoadFromConfigFile(string filePath);
        bool SaveToConfigFile(string filePath);
    }
}