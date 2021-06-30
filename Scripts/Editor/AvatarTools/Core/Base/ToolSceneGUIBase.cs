#if UNITY_EDITOR
using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    public abstract class ToolSceneGUIBase : ITool
    {
        const float windowPadding = 5;

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

        public ISettingsContainer Settings { get; private set; }

        public bool Enabled
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

        public virtual UIDefinition UIDefs { get; set; }

        public virtual bool EnabledInUI { get; set; } = true;

        Rect miniWindow = new Rect(EditorGUIUtility.pixelsPerPoint * windowPadding, EditorGUIUtility.pixelsPerPoint * windowPadding + 18, 0, 0);
        public SerializedObject serializedObject;
        EditorApplication.CallbackFunction updateCallback;

        public ToolSceneGUIBase()
        {
            if(UIDefs == null)
                UIDefs = new UIDefinition(GetType().Name);
            Settings = this.GetOrCreateSettingsContainer();
        }

        protected virtual GUIContent CreateGUIContent()
        {
            return new GUIContent(UIDefs.Name, UIDefs.Description);
        }

        void SetupUpdateCallback(ref EditorApplication.CallbackFunction callback, bool add)
        {
            if(callback == null)
            {
                PumkinTools.LogVerbose($"Setting up Update callback for <b>{UIDefs.Name}</b>");
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
                PumkinTools.LogVerbose($"Setting up OnSceneGUI callback for <b>{UIDefs.Name}</b>");
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

            Handles.BeginGUI();
            {
                miniWindow = GUILayout.Window(2, miniWindow,
                    id =>
                    {
                        EditorGUILayout.Space();

                        DrawInsideSceneGUIWindow();

                        UIHelpers.DrawLine(1, true, false);

                        GUILayout.BeginHorizontal();
                        {
                            if(GUILayout.Button("Cancel"))
                                PressedCancel(serializedObject?.targetObject as GameObject);

                            if(GUILayout.Button("Apply"))
                                PressedApply(serializedObject?.targetObject as GameObject);
                        }
                        GUILayout.EndHorizontal();
                    }, UIDefs.Name);
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
            catch { }
        }

        protected virtual void DrawHandles(GameObject target) { }
        protected virtual void DrawInsideSceneGUIWindow() { }

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
                    _editorToolOld = UnityEditor.Tools.current;
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

            UnityEditor.Tools.current = _editorToolOld;

            if(success)
            {
                Status = ToolStatus.CompletedOK;
                PumkinTools.Log($"<b>{UIDefs.Name}</b> completed successfully");
            }
            else
            {
                if(Status == ToolStatus.Canceled)
                    PumkinTools.Log($"<b>{UIDefs.Name}</b> was cancelled");
                else
                    PumkinTools.LogWarning($"<b>{UIDefs.Name}</b> failed");
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

        public void CheckThenUpdate()
        {
            if(Enabled)
                Update();
        }

        public virtual void Update() { }

        public void DrawUI(params GUILayoutOption[] options)
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

        bool _allowSceneGUI;
        bool _allowUpdate;
        GUIContent _content;
        Tool _editorToolOld;
    }
}
#endif