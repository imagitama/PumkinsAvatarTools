#if UNITY_EDITOR
using Pumkin.AvatarTools2.Interfaces;
using System;
using Pumkin.Core.Extensions;
using UnityEditor;
using UnityEngine;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core.Helpers;

namespace Pumkin.AvatarTools2.Settings
{
    [Serializable]
    public class SettingsContainerBase : ScriptableObject, ISettingsContainer
    {
        static Type genericInspectorType;
        static Type defaultEditorType;

        public Editor Editor
        {
            get
            {
                // Creates editors for everything, then replaces default editors with default SettingsEditors. Don't like it, but it will do for now.
                // TODO: Potential crashes here
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
            Editor.OnInspectorGUINoScriptField();   //Try to draw so it initializes and opens instantly later
        }

        public void DrawUI()
        {
            Editor.OnInspectorGUI();
        }

        public bool SaveToConfigFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public bool LoadFromConfigFile(string filePath)
        {
            throw new NotImplementedException();
        }

        protected Editor _editor;
    }
}
#endif