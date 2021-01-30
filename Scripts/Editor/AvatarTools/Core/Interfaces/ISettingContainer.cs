using UnityEditor;

namespace Pumkin.AvatarTools2.Interfaces
{
    public interface ISettingsContainer
    {
        Editor Editor { get; }
        bool LoadFromConfigFile(string filePath);
        bool SaveToConfigFile(string filePath);

        void DrawUI();
    }
}