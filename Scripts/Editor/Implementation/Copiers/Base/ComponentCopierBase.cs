#if UNITY_EDITOR
using Pumkin.Interfaces.ComponentCopier;
using Pumkin.UnityTools.Helpers;
using Pumkin.UnityTools.Implementation.Modules;
using Pumkin.UnityTools.Implementation.Settings;
using Pumkin.UnityTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Copiers
{
    abstract class ComponentCopierBase : IComponentCopier
    {
        GUIContent Content { get; set; }
        public string ComponentName { get; set; }

        public virtual SettingsContainer Settings { get => null; }
        public bool ExpandSettings { get; private set; }

        protected abstract bool DoCopy(GameObject objFrom, GameObject objTo);
        
        public virtual void DrawUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if(GUILayout.Button(Content, Styles.MediumButton))
                    TryCopyComponents(PumkinTools.SelectedAvatar, ComponentCopiersModule.CopyToAvatar);
                if(Settings)
                    if(GUILayout.Button(Icons.Settings, Styles.MediumIconButton))
                        ExpandSettings = !ExpandSettings;
            }
            EditorGUILayout.EndHorizontal();

            //Draw settings here            
            if(!Settings || !ExpandSettings)
                return;

            UIHelpers.VerticalBox(() =>
            {
                EditorGUILayout.Space();
                Settings.Editor.OnInspectorGUI();
            });
        }

        protected virtual bool Prepare(GameObject objFrom, GameObject objTo)
        {
            return false;
        }

        protected virtual void Finish(GameObject objFrom, GameObject objTo)
        {            
            PumkinTools.Log($"{ComponentName} copier completed successfully.");
        }

        public bool TryCopyComponents(GameObject objFrom, GameObject objTo)
        {
            try
            {
                if(Prepare(objFrom, objTo) && DoCopy(objFrom, objTo))
                {                    
                    Finish(objFrom, objTo);
                    return true;
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
            return false;
        }
    }
}
#endif