#if UNITY_EDITOR
using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.Modules;
using Pumkin.Core.Extensions;
using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.UI
{
    /// <summary>
    /// Main UI class, responsible for drawing everything
    /// </summary>
    public class MainUI
    {
        public List<IUIModule> UIModules = new List<IUIModule>();
        public List<UIModuleBase> mods = new List<UIModuleBase>();
        bool drawSettings = false;

        Vector2 scroll = Vector2.zero;

        GUIContent configLabel = new GUIContent(ConfigurationManager.CurrentConfigurationString);
        GUIContent versionLabel = new GUIContent($"Version {PumkinTools.version}{PumkinTools.versionSuffix}");

        public IUIModule OrphanHolder
        {
            get => _orphanHolder ?? (_orphanHolder = new OrphanHolderModule());
            private set => _orphanHolder = value;
        }

        IUIModule _orphanHolder;

        int selectedConfigIndex = 0;

        public MainUI()
        {
            selectedConfigIndex = ConfigurationManager.CurrentConfigurationIndex;
        }

        public MainUI(List<IUIModule> modules)
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
                EditorGUILayout.PrefixLabel(versionLabel, EditorStyles.label);
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(configLabel, Styles.RightAlignedLabel);
            }
            EditorGUILayout.EndHorizontal();

            UIHelpers.DrawLine();

            if(drawSettings)
            {
                DrawSettings();
                return;
            }

            PumkinTools.SelectedAvatar = EditorGUILayout.ObjectField("Avatar", PumkinTools.SelectedAvatar, typeof(GameObject), true) as GameObject;

            if(GUILayout.Button("Select from Scene"))
                PumkinTools.SelectedAvatar = Selection.activeGameObject ?? PumkinTools.SelectedAvatar;
            UIHelpers.DrawLine();

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

        private void DrawSettings()
        {
            EditorGUI.BeginChangeCheck();
            {
                selectedConfigIndex = EditorGUILayout.Popup("Configuration", selectedConfigIndex, ConfigurationManager.Configurations);
            }
            if(EditorGUI.EndChangeCheck())
            {
                ConfigurationManager.CurrentConfigurationString = ConfigurationManager.Configurations[selectedConfigIndex];
            }

            EditorGUILayout.Space();

            bool logVerbose = PumkinTools.VerboseLogger.logEnabled;
            EditorGUI.BeginChangeCheck();
            logVerbose = EditorGUILayout.ToggleLeft("Enable Verbose logging", logVerbose);

            if(EditorGUI.EndChangeCheck())
                PumkinTools.VerboseLogger.logEnabled = logVerbose;

            UIHelpers.DrawLine();

            Credits.DrawUI();
        }

        public IUIModule FindModule(string name)
        {
            return UIModules.FirstOrDefault(s => string.Equals(name, s.UIDefs.Name, StringComparison.InvariantCultureIgnoreCase));
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
            return UIModules.RemoveAll(m => string.Equals(m.UIDefs.Name, name, StringComparison.InvariantCultureIgnoreCase));
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
            return !ReferenceEquals(ui, null) &&
                (ui.UIModules.IsNullOrEmpty() && ui.OrphanHolder.ChildModules.IsNullOrEmpty());
        }
    }
}
#endif