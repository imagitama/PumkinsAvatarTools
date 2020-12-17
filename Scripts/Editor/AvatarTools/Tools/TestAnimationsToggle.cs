using Pumkin.Core;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    [AutoLoad(DefaultIDs.Tools.TestAnimations, ParentModuleID = DefaultIDs.Modules.TestAvatar)]
    class TestAnimationsToggle : ToolUpdateToggleBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Test Animations");
        readonly RuntimeAnimatorController avatarTestController;

        GameObject avatar = null;
        Animator animator = null;

        bool oldRootMotion = false;
        RuntimeAnimatorController oldRuntimeController = null;

        enum TestAnimation { Idle, Walk };

        TestAnimation animation = TestAnimation.Idle;

        bool CanPlay => animator && animator.isHuman;

        bool WantsToPlay
        {
            get => _wantsToPlay;
            set
            {
                if(value == _wantsToPlay)
                    return;

                if(value)
                    SetupAnimator();
                else
                    RestoreAvatar();
                _wantsToPlay = value;
            }

        }

        private void SetupAnimator()
        {
            oldRootMotion = animator.applyRootMotion;

            animator.applyRootMotion = false;
            oldRuntimeController = animator.runtimeAnimatorController;
            animator.runtimeAnimatorController = avatarTestController;

            animator.SetFloat("Animation", (float)animation);
        }

        bool _wantsToPlay = false;

        public TestAnimationsToggle()
        {
            avatarTestController = Resources.Load<RuntimeAnimatorController>("Pumkin/Animations/AvatarTestController");
            PumkinTools.OnAvatarSelectionChanged += OnAvatarSelectionChanged;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        public override void DrawUI(params GUILayoutOption[] options)
        {
            if(!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Please enter Play mode to test animations", MessageType.Info);
                return;
            }
            else if(!CanPlay)
            {
                EditorGUILayout.HelpBox("Please select a Humanoid avatar to test animations", MessageType.Info);
                return;
            }
            else if(!avatarTestController)
            {
                EditorGUILayout.HelpBox("AvatarTestController is missing. Can't test animations.\nPlease reimport the tools if you want to use this feature.", MessageType.Error);
                return;
            }

            EditorGUILayout.BeginHorizontal();
            {
                WantsToPlay = EditorGUILayout.ToggleLeft("Test Animations", WantsToPlay);

                GUILayout.FlexibleSpace();  //Makes the label stretch out less

                EditorGUI.BeginChangeCheck();
                {
                    EditorGUI.BeginDisabledGroup(!CanPlay || !WantsToPlay);
                    {
                        animation = (TestAnimation)EditorGUILayout.EnumPopup(GUIContent.none, animation);
                    }
                    EditorGUI.EndDisabledGroup();
                }
                if(EditorGUI.EndChangeCheck())
                    animator.SetFloat("Animation", (float)animation);
            }
            EditorGUILayout.EndHorizontal();
        }

        void SetupAvatar(GameObject newAvatar)
        {
            avatar = newAvatar;
            if(!newAvatar)
                return;

            animator = newAvatar.GetComponent<Animator>();

            if(WantsToPlay)
                SetupAnimator();
        }

        void RestoreAvatar()
        {
            if(!animator)
                return;

            animator.applyRootMotion = oldRootMotion;
            animator.runtimeAnimatorController = oldRuntimeController;
        }

        private void OnPlayModeChanged(PlayModeStateChange mode)
        {
            Debug.Log("Calling PlayModeStateChanged()!");
            if(mode == PlayModeStateChange.ExitingPlayMode)
                WantsToPlay = false;
        }

        private void OnAvatarSelectionChanged(GameObject newSelection)
        {
            RestoreAvatar();
            SetupAvatar(newSelection);
        }
    }
}
