using System;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    /// <summary>
    /// Base tool that draws a toggle and runs it's Update method based on the toggle.
    /// </summary>
    public abstract class ToolUpdateToggleBase : ToolBase
    {
        private bool _allowSceneGUI = false;

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

        protected virtual void OnEnable()
        {
            CanUpdate = true;
            CanDrawSceneGUI = true;
        }

        protected virtual void OnDisable()
        {
            CanUpdate = false;
            CanDrawSceneGUI = false;
        }

        public ToolUpdateToggleBase()
        {
            CanUpdate = false;
        }

        public override void DrawUI(params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            bool flag = EditorGUILayout.ToggleLeft(Content, CanUpdate, options);
            if(EditorGUI.EndChangeCheck())
            {
                CallbackThenPrepare(flag, PumkinTools.SelectedAvatar);
            }
        }

        void CallbackThenPrepare(bool flag, GameObject target)
        {
            if(flag && Prepare(target))
                OnEnable();
            else
                OnDisable();
            DoAction(target);
        }

        protected override bool DoAction(GameObject target)
        {
            return true;
        }

        void SetupOnSceneGUIDelegate(bool shouldAdd)
        {
            if(shouldAdd)
            {
                PumkinTools.LogVerbose($"Setting up OnSceneGUI callback for <b>{UIDefs.Name}</b>");
                SceneView.onSceneGUIDelegate += CheckThenOnSceneGUI;
            }
            else
            {
                SceneView.onSceneGUIDelegate -= CheckThenOnSceneGUI;
            }
        }

        void CheckThenOnSceneGUI(SceneView sceneView)
        {
            if(CanDrawSceneGUI)
                OnSceneGUI(sceneView);
        }

        protected virtual void OnSceneGUI(SceneView sceneView) { }

        public override void Dispose()
        {
            base.Dispose();
            SetupOnSceneGUIDelegate(false);
        }
    }
}
