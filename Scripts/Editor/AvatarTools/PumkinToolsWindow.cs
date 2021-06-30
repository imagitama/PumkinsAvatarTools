using Pumkin.AvatarTools2.UI;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pumkin.AvatarTools2
{
    [CanEditMultipleObjects]
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
            ConfigurationManager.OnConfigurationChanged -= ConfigurationChanged;
            ConfigurationManager.OnConfigurationChanged += ConfigurationChanged;
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
            guiSkin = Resources.Load<GUISkin>("UI/PumkinToolsGUISkin");
            OnWindowEnabled?.Invoke();
        }

        void OnDisable()
        {
            OnWindowDisabled?.Invoke();
        }

        void OnDestroy()
        {
            OnWindowDestroyed?.Invoke();
        }

        void OnGUI()
        {
            GUISkin oldSkin = null;
            if(guiSkin != null)
            {
                oldSkin = GUI.skin;
                GUI.skin = guiSkin;
            }

            if(!UI)
                RebuildUI();

            UI.Draw();

            if(oldSkin != null)
                GUI.skin = oldSkin;
        }

        void ConfigurationChanged(string newString)
        {
            if(UI != null)
                RebuildUI(newString);
        }

        internal static void RebuildUI()
        {
            RebuildUI(ConfigurationManager.CurrentConfigurationString);
        }

        static void RebuildUI(string newConfig)
        {
            if(UIBuilder.BuildUI(out MainUI tempUI))
                UI = tempUI ?? UI;
        }
    }
}