using System.Collections.Generic;

namespace Pumkin.UnityTools.Interfaces.Settings
{
    interface ISettingsContainer
    {
        bool LoadFromConfigFile(string filePath);
        bool SaveToConfigFile(string filePath);
    }
}