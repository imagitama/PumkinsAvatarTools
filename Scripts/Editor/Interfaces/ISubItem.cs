#if UNITY_EDITOR

namespace Pumkin.AvatarTools.Interfaces
{
    public interface ISubItem
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
        void DrawUI();
    }
}
#endif