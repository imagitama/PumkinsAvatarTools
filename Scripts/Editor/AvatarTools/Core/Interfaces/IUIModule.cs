using Pumkin.Core.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Pumkin.AvatarTools2.Interfaces
{
    public interface IUIModule
    {
        string GameConfigurationString { get; set; }
        GUIContent GUIContent { get; set; }
        List<IUIModule> ChildModules { get; set; }
        List<IItem> SubItems { get; set; }
        UIDefinition UIDefs { get; set; }

        void DrawUI();

        void OrderChildren();

        void Start();

        bool ReplaceSubItem(IItem newItem, IItem oldItem);
    }
}