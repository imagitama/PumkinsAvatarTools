#if UNITY_EDITOR
using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.Settings;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core.Extensions;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    /// <summary>
    /// Base tool class. Should be inherited to create a tool
    /// </summary>
    public abstract class ToolBase : ITool, IDisposable
    {
        public virtual UIDefinition UIDefs { get; set; }

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
            return new GUIContent(UIDefs.Name, UIDefs.Description);
        }

        public virtual bool EnabledInUI { get; set; } = true;

        public ISettingsContainer Settings { get; private set; }

        bool _allowUpdate;
        GUIContent _content;
        EditorApplication.CallbackFunction updateCallback;

        public SerializedObject serializedObject;

        public ToolBase()
        {
            if(UIDefs == null)
                UIDefs = new UIDefinition(StringHelpers.ToTitleCase(GetType().Name));
            Settings = this.GetOrCreateSettingsContainer();
        }

        void SetupUpdateCallback(ref EditorApplication.CallbackFunction callback, bool add)
        {
            if(callback == null)
            {
                PumkinTools.LogVerbose($"Setting up Update callback for <b>{UIDefs.Name}</b>");
                callback = new EditorApplication.CallbackFunction(CheckThenUpdate);
            }

            if(!add)
                EditorApplication.update -= callback;
            else
                EditorApplication.update += callback;
        }

        public virtual void DrawUI(params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal();
            {
                if(GUILayout.Button(Content, Styles.SubToolButton, options))
                    TryExecute(PumkinTools.SelectedAvatar);
                if(Settings != null)
                    UIDefs.ExpandSettings = GUILayout.Toggle(UIDefs.ExpandSettings, Icons.Options, Styles.MediumIconButton);
            }
            EditorGUILayout.EndHorizontal();

            //Draw settings here
            if(Settings != null && UIDefs.ExpandSettings)
            {
                UIHelpers.DrawInVerticalBox(() =>
                {
                    EditorGUILayout.Space();
                    Settings?.DrawUI();
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
                PumkinTools.Log($"<b>{UIDefs.Name}</b> completed successfully");
            else
                PumkinTools.LogWarning($"<b>{UIDefs.Name}</b> failed");
        }

        public void CheckThenUpdate()
        {
            if(CanUpdate)
                Update();
        }

        public virtual void Update() { }

        public virtual void Dispose()
        {
            EditorApplication.update -= CheckThenUpdate;
        }
    }
}
#endif