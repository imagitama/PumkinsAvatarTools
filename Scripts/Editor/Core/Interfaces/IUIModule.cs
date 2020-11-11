#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Pumkin.AvatarTools.Interfaces
{
    public interface IUIModule
    {
        string Name { get; set; }
        string Description { get; set; }
        string GameConfigurationString { get; set; }
        bool IsExpanded { get; set; }
        GUIContent GUIContent { get; set; }
        List<IUIModule> ChildModules { get; set; }
        List<IItem> SubItems { get; set; }
        int OrderInUI { get; set; }
        bool IsHidden { get; set; }

        void DrawUI();

        void OrderChildren();

        void Start();

        bool ReplaceSubItem(IItem newItem, IItem oldItem);
    }
}
#endif