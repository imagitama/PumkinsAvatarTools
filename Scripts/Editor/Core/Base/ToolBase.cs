#if UNITY_EDITOR
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.Tools;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Base
{
    /// <summary>
    /// Base tool class. Should be inherited to create a tool
    /// </summary>
    public abstract class ToolBase : ITool, IDisposable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string GameConfigurationString { get; set; }
        public bool CanUpdate
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
                    SetupUpdateCallback(ref updateCallback, true);
                else
                    SetupUpdateCallback(ref updateCallback, false);
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
        public bool EnabledInUI { get; set; } = true;

        bool _allowUpdate;
        GUIContent _content;

        EditorApplication.CallbackFunction updateCallback;

        public SerializedObject serializedObject;

        public ToolBase()
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

        void SetupUpdateCallback(ref EditorApplication.CallbackFunction callback, bool add)
        {
            if(callback == null)
            {
                PumkinTools.LogVerbose($"Setting up Update callback for <b>{Name}</b>");
                callback = new EditorApplication.CallbackFunction(Update);
            }

            if(!add)
                EditorApplication.update -= callback;
            else
                EditorApplication.update += callback;
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
                    Settings?.Editor?.OnInspectorGUI();
                });
            }
        }

        public virtual bool TryExecute(GameObject target)
        {
            try
            {
                if(Prepare(target) && DoAction(target))
                {
                    serializedObject.ApplyModifiedProperties();
                    Finish(target, true);
                    return true;
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
            Finish(target, false);
            return false;
        }

        protected virtual bool Prepare(GameObject target)
        {
            if(!target)
            {
                PumkinTools.LogError("No avatar selected");
                return false;
            }

            serializedObject = new SerializedObject(target);
            return true;
        }

        protected abstract bool DoAction(GameObject target);

        protected virtual void Finish(GameObject target, bool success)
        {
            if(success)
                PumkinTools.Log($"<b>{Name}</b> completed successfully");
            else
                PumkinTools.LogWarning($"<b>{Name}</b> failed");
        }

        public virtual void Update()
        {
            if(!CanUpdate)
                return;
        }

        public virtual void Dispose()
        {
            EditorApplication.update -= Update;
        }
    }
}
#endif