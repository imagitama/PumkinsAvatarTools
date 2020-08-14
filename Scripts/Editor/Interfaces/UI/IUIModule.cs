#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.UnityTools.Interfaces
{
    public interface IUIModule
    {
        string Name { get; set; }
        string Description { get; set; }        
        string GameConfigurationString { get; set; }
        bool IsExpanded { get; set; }        
        GUIContent LabelContent { get; set; }
        List<IUIModule> ChildModules { get; set; }
        List<ISubTool> SubTools { get; set; }        
        int OrderInUI { get; set; }
        bool IsHidden { get; set; }

        void Draw();

        void OrderSubTools();
        void Start();
    }
}
#endif