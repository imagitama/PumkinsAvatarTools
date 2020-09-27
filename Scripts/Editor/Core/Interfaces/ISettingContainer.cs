#if UNITY_EDITOR
using UnityEditor;

namespace Pumkin.AvatarTools.Interfaces
{
    public interface ISettingsContainer
    {
        Editor Editor { get; }
        bool LoadFromConfigFile(string filePath);
        bool SaveToConfigFile(string filePath);
    }
}
#endif