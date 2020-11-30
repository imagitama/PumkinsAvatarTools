using NUnit.Framework.Internal;
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.AvatarTools.Types;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Tools
{
    [AutoLoad("tools_editScale", "vrchat", ParentModuleID = DefaultModuleIDs.TOOLS_SETUP_AVATAR)]
    [UIDefinition("[VRC] Edit Scale")]
    class EditScale_VRChat : ToolSceneGUIBase
    {
        SerializedObject serialDesc = null;

        bool moveViewpoint = true;

        Vector3 startView;
        Vector3 startPosition;
        Vector3 tempView;

        Vector3 startScale;
        float tempScale = 0;

        public EditScale_VRChat()
        {
            WindowSize = new Vector2(200, 70);
        }

        protected override bool Prepare(GameObject target)
        {
            if(base.Prepare(target))
            {
                if(VRChatTypes.VRC_AvatarDescriptor == null)
                {
                    PumkinTools.LogError("Couldn't find VRC_AvatarDescriptor type in project. Is the VRCSDK Imported?");
                    return false;
                }

                startScale = target.transform.localScale;
                startPosition = target.transform.position;

                tempView = startView;
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
