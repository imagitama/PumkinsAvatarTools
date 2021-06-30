using Pumkin.Core.Helpers;
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
        private bool _allowSceneGUI;
        public bool Enabled => _allowUpdate;
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

        protected virtual void OnEnableToggle()
        {
            base.Enabled = true;
            CanDrawSceneGUI = true;
        }

        protected virtual void OnDisableToggle()
        {
            base.Enabled = false;
            CanDrawSceneGUI = false;
        }

        protected ToolUpdateToggleBase()
        {
            base.Enabled = false;
        }

        public override void DrawUI(params GUILayoutOption[] options)
        {
            UIHelpers.DrawLine();
            EditorGUI.BeginChangeCheck();
            bool flag = EditorGUILayout.ToggleLeft(Content, Enabled, options);
            if(EditorGUI.EndChangeCheck())
            {
                CallbackThenPrepare(flag, PumkinTools.SelectedAvatar);
            }
            UIHelpers.DrawLine();
        }

        void CallbackThenPrepare(bool flag, GameObject target)
        {
            if(flag && Prepare(target))
                OnEnableToggle();
            else
                OnDisableToggle();
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
