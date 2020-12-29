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

namespace Pumkin.AvatarTools2.Settings
{
    [Serializable]
    public class SettingsContainerBase : ScriptableObject, ISettingsContainer
    {
        const string MAIN_FOLDER_SUFFIX = "Settings/";

        static Type genericInspectorType;
        static Type defaultEditorType;

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
                    _saveFolder = $"{PumkinTools.MainFolderPath}/{MAIN_FOLDER_SUFFIX}";
                return _saveFolder;
            }
        }

        string Header
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
                // Creates editors for everything, then replaces default editors with default SettingsEditors. Don't like it, but it will do for now.
                // NOTE: Potential crashes here
                if(genericInspectorType == null)
                    genericInspectorType = TypeHelpers.GetTypeAnwhere("UnityEditor.GenericInspector");
                if(defaultEditorType == null)
                    defaultEditorType = typeof(SettingsEditor);

                if(!_editor || (_editor && _editor.serializedObject == null))
                {
                    _editor = Editor.CreateEditor(this);
                    if(_editor.GetType() == genericInspectorType)
                        _editor = Editor.CreateEditor(this, defaultEditorType);
                }
                return _editor;
            }
        }

        public void Awake()
        {
            LoadFromConfigFile(SavePath);
            Editor.OnInspectorGUINoScriptField();   //Try to draw so it initializes and opens instantly later
            PumkinToolsWindow.OnWindowDisabled += PumkinToolsWindow_OnWindowDisabled;
        }

        private void PumkinToolsWindow_OnWindowDisabled()
        {
            SaveToConfigFile(SavePath);
        }

        private void OnDisable()
        {
            Debug.Log(GetType().Name + " " + "Called OnDisable()");
            //SaveToConfigFile(SavePath);
        }

        public void DrawUI()
        {
            Editor?.OnInspectorGUI();
        }

        public bool SaveToConfigFile(string filePath)
        {
            if(!Directory.Exists(SaveFolder))
                Directory.CreateDirectory(SaveFolder);

            try
            {
                string json = Header + '\n' + JsonUtility.ToJson(this, true);
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

                if(lines[0] != Header)
                {
                    PumkinTools.LogVerbose($"Trying to load settings for {GetType().Name} but the file header is not valid for this container");
                    return false;
                }

                string json = string.Join(Environment.NewLine,
                    lines[0].StartsWith("//") ? lines.Skip(1).ToArray() : lines);

                JsonUtility.FromJsonOverwrite(json, this);
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

        private void OnDestroy()
        {
            SaveToConfigFile(SavePath);
        }

        protected Editor _editor;
        private string _savePath;
        private string _header;
        private string _saveFolder;
    }
}
#endif