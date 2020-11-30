#if UNITY_EDITOR
using Pumkin.AvatarTools.Interfaces;
using System;
using Pumkin.Core.Extensions;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;

namespace Pumkin.AvatarTools.Base
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
            Editor.OnInspectorGUINoScriptField();   //Try to draw to initialize it so it opens instantly later
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