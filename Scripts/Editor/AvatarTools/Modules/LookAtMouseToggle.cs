using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    [AutoLoad(DefaultIDs.Tools.LookAtMouseToggle, ParentModuleID = DefaultIDs.Modules.TestAvatar)]
    public class LookAtMouseToggle : ToolUpdateToggleBase
    {
        Quaternion StartHeadRotation { get; set; }
        Vector3 lookAtPosition;
        Transform head;

        bool showLookTarget = false;
        private bool _lookAtMouse;

        bool ShouldLookAtMouse
        {
            get => _lookAtMouse;
            set
            {
                if(value == _lookAtMouse)
                    return;

                _lookAtMouse = value;
                if(head)
                {
                    if(value)
                        StartHeadRotation = head.rotation;
                    else
                        head.rotation = StartHeadRotation;
                }
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ShouldLookAtMouse = true;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ShouldLookAtMouse = false;
        }

        public override bool EnabledInUI => PumkinTools.SelectedAvatar != null;

        protected override bool Prepare(GameObject target)
        {
            if(!base.Prepare(target))
                return false;

            var anim = target.GetComponent<Animator>();
            if(!anim)
                return false;

            head = anim.GetBoneTransform(HumanBodyBones.Head);

            return true;
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
