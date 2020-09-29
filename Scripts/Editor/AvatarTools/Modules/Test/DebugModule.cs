#if UNITY_EDITOR && PUMKIN_DEV
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using UnityEngine;

namespace Pumkin.AvatarTools.Modules
{
    [AutoLoad(DefaultModuleIDs.DEBUG, "debug")]
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
                PumkinToolsWindow.UI.FindModule("tools")?.SubItems.RemoveAll(s => string.Equals(s.Name, "reset transforms", System.StringComparison.InvariantCultureIgnoreCase));

            if(GUILayout.Button("Remove tools module"))
                PumkinToolsWindow.UI.RemoveModule("tools");

            if(GUILayout.Button("Build UI"))
                PumkinToolsWindow.UI = UIBuilder.BuildUI();

            if(GUILayout.Button("Dump Default GUISkin"))
                DebugHelpers.DumpDefaultGUISkin();
        }
    }
}
#endif