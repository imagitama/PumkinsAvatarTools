using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;


namespace Pumkin.AvatarTools2.Modules
{
    [AutoLoad(DefaultIDs.Modules.TestAvatar)]
    class TestAvatarModule : UIModuleBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Test Avatar", 4);

        GameObject Avatar { get; set; }

        Animator Animator { get; set; }

        RuntimeAnimatorController OldRuntimeController { get; set; }


        bool PlayAnimations
        {
            get => _playAnimations;
            set
            {
                if(value == _playAnimations)
                    return;

                _playAnimations = value;
                if(value)
                {
                    defaultRootMotion = Animator.applyRootMotion;
                    Animator.applyRootMotion = false;
                    Animator.runtimeAnimatorController = defaultController;
                }
                else
                {
                    Animator.runtimeAnimatorController = OldRuntimeController;
                    Animator.applyRootMotion = defaultRootMotion;
                }
            }
        }

        bool _playAnimations = false;

        //Walking and animations
        RuntimeAnimatorController defaultController = null;
        bool defaultRootMotion = false;

        enum TestAnimation { Idle, Walk };

        TestAnimation animation = TestAnimation.Idle;
        private bool _lookAtMouse;

        public TestAvatarModule()
        {
            PumkinTools.OnAvatarSelectionChanged += PumkinTools_OnAvatarSelectionChanged;
            defaultController = Resources.Load<RuntimeAnimatorController>("Pumkin/Animations/AvatarTestController");
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        private void EditorApplication_playModeStateChanged(PlayModeStateChange mode)
        {
            if(mode == PlayModeStateChange.ExitingPlayMode)
            {
                PlayAnimations = false;
            }
        }

        private void PumkinTools_OnAvatarSelectionChanged(GameObject newSelection)
        {
            if(!newSelection)
                return;

            CleanupAvatar(Avatar);
            SetupAvatar(newSelection);
        }
        void SetupAvatar(GameObject avatar)
        {
            Avatar = avatar;
            Animator = Avatar.GetComponent<Animator>();

            if(Animator)
            {
                OldRuntimeController = Animator.runtimeAnimatorController ?? null;
            }
        }

        void CleanupAvatar(GameObject avatar)
        {
            if(!Avatar)
                return;

            if(Animator)
                Animator.runtimeAnimatorController = OldRuntimeController;

            Avatar = null;
            Animator = null;
        }

        public override void DrawContent()
        {
            DrawControlls();
            base.DrawContent();

            CanUpdate = PlayAnimations;
        }

        void DrawControlls()
        {
            PlayAnimations = EditorGUILayout.ToggleLeft("Test Animations", PlayAnimations);

            if(PlayAnimations)
                animation = (TestAnimation)EditorGUILayout.EnumPopup("Animation", animation);
        }

        public override void Update()
        {
            if(PlayAnimations)
                Animator.SetFloat("Animation", (float)animation);
        }
    }
}
