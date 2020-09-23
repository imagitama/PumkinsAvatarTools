#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Helpers;
using Pumkin.AvatarTools.Implementation.Settings;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools
{
    /// <summary>
    /// Base sub tool class. Should be inherited to create a tool
    /// </summary>
    abstract class SubToolBase : ISubTool
    {
        public string Name { get; set; }
        public string Description { get; set; }        
        public string GameConfigurationString { get; set; }
        public bool AllowUpdate
        {
            get
            {
                return _allowUpdate;
            }

            set
            {
                if(_allowUpdate == value)
                    return;
                
                if(_allowUpdate = value)    //Intentional assign + check
                {
                    SetupUpdateCallback();
                    EditorApplication.update += updateCallback;
                }
                else
                {
                    EditorApplication.update -= updateCallback;
                }
            }
        }
        public int OrderInUI { get; set; }
        protected virtual GUIContent Content 
        {
            get
            {
                return _content ?? (_content = new GUIContent(Name, Description));
            }
            set
            {
                _content = value;
            }
        }
        public virtual SettingsContainer Settings { get => null; }
        public bool ExpandSettings { get; protected set; }        

        bool _allowUpdate;
        GUIContent _content;        

        EditorApplication.CallbackFunction updateCallback;

        public SerializedObject serializedObject;

        public SubToolBase() 
        {
            var uiDefAttr = GetType().GetCustomAttribute<UIDefinitionAttribute>(false);
            if(uiDefAttr != null)   //Don't want default values if attribute missing, so not using uiDefAttr?.Description ?? "whatever"
            {
                Name = uiDefAttr.FriendlyName;
                Description = uiDefAttr.Description;
                OrderInUI = uiDefAttr.OrderInUI;
            }
            else
            {
                Name = GetType().Name;
                Description = "Base Tool description";
                OrderInUI = 0;
            }
            SetupSettings();
        }

        void SetupUpdateCallback()
        {
            if(updateCallback == null)
            {
                PumkinTools.Log($"Setting up Update callback for {Name}");
                updateCallback = new EditorApplication.CallbackFunction(Update);
            }            
        }

        protected virtual void SetupSettings() { }

        public virtual void DrawUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if(GUILayout.Button(Content, Styles.MediumButton))
                    TryExecute(PumkinTools.SelectedAvatar);
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
        
        public bool TryExecute(GameObject target)
        {
            try
            {
                if(Prepare(target) && DoAction(target))
                {
                    serializedObject.ApplyModifiedProperties();
                    Finish(target);
                    return true;
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
            return false;
        }

        protected virtual bool Prepare(GameObject target)
        {
            if(!target)
            {
                Debug.LogError("No avatar selected");
                return false;
            }

            serializedObject = new SerializedObject(target);
            return true;
        }        

        protected abstract bool DoAction(GameObject target);

        protected virtual void Finish(GameObject target)
        {
            PumkinTools.Log($"{Name} completed successfully.");
        }

        public virtual void Update()
        {
            if(!AllowUpdate)
                return;
        }

    }
}
#endif