using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Modules
{
    [AllowAutoLoad]
    [ModuleID("main_test")]
    class TestModule : UIModuleBase
    {        
        public TestModule()
        {
            Name = "Test Module";
            Description = "Debug and test stuff";
            OrderInUI = 100;
        }

        public override void Draw()
        {
            base.Draw();

            if(!IsExpanded)
                return;

            EditorGUILayout.LabelField("This is the test module!");

            EditorGUILayout.Space();

            if(GUILayout.Button("Remove reset transforms from tools"))
                AvatarToolsWindow.UI.FindModule("tools")?.SubTools.RemoveAll(s => string.Equals(s.Name, "reset transforms", System.StringComparison.InvariantCultureIgnoreCase));

            if(GUILayout.Button("Remove tools module"))
                AvatarToolsWindow.UI.RemoveModule("tools");

            if(GUILayout.Button("Build UI"))
                AvatarToolsWindow.UI = UIBuilder.BuildUI();
        }
    }
}
