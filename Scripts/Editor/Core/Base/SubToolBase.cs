#if UNITY_EDITOR
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using Pumkin.Core.Attributes;
using Pumkin.Core.Helpers;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Base
{
    /// <summary>
    /// Base sub tool class. Should be inherited to create a tool
    /// </summary>
    abstract class SubToolBase : ITool
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
        public virtual GUIContent Content
        {
            get
            {
                if(_content == null)
                    _content = CreateGUIContent();
                return _content;
            }
        }

        protected virtual GUIContent CreateGUIContent()
        {
            return new GUIContent(Name, Description);
        }

        public virtual ISettingsContainer Settings { get => null; }
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

        public virtual void DrawUI(params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal();
            {
                if(GUILayout.Button(Content, Styles.SubToolButton, options))
                    TryExecute(PumkinTools.SelectedAvatar);
                if(Settings != null)
                    ExpandSettings = GUILayout.Toggle(ExpandSettings, Icons.Options, Styles.MediumIconButton);
            }
            EditorGUILayout.EndHorizontal();

            //Draw settings here
            if(Settings != null && ExpandSettings)
            {
                UIHelpers.VerticalBox(() =>
                {
                    EditorGUILayout.Space();
                    Settings.Editor.OnInspectorGUI();
                });
            }
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