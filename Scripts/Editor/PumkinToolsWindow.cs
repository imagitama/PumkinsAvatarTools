#if UNITY_EDITOR
using Pumkin.AvatarTools.UI;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools
{
    class PumkinToolsWindow : EditorWindow
    {
        public static MainUI UI { get; set; }
        static Vector2 minWindowSize = new Vector2(360,580);
        static GUISkin guiSkin;

        [MenuItem("Pumkin/Avatar Tools", false, 0)]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow editorWindow = GetWindow(typeof(PumkinToolsWindow));
            editorWindow.autoRepaintOnSceneChange = true;

            editorWindow.minSize = minWindowSize;

            editorWindow.Show();
            editorWindow.titleContent = new GUIContent("Pumkin Tools");
        }

        void OnEnable()
        {
            if(!UI)
                UI = UIBuilder.BuildUI();
            guiSkin = Resources.Load<GUISkin>("UI/PumkinToolsGUISkin");
        }

        private void OnGUI()
        {
            GUISkin oldSkin = null;
            if(guiSkin != null)
            {
                oldSkin = GUI.skin;
                GUI.skin = guiSkin;
            }

            UI?.Draw();

            if(oldSkin != null)
                GUI.skin = oldSkin;
        }
    }
}
#endif