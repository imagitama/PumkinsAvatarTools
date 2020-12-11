#if UNITY_EDITOR
using Pumkin.AvatarTools2.Interfaces;
using System;
using Pumkin.Core.Extensions;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Settings
{
    [Serializable]
    public class SettingsContainerBase : ScriptableObject, ISettingsContainer
    {
        protected Editor editor;
        public Editor Editor
        {
            get
            {
                if(!editor || (editor && editor.serializedObject == null))
                    editor = Editor.CreateEditor(this);
                return editor;
            }
        }

        public void Awake()
        {
            Editor.OnInspectorGUINoScriptField();   //Try to draw so it initializes and opens instantly later
        }

        public bool SaveToConfigFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public bool LoadFromConfigFile(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
#endif