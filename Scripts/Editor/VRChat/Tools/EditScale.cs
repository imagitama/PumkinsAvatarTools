using Pumkin.AvatarTools.VRChat;
using Pumkin.AvatarTools2.Tools;
using Pumkin.AvatarTools2.Types;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Tools
{
    [AutoLoad(DefaultIDs.Tools.EditScale, "vrchat", ParentModuleID = DefaultIDs.Modules.Tools_SetupAvatar)]
    class EditScale : ToolSceneGUIBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("[VRC] Edit Scale");

        bool moveViewpoint = true;

        Vector3 startView;
        Vector3 startPosition;

        Vector3 startScale;
        float tempScale = 0;

        public EditScale()
        {
            WindowSize = new Vector2(200, 70);
        }

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

                return true;
            }
            return false;
        }

        protected override void DrawInsideSceneWindowGUI()
        {
            base.DrawInsideSceneWindowGUI();
            moveViewpoint = EditorGUILayout.ToggleLeft("Move Viewpoint", moveViewpoint);
        }

        protected override bool DoAction(GameObject target)
        {
            VRChatHelpers.SetAvatarScale(target, tempScale, moveViewpoint);

            serializedObject.ApplyModifiedProperties();
            return true;
        }

        protected override void PressedCancel(GameObject target)
        {
            VRChatHelpers.SetAvatarScale(target, startScale.y, moveViewpoint);
            base.PressedCancel(target);
        }

        protected override void DrawHandles(GameObject target)
        {
            EditorGUI.BeginChangeCheck();
            {
                tempScale = Handles.ScaleSlider(tempScale, startPosition, Vector3.up, Quaternion.identity, HandleUtility.GetHandleSize(startPosition) * 2, 0.01f);
            }
            if(EditorGUI.EndChangeCheck())
            {
                VRChatHelpers.SetAvatarScale(serializedObject.targetObject as GameObject, tempScale, moveViewpoint);
            }
        }
    }
}
