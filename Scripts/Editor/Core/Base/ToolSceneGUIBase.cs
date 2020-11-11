using NUnit.Framework;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.Tools;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

namespace Pumkin.AvatarTools.Base
{
    public abstract class ToolSceneGUIBase : ITool
    {
        bool _allowSceneGUI;
        public bool CanDrawSceneGUI
        {
            get
            {
                return _allowSceneGUI;
            }

            set
            {
                if(_allowSceneGUI == value)
                    return;

                if(_allowSceneGUI = value)    //Intentional assign + check
                    SetupOnSceneGUIDelegate(true);
                else
                    SetupOnSceneGUIDelegate(false);
            }
        }
        protected Vector2 WindowSize { get; set; } = new Vector2(200, 50);
        float Padding { get; set; } = 10;
        public string Name { get; set; }
        public string Description { get; set; }
        public string GameConfigurationString { get; set; }
        public int OrderInUI { get; set; }
        public virtual ISettingsContainer Settings { get => null; }
        public bool ExpandSettings { get; protected set; }
        public bool EnabledInUI { get; set; } = true;

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

        protected ToolStatus Status = ToolStatus.Waiting;

        public virtual GUIContent Content
        {
            get
            {
                if(_content == null)
                    _content = CreateGUIContent();
                return _content;
            }
        }

        bool _allowUpdate;
        GUIContent _content;
        Tool editorToolOld;

        public SerializedObject serializedObject;

        EditorApplication.CallbackFunction updateCallback;

        public ToolSceneGUIBase()
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

        protected virtual void SetupSettings() { }

        protected virtual GUIContent CreateGUIContent()
        {
            return new GUIContent(Name, Description);
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

        void SetupOnSceneGUIDelegate(bool add)
        {
            if(add)
            {
                PumkinTools.LogVerbose($"Setting up Update callback for <b>{Name}</b>");
                SceneView.onSceneGUIDelegate += OnSceneGUI;
            }
            else
            {
                SceneView.onSceneGUIDelegate -= OnSceneGUI;
            }
        }

        public void OnSceneGUI(SceneView scene)
        {
            if(!CanDrawSceneGUI)
                return;

            Rect rect = scene.camera.pixelRect;
            rect = new Rect(Padding, rect.height - Padding - WindowSize.y, WindowSize.x, WindowSize.y);
            float minButtonWidth = rect.width / 2 - Styles.Box.padding.left - Styles.Box.padding.right;

            Handles.BeginGUI();
            {
                GUILayout.BeginArea(rect, Styles.Box);
                {
                    GUILayout.Label(Name);

                    if(CanDrawSceneGUI)
                    {
                        DrawInsideSceneWindowGUI();
                    }

                    GUILayout.BeginHorizontal();
                    {
                        if(GUILayout.Button("Cancel", GUILayout.MinWidth(minButtonWidth)))
                            PressedCancel(serializedObject?.targetObject as GameObject);

                        if(GUILayout.Button("Apply", GUILayout.MinWidth(minButtonWidth)))
                            PressedApply(serializedObject?.targetObject as GameObject);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();

            try
            {
                if(CanDrawSceneGUI)
                {
                    if(!serializedObject?.targetObject)
                    {
                        Status = ToolStatus.Error;
                        Finish(null, false);
                    }
                    else
                        DrawHandles(serializedObject?.targetObject as GameObject);
                }
            }
            catch {}
        }

        protected virtual void DrawHandles(GameObject target) { }
        protected virtual void DrawInsideSceneWindowGUI() { }

        protected virtual bool Prepare(GameObject target)
        {
            Status = ToolStatus.InProgress;
            if(!target)
            {
                PumkinTools.LogError("No avatar selected");
                return false;
            }

            serializedObject = new SerializedObject(target);
            return true;
        }

        public bool TryExecute(GameObject target)
        {
            try
            {
                if(Prepare(target))
                {
                    editorToolOld = UnityEditor.Tools.current;
                    UnityEditor.Tools.current = Tool.None;
                    CanDrawSceneGUI = true;
                    EnabledInUI = false;
                }
                else
                {
                    Status = ToolStatus.Error;
                    Finish(target, false);
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
            return false;
        }

        protected virtual void Finish(GameObject target, bool success)
        {
            CanDrawSceneGUI = false;
            EnabledInUI = true;

            UnityEditor.Tools.current = editorToolOld;

            if(success)
            {
                Status = ToolStatus.CompletedOK;
                PumkinTools.Log($"<b>{Name}</b> completed successfully");
            }
            else
            {
                if(Status == ToolStatus.Canceled)
                    PumkinTools.Log($"<b>{Name}</b> was cancelled");
                else
                    PumkinTools.LogWarning($"<b>{Name}</b> failed");
            }
        }

        protected virtual void PressedApply(GameObject target)
        {
            if(!DoAction(target))
            {
                Finish(target, false);
                return;
            }
            serializedObject.ApplyModifiedProperties();
            Finish(target, true);
        }

        protected abstract bool DoAction(GameObject target);

        protected virtual void PressedCancel(GameObject target)
        {
            Status = ToolStatus.Canceled;
            Finish(target, false);
        }

        public void Dispose()
        {
            EditorApplication.update -= Update;
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
        }

        public void Update()
        {
            if(!CanUpdate)
                return;
        }

        public void DrawUI(params GUILayoutOption[] options)
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
    }
}
