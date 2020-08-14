#if UNITY_EDITOR
using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Modules
{    
    [AutoLoad("test", ConfigurationString = "debug")]
    class DebugModule : UIModuleBase
    {        
        public DebugModule()
        {
            Name = "Debug";
            Description = "Debug and test stuff";
            OrderInUI = 100;            
        }

        public override void DrawContent()
        {
            base.DrawContent();

            if(GUILayout.Button("Remove reset transforms from tools"))
                AvatarToolsWindow.UI.FindModule("tools")?.SubTools.RemoveAll(s => string.Equals(s.Name, "reset transforms", System.StringComparison.InvariantCultureIgnoreCase));

            if(GUILayout.Button("Remove tools module"))
                AvatarToolsWindow.UI.RemoveModule("tools");

            if(GUILayout.Button("Build UI"))
                AvatarToolsWindow.UI = UIBuilder.BuildUI();
        }
    }
}
#endif