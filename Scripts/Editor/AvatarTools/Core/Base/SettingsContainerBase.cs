#if UNITY_EDITOR
using Pumkin.AvatarTools2.Interfaces;
using System;
using Pumkin.Core.Extensions;
using UnityEditor;
using UnityEngine;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core.Helpers;
using System.IO;

namespace Pumkin.AvatarTools2.Settings
{
    [Serializable]
    public class SettingsContainerBase : ScriptableObject, ISettingsContainer
    {
        static Type genericInspectorType;
        static Type defaultEditorType;

        const string RESOURCE_FOLDER_PREFIX = "Pumkin/Settings/";

        string SavePath
        {
            get
            {
                if(_savePath == null)
                    _savePath = $"{PumkinTools.ResourceFolderPath}/{RESOURCE_FOLDER_PREFIX}{GetType().Name}.json";
                return _savePath;
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
        }

        public void DrawUI()
        {
            Editor.OnInspectorGUI();
        }

        public bool SaveToConfigFile(string filePath)
        {
            try
            {
                string json = JsonUtility.ToJson(this, true);
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

                string json = File.ReadAllText(SavePath);
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
    }
}
#endif