using Pumkin.AvatarTools2.VRChat;
using Pumkin.AvatarTools2.Tools;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Tools
{
    [AutoLoad(DefaultIDs.Tools.EditScale, "VRChat", ParentModuleID = DefaultIDs.Modules.Tools_SetupAvatar)]
    class EditScale : ToolSceneGUIBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("[VRC] Edit Scale");

        bool moveViewpoint = true;

        Vector3 startView;
        Vector3 startPosition;

        Vector3 startScale;
        float tempScale;
        Vector3 tempViewpoint;
        bool hasDescriptor;

        protected override bool Prepare(GameObject target)
        {
            if(base.Prepare(target))
            {
                if(VRChatTypes.VRC_AvatarDescriptor == null)
                {
                    PumkinTools.LogWarning("Couldn't find VRC_AvatarDescriptor type in project. Is the VRCSDK Imported?");
                    return false;
                }

                startScale = target.transform.localScale;
                startPosition = target.transform.position;

                tempScale = startScale.y;
                hasDescriptor = target.GetComponent(VRChatTypes.VRC_AvatarDescriptor);

                return true;
            }
            return false;
        }

        protected override void DrawInsideSceneGUIWindow()
        {
            base.DrawInsideSceneGUIWindow();
            EditorGUI.BeginDisabledGroup(!hasDescriptor);
            {
                moveViewpoint = EditorGUILayout.ToggleLeft("Move Viewpoint", moveViewpoint);
            }
            EditorGUI.EndDisabledGroup();
        }

        protected override bool DoAction(GameObject target)
        {
            VRChatHelpers.SetAvatarScale(target, tempScale, moveViewpoint, out _);

            serializedObject.ApplyModifiedProperties();
            return true;
        }

        protected override void PressedCancel(GameObject target)
        {
            VRChatHelpers.SetAvatarScale(target, startScale.y, moveViewpoint, out _);
            base.PressedCancel(target);
        }

        protected override void DrawHandles(GameObject target)
        {
            EditorGUI.BeginChangeCheck();
            {
                tempScale = Handles.ScaleSlider(tempScale, startPosition, Vector3.up, Quaternion.identity, HandleUtility.GetHandleSize(startPosition) * 2, 0.01f);

                if(moveViewpoint)
                {
                    Color old = Handles.color;
                    Handles.color = Colors.viewpointBall;
                    Handles.SphereHandleCap(0, tempViewpoint, Quaternion.identity, 0.02f, EventType.Repaint);
                    Handles.color = old;
                }
            }
            if(EditorGUI.EndChangeCheck())
            {
                VRChatHelpers.SetAvatarScale(serializedObject.targetObject as GameObject, tempScale, moveViewpoint, out tempViewpoint);
            }
        }
    }
}
