#if UNITY_EDITOR
using Pumkin.AvatarTools2.UI;
using System;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2
{
    [ExecuteInEditMode, CanEditMultipleObjects]
    public class PumkinToolsWindow : EditorWindow
    {
        public static MainUI UI { get; set; }
        static Vector2 minWindowSize = new Vector2(300,400);
        static GUISkin guiSkin;

        public static event Action OnWindowEnabled;
        public static event Action OnWindowDisabled;
        public static event Action OnWindowDestroyed;

        public PumkinToolsWindow()
        {
            ConfigurationManager.OnConfigurationChanged += RebuildUI;
        }

        private void RebuildUI(string newConfig)
        {
            UI = UIBuilder.BuildUI();
        }

        [MenuItem("Pumkin/Tools/Avatar Tools 2", false, 0)]
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
            //UI build gets called by configuration being loaded
            guiSkin = Resources.Load<GUISkin>("UI/PumkinToolsGUISkin");
            OnWindowEnabled?.Invoke();
        }

        void OnDisable()
        {
            OnWindowDisabled?.Invoke();
        }

        private void OnDestroy()
        {
            OnWindowDestroyed?.Invoke();
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