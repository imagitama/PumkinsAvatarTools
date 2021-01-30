using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools2.Interfaces
{
    public interface IItem
    {
        /// <summary>
        /// Label content
        /// </summary>
        GUIContent Content { get; }

        /// <summary>
        /// UI Definition containing all the UI related stuff
        /// </summary>
        UIDefinition UIDefs { get; set; }

        /// <summary>
        /// Draws the UI
        /// </summary>
        /// <param name="options"></param>
        void DrawUI(params GUILayoutOption[] options);

        ISettingsContainer Settings { get; }

        bool EnabledInUI { get; set; }
    }
}