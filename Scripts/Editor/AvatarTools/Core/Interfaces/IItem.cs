#if UNITY_EDITOR

using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools2.Interfaces
{
    public interface IItem
    {
        /// <summary>
        /// Name of the game configuration to look for. Must match a value in AvatarTools.GameConfiguration enum.
        /// For example: "VRChat" will only work in vrchat, "All" or "Generic" will work everywhere
        /// Can be overriden by adding the [UIDefinition] attribute to the class
        /// </summary>
        string GameConfigurationString { get; set; }

        /// <summary>
        /// Label content
        /// </summary>
        GUIContent Content { get; }

        /// <summary>
        /// Draws the UI
        /// </summary>
        /// <param name="options"></param>
        void DrawUI(params GUILayoutOption[] options);

        ISettingsContainer Settings { get; }

        bool EnabledInUI { get; set; }

        UIDefinition UIDefs { get; set; }
    }
}
#endif