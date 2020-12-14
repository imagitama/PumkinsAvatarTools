using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;


namespace Pumkin.AvatarTools2.Modules
{
    [AutoLoad("Test Avatar")]
    class TestAvatarModule : UIModuleBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Test Avatar", 4);

        GameObject Avatar { get; set; }

        Animator Animator { get; set; }

        RuntimeAnimatorController OldRuntimeController { get; set; }

        Transform Head { get; set; }


        //Look At Mouse
        Quaternion StartHeadRotation { get; set; }
        Vector3 lookAtPosition;

        bool LookAtMouse
        {
            get => _lookAtMouse;
            set
            {
                if(value == _lookAtMouse)
                    return;

                _lookAtMouse = value;
                if(Avatar && Head)
                {
                    if(value)
                        StartHeadRotation = Head.rotation;
                    else
                        Head.rotation = StartHeadRotation;
                }
            }
        }

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
                LookAtMouse = false;
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
                Head = Animator.GetBoneTransform(HumanBodyBones.Head) ?? null;
            }
        }

        void CleanupAvatar(GameObject avatar)
        {
            if(!Avatar)
                return;

            if(Animator)
                Animator.runtimeAnimatorController = OldRuntimeController;
            if(Head)
                Head.transform.rotation = StartHeadRotation;

            Avatar = null;
            Animator = null;
            Head = null;
        }

        public override void DrawContent()
        {
            if(!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Please start play mode", MessageType.Info);
                return;
            }
            if(!Avatar || !Animator || !Animator.isHuman)
            {
                EditorGUILayout.HelpBox("Please select a humanoid avatar", MessageType.Info);
                return;
            }

            DrawControlls();
            base.DrawContent();

            CanDrawSceneGUI = LookAtMouse;
            CanUpdate = LookAtMouse || PlayAnimations;
        }

        void DrawControlls()
        {
            LookAtMouse = EditorGUILayout.ToggleLeft("Look at mouse", LookAtMouse);

            EditorGUILayout.Space();

            PlayAnimations = EditorGUILayout.ToggleLeft("Test Animations", PlayAnimations);

            if(PlayAnimations)
                animation = (TestAnimation)EditorGUILayout.EnumPopup("Animation", animation);
        }

        public override void OnSceneGUI(SceneView sceneView)
        {
            //Get mouse position in world space
            if(LookAtMouse)
            {
                float lookAtDistance = (sceneView.camera.transform.position.magnitude - Head.transform.position.magnitude) * 0.5f;
                lookAtDistance = Mathf.Clamp(lookAtDistance, 0.1f, 1);
                Vector3 distanceFromCam = new Vector3(sceneView.camera.transform.position.x, sceneView.camera.transform.position.y, lookAtDistance);
                Plane plane = new Plane(Vector3.forward, distanceFromCam);

                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                if(plane.Raycast(ray, out float enter))
                    lookAtPosition = ray.GetPoint(enter);
            }
        }

        public override void Update()
        {
            if(LookAtMouse)
            {
                //Animator.SetLookAtPosition(lookAtPosition);
                DevHelpers.DrawDebugStar(lookAtPosition, 0.3f, 1);
                Head.LookAt(lookAtPosition);
            }
            if(PlayAnimations)
                Animator.SetFloat("Animation", (float)animation);
        }
    }
}
