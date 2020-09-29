#if UNITY_EDITOR
using Pumkin.AvatarTools.Interfaces;
using System;
using UnityEditor;
using UnityEngine;

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
                if(!editor)
                    editor = Editor.CreateEditor(this);
                return editor;
            }
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