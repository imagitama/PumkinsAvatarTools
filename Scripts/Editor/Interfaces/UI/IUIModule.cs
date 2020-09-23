#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        List<ISubItem> SubItems { get; set; }        
        int OrderInUI { get; set; }
        bool IsHidden { get; set; }

        void Draw();

        void OrderChildren();
        void Start();
    }
}
#endif