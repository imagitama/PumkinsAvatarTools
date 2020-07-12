using Pumkin.AvatarTools.Implementation.Tools;
using Pumkin.AvatarTools.Implementation.Tools.SubTools;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools
{
    class AvatarToolsWindow : EditorWindow
    {
        MainUI UI;

        [MenuItem("Pumkin/Avatar Tools", false, 0)]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow editorWindow = GetWindow(typeof(AvatarToolsWindow));
            editorWindow.autoRepaintOnSceneChange = true;

            editorWindow.Show();
            editorWindow.titleContent = new GUIContent("Pumkin Tools");
        }

        void OnEnable()
        {
            UI = UIBuilder.BuildUI();            
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();

            AvatarTools.SelectedAvatar = EditorGUILayout.ObjectField("Avatar", AvatarTools.SelectedAvatar, typeof(GameObject), true) as GameObject;
            
            EditorGUILayout.Space();

            if(UI != null)
                UI.Draw();

            EditorGUILayout.Space();

            if(GUILayout.Button("Remove reset pose"))
                UI.FindModule("tools")?.SubTools.RemoveAll(s => string.Equals(s.Name, "reset transforms", System.StringComparison.InvariantCultureIgnoreCase));
            
            if(GUILayout.Button("Remove tools module"))
                UI.RemoveModule("tools");
            
            if(GUILayout.Button("Build UI"))
                UI = UIBuilder.BuildUI();
        }
    }
}
