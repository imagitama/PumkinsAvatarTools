using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    [AutoLoad(DefaultIDs.Tools.LookAtMouseToggle, ParentModuleID = DefaultIDs.Modules.TestAvatar)]
    public class LookAtMouseToggle : ToolUpdateToggleBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Look at Mouse");
        public override bool EnabledInUI => PumkinTools.SelectedAvatar != null && head != null;
        Quaternion StartHeadRotation { get; set; }

        Vector3 lookAtPosition;
        Transform head;

        bool showLookTarget = false;
        private bool _lookAtMouse;

        public LookAtMouseToggle()
        {
            PumkinTools.OnAvatarSelectionChanged += OnAvatarSelectionChanged;
            SetupNewAvatar(PumkinTools.SelectedAvatar);
        }

        protected override void OnDisableToggle()
        {
            base.OnDisableToggle();
            RestoreOldAvatar();
        }

        private void OnAvatarSelectionChanged(GameObject newSelection)
        {
            RestoreOldAvatar();
            SetupNewAvatar(newSelection);
        }

        void SetupNewAvatar(GameObject newAvatar)
        {
            if(!newAvatar)
                return;

            var anim = newAvatar.GetComponent<Animator>();
            if(!anim)
                return;

            head = anim.GetBoneTransform(HumanBodyBones.Head);
        }

        void RestoreOldAvatar()
        {
            if(head)
                head.transform.rotation = StartHeadRotation;
        }

        protected override void OnSceneGUI(SceneView sceneView)
        {
            //Get mouse position in world space
            float lookAtDistance = (sceneView.camera.transform.position.magnitude - head.transform.position.magnitude) * 0.5f;
            lookAtDistance = Mathf.Clamp(lookAtDistance, 0.1f, 1);
            Vector3 distanceFromCam = new Vector3(sceneView.camera.transform.position.x, sceneView.camera.transform.position.y, lookAtDistance);
            Plane plane = new Plane(Vector3.forward, distanceFromCam);

            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            if(plane.Raycast(ray, out float enter))
                lookAtPosition = ray.GetPoint(enter);
        }

        public override void Update()
        {
            if(!head)
                return;

            if(showLookTarget)
                DevHelpers.DrawDebugStar(lookAtPosition, 0.1f, 0.5f);
            head.LookAt(lookAtPosition);
        }
    }
}
