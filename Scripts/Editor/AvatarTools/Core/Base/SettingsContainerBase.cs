#if UNITY_EDITOR
using Pumkin.AvatarTools2.Interfaces;
using System;
using Pumkin.Core.Extensions;
using UnityEditor;
using UnityEngine;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core.Helpers;
using System.IO;
using System.Linq;
using System.Reflection;
using Pumkin.Core;

namespace Pumkin.AvatarTools2.Settings
{
    /// <summary>
    /// Settings container for IItem. Note: All settings containers MUST be in their own files to work
    /// </summary>
    [Serializable]
    public class SettingsContainerBase : ScriptableObject, ISettingsContainer
    {
        const string FOLDER_NAME = "Configs";

        static Type genericInspectorType;
        static Type defaultSettingsEditorType;

        string SavePath
        {
            get
            {
                if(_savePath == null)
                    _savePath = $"{SaveFolder}{GetType().Name}.json";
                return _savePath;
            }
        }

        string SaveFolder
        {
            get
            {
                if(_saveFolder == null)
                {
                    string config = null;
                    var attr = GetType().GetCustomAttribute<CustomSettingsContainerAttribute>();
                    if(attr != null)
                    {
                        var ownerAutoLoad = attr.OwnerType.GetCustomAttribute<AutoLoadAttribute>();
                        if(ownerAutoLoad != null)
                            config = ownerAutoLoad.ConfigurationStrings.FirstOrDefault();
                    }
                    config = string.IsNullOrWhiteSpace(config) ? ConfigurationManager.DEFAULT_CONFIGURATION : config;
                    _saveFolder = $"{SettingsManager.SettingsPath}{FOLDER_NAME}/{config}/";
                }
                return _saveFolder;
            }
        }

        string JSONHeader
        {
            get
            {
                if(_header == null)
                    _header = $"//{GetType().FullName}";
                return _header;
            }
        }

        public Editor Editor
        {
            get
            {
                if(_editor == null)
                {
                    //Create a custom editor, if it's a generic one replace it with a SettingsEditor
                    _editor = Editor.CreateEditor(this);
                    if(_editor.GetType() == genericInspectorType)
                        _editor = Editor.CreateEditor(this, defaultSettingsEditorType);
                }
                return _editor;
            }
        }

        public void Awake()
        {
            Editor.OnInspectorGUINoScriptField();   //Try to draw so it initializes and opens instantly later

            if(genericInspectorType == null)
                genericInspectorType = TypeHelpers.GetTypeAnywhere("UnityEditor.GenericInspector");
            if(defaultSettingsEditorType == null)
                defaultSettingsEditorType = typeof(SettingsEditor);

            SettingsManager.SaveSettingsCallback -= SettingsManager_SaveSettingsCallback;
            SettingsManager.SaveSettingsCallback += SettingsManager_SaveSettingsCallback;
            LoadFromConfigFile(SavePath);
        }

        private void SettingsManager_SaveSettingsCallback()
        {
            SaveToConfigFile(SavePath);
        }

        public void DrawUI()
        {
            Editor?.OnInspectorGUI();
        }

        public bool SaveToConfigFile(string filePath)
        {
            Directory.CreateDirectory(SaveFolder);
            try
            {
                string json = JsonUtility.ToJson(this, true);
                if(json == "{}")
                {
                    PumkinTools.LogVerbose($"Json for <b>{GetType().Name} is empty. Ignoring</b>");
                    return false;
                }

                json = JSONHeader + '\n' + JsonUtility.ToJson(this, true);
                File.WriteAllText(SavePath, json);
            }
            catch(Exception e)
            {
                PumkinTools.LogException(e);
                return false;
            }
            return true;
        }

        public bool LoadFromConfigFile(string filePath)
        {
            try
            {
                if(!File.Exists(SavePath))
                    return false;

                var lines = File.ReadAllLines(SavePath);
                if(lines.Length == 0)
                    return false;

                if(lines[0] != JSONHeader)
                {
                    PumkinTools.LogVerbose($"Trying to load settings for {GetType().Name} but the file header is not valid for this container");
                    return false;
                }

                string json = string.Join(Environment.NewLine,
                    lines[0].StartsWith("//") ? lines.Skip(1).ToArray() : lines);

                JsonUtility.FromJsonOverwrite(json, this);
                Editor.OnInspectorGUI();    //refresh settings

            }
            catch(Exception e)
            {
                PumkinTools.LogException(e);
                if(File.Exists(SavePath))
                    File.Delete(SavePath);
                return false;
            }
            return true;
        }

        protected Editor _editor;
        private string _savePath;
        private string _header;
        private string _saveFolder;
    }
}
#endif