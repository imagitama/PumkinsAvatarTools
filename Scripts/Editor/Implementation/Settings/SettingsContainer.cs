#if UNITY_EDITOR
using Pumkin.UnityTools.Interfaces.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Settings
{
    [Serializable]
    public abstract class SettingsContainer : ScriptableObject, ISettingsContainer
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