using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.AvatarTools.Types;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Tools
{
    [AutoLoad(DefaultIDs.Tools.EditViewpoint, "vrchat", ParentModuleID = DefaultIDs.Modules.Tools_SetupAvatar)]
    class EditViewpoint_VRChat : ToolSceneGUIBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Edit Viewpoint");
        protected override Vector2 WindowSize { get; set; } = new Vector2(200, 70);

        Vector3 startViewpoint;
        Vector3 tempViewpoint;

        SerializedObject serialDescriptor;
        SerializedProperty viewPos;

        bool avatarIsHumanoid = false;

        protected override bool Prepare(GameObject target)
        {
            if(base.Prepare(target))
            {
                serialDescriptor = null;
                viewPos = null;
                avatarIsHumanoid = AvatarHelpers.IsHumanoid(target);

                if(VRChatTypes.VRC_AvatarDescriptor == null)
                {
                    PumkinTools.LogError("Couldn't find VRC_AvatarDescriptor type in project. Is the VRCSDK Imported?");
                    return false;
                }

                var desc = target.GetComponent(VRChatTypes.VRC_AvatarDescriptor);

                if(desc == null)
                {
                    Status = ToolStatus.Error;
                    return false;
                }

                serialDescriptor = new SerializedObject(desc);
                viewPos = serialDescriptor.FindProperty("ViewPosition");

                if(viewPos == null)
                {
                    Status = ToolStatus.Error;
                    return false;
                }

                startViewpoint = viewPos.vector3Value;
                tempViewpoint = startViewpoint + target.transform.position;
                return true;
            }
            return false;
        }

        protected override bool DoAction(GameObject target)
        {
            return true;    //Moving viewpoint is handled in DrawHandles
        }

        protected override void PressedCancel(GameObject target)
        {
            base.PressedCancel(target);
            VRChatHelpers.SetAvatarViewpoint(serialDescriptor, startViewpoint);
        }

        protected override void DrawHandles(GameObject target)
        {
            base.DrawHandles(target);
            tempViewpoint = viewPos.vector3Value + target.transform.position;

            EditorGUI.BeginChangeCheck();
            {
                tempViewpoint = Handles.PositionHandle(tempViewpoint, Quaternion.identity);
            }
            if(EditorGUI.EndChangeCheck())
            {
                VRChatHelpers.SetAvatarViewpoint(serialDescriptor, tempViewpoint, target.transform.position);
            }
        }

        protected override void DrawInsideSceneWindowGUI()
        {
            base.DrawInsideSceneWindowGUI();
            EditorGUI.BeginDisabledGroup(!avatarIsHumanoid);
            {
                if(GUILayout.Button("Move to Eyes"))
                {
                    viewPos.vector3Value = AvatarHelpers.GetEyePositionLocal(serializedObject.targetObject as GameObject) + new Vector3(0, 0, 0.05f);
                    viewPos.serializedObject.ApplyModifiedProperties();
                }
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}
