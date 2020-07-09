using Pumkin.AvatarTools.Implementation.Tools;
using Pumkin.AvatarTools.Implementation.Tools.SubTools;
using Pumkin.AvatarTools.Interfaces;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools
{
    class AvatarToolsWindow : EditorWindow
    {
        static List<IMainMenuModule> MainMenuModules = new List<IMainMenuModule>();

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
            MainMenuModules = new List<IMainMenuModule>()
            {
                new ToolsModule
                (
                    new List<ISubTool>()
                    {
                        new SetupLipsync(),
                        new ResetPose(),
                        new RevertBlendshapes(),
                    }
                )
            };
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();

            AvatarTools.SelectedAvatar = EditorGUILayout.ObjectField("Avatar", AvatarTools.SelectedAvatar, typeof(GameObject), true) as GameObject;
            
            EditorGUILayout.Space();
            
            foreach(var mod in MainMenuModules)
            {
                mod.Draw();
            }
        }
    }
}
