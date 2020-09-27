#if UNITY_EDITOR

using UnityEngine;

namespace Pumkin.AvatarTools.Interfaces
{
    public interface IItem
    {
        /// <summary>
        /// The name of the tool. Used in the UI. Can be overriden by adding the [UIDefinition] attribute to the class
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The name of the tool. Used in the UI. Can be overriden by adding the [UIDefinition] attribute to the class
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Name of the game configuration to look for. Must match a value in AvatarTools.GameConfiguration enum.
        /// For example: "VRChat" will only work in vrchat, "All" or "Generic" will work everywhere
        /// Can be overriden by adding the [UIDefinition] attribute to the class
        /// </summary>
        string GameConfigurationString { get; set; }

        /// <summary>
        /// The order of which this will tool will be drawn in the UI. Can be overriden by adding the [UIDefinition] attribute to the class
        /// </summary>
        int OrderInUI { get; set; }

        /// <summary>
        /// Draws the UI
        /// </summary>
        /// <param name="rect">Optional rect to draw in</param>
        void DrawUI(params GUILayoutOption[] options);

        /// <summary>
        /// Label content
        /// </summary>
        GUIContent Content { get; }
        ISettingsContainer Settings { get; }
    }
}
#endif