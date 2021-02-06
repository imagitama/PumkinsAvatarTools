using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.Modules;
using Pumkin.AvatarTools2.Settings;
using Pumkin.AvatarTools2.UI.Credits;
using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pumkin.AvatarTools2.UI
{
    /// <summary>
    /// Main UI class, responsible for drawing everything
    /// </summary>
    public class MainUI
    {
        public List<IUIModule> UIModules = new List<IUIModule>();
        bool drawSettings = false;

        Vector2 scroll = Vector2.zero;

        GUIContent configLabel = new GUIContent(ConfigurationManager.CurrentConfigurationString);
        GUIContent versionLabel = new GUIContent($"Version {PumkinTools.version}{PumkinTools.versionSuffix}");

        public IUIModule OrphanHolder
        {
            get => _orphanHolder ?? (_orphanHolder = new OrphanHolderModule());
            private set => _orphanHolder = value;
        }

        PumkinTools_Settings Settings
        {
            get
            {
                if(!_settings)
                    _settings = ScriptableObject.CreateInstance<PumkinTools_Settings>();
                return _settings;
            }
        }

        internal MainUI() { }

        internal MainUI(List<IUIModule> modules)
        {
            UIModules = modules;
        }

        public void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Pumkin's Avatar Tools", Styles.TitleLabel);
                if(GUILayout.Button(Icons.Settings, Styles.MediumIconButton))
                    drawSettings = !drawSettings;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorStyles.label.normal.background = null;

                UIHelpers.DrawTightLabel(versionLabel);
                GUILayout.FlexibleSpace();
                UIHelpers.DrawTightLabel(configLabel, Styles.RightAlignedLabel);
            }
            EditorGUILayout.EndHorizontal();

            UIHelpers.DrawLine();

            if(drawSettings)
            {
                DrawSettings();
                return;
            }

            //Select avatar
            {
                GameObject newAvatar = null;
                EditorGUI.BeginChangeCheck();

                newAvatar = EditorGUILayout.ObjectField("Avatar", PumkinTools.SelectedAvatar, typeof(GameObject), true) as GameObject;

                if(GUILayout.Button("Select from Scene"))
                    newAvatar = Selection.activeGameObject ?? PumkinTools.SelectedAvatar;
                UIHelpers.DrawLine();

                if(EditorGUI.EndChangeCheck())
                    PumkinTools.SelectedAvatar = newAvatar;
            }
            scroll = EditorGUILayout.BeginScrollView(scroll);

            //Draw modules
            foreach(var mod in UIModules)
            {
                if(mod != null)
                {
                    try
                    {
                        if(!mod.UIDefs.IsHidden)
                            mod.DrawUI();
                    }
                    catch(Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                else
                {
                    PumkinTools.Log($"'{mod}' is null");
                }
            }

            //Draw Orphan Holder module
            try
            {
                if(OrphanHolder != null)
                    if(!OrphanHolder.UIDefs.IsHidden)
                        OrphanHolder.DrawUI();
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }

            EditorGUILayout.EndScrollView();

        }

        void DrawSettings()
        {
            Settings?.DrawUI();

#if PUMKIN_DEV
            if(GUILayout.Button("Rebuild UI"))
                PumkinToolsWindow.RebuildUI();
#endif

            UIHelpers.DrawLine();
            thanksList.Draw();
            UIHelpers.DrawLine();
            linksList.Draw();
        }

        public IUIModule FindModule(string name)
        {
            return UIModules.FirstOrDefault(s => string.Equals(name, s.UIDefs.Name, StringComparison.OrdinalIgnoreCase));
        }

        public bool HasModule(IUIModule module)
        {
            return UIModules.Exists(m => m == module);
        }

        public void AddModule(IUIModule module)
        {
            if(module != null)
                UIModules.Add(module);
        }

        public bool RemoveModule(IUIModule module)
        {
            return UIModules.Remove(module);
        }

        public int RemoveModule(string name)
        {
            return UIModules.RemoveAll(m => string.Equals(m.UIDefs.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Orders modules by their OrderInUI value
        /// </summary>
        public void OrderModules()
        {
            UIModules = UIModules.OrderBy(m => m.UIDefs.OrderInUI).ToList();
            UIModules.ForEach(x => x.OrderChildren());
            OrphanHolder.OrderChildren();
        }

        /// <summary>
        /// Returns null if null or if has no modules
        /// </summary>
        /// <param name="ui"></param>
        public static implicit operator bool(MainUI ui)
        {
            return !ReferenceEquals(ui, null);
        }

        readonly CreditsList thanksList = new CreditsList(
            "Thanks to the following people for their help!",
            new NoteEntry("Xiexe", "Fallback shaders"),
            new NoteEntry("Dreadrith", "Reset pose to avatar definition")
        );

        readonly CreditsList linksList = new CreditsList(
            "Links",
            new URLEntry("VRWorld Toolkit", "https://github.com/oneVR/VRWorldToolkit"),
            new URLEntry("Poiyomi Shaders", "https://github.com/poiyomi/PoiyomiToonShader"),
            new URLEntry("Thry Editor", "https://thryeditor.thryrallo.de/")
        );


        IUIModule _orphanHolder;
        PumkinTools_Settings _settings;
    }
}