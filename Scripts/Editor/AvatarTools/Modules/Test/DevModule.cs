#if UNITY_EDITOR && PUMKIN_DEV
using Pumkin.AvatarTools2.Settings;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using UnityEngine;

namespace Pumkin.AvatarTools2.Modules
{
    [AutoLoad(DefaultIDs.Modules.Debug, "Dev")]
    class DevModule : UIModuleBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Dev", "Debug and test stuff", 100);

        public override void DrawContent()
        {
            base.DrawContent();

            if(GUILayout.Button("Remove reset transforms from tools"))
                PumkinToolsWindow.UI.FindModule("tools")?.SubItems.RemoveAll(s =>
                string.Equals(s.UIDefs.Name, "reset transforms", System.StringComparison.OrdinalIgnoreCase));

            if(GUILayout.Button("Remove tools module"))
                PumkinToolsWindow.UI.RemoveModule("tools");

            //if(GUILayout.Button("Build UI"))
            //    if(UIBuilder.BuildUI(out MainUI ui))
            //        PumkinToolsWindow.UI = ui;

            if(GUILayout.Button("Dump Default GUISkin"))
                DevHelpers.DumpDefaultGUISkin();

            if(GUILayout.Button("Log current pose muscles"))
            {
                var muscles = DevHelpers.GetHumanMusclesFromCurrentPose(PumkinTools.SelectedAvatar);
                string s = "float[] pose = \n{";
                for(int i = 0; i < muscles.Length; i++)
                {
                    s += $"{muscles[i]}f {((i != muscles.Length - 1) ? ", " : "")}";
                    s += i % 13 == 0 ? "\n" : "";
                }
                s += "\n};";
                Debug.Log(s);
            }

            if(GUILayout.Button("Test dynamic bones in project"))
                Debug.Log(TypeHelpers.GetTypeAnwhere("DynamicBone")?.FullName ?? "Not here");

            if(GUILayout.Button("Get Main Script Path"))
                Debug.Log(PumkinTools.MainFolderPath);

            if(GUILayout.Button("Log verbose"))
                PumkinTools.LogVerbose("Hello!", LogType.Warning);

            if(GUILayout.Button("Print settings containers"))
                foreach(var s in GameObject.FindObjectsOfType<SettingsContainerBase>())
                    Debug.Log(s.GetType().Name);
        }
    }
}
#endif