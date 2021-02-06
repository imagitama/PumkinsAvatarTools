using Pumkin.Core;
using Pumkin.Core.Helpers;
using UnityEditor;
using UnityEngine;
using Pumkin.Core.UI;

namespace Pumkin.AvatarTools2.Tools
{
    [AutoLoad(DefaultIDs.Tools.EditScale, ParentModuleID = DefaultIDs.Modules.Tools_SetupAvatar)]
    class EditScale : ToolSceneGUIBase
    {
        Vector3 startScale;
        float tempScale = 0;

        private SerializedProperty scaleProp;
        private SerializedObject serialTransform;

        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Edit Scale");


        protected override bool Prepare(GameObject target)
        {
            if(base.Prepare(target))
            {
                startScale = target.transform.localScale;
                tempScale = startScale.y;

                serialTransform = new SerializedObject(target.transform);
                scaleProp = serialTransform.FindProperty("m_LocalScale");

                if(scaleProp != null)
                    return true;
            }
            return false;
        }

        protected override bool DoAction(GameObject target)
        {
            serialTransform.ApplyModifiedProperties();
            return true;
        }

        protected override void PressedCancel(GameObject target)
        {
            base.PressedCancel(target);
            scaleProp.vector3Value = startScale;
            serialTransform.ApplyModifiedProperties();
        }

        protected override void DrawHandles(GameObject target)
        {
            EditorGUI.BeginChangeCheck();
            {
                tempScale = Handles.ScaleSlider(tempScale, target.transform.position, Vector3.up,
                    Quaternion.identity, HandleUtility.GetHandleSize(target.transform.position) * 2, 0.01f);
            }
            if(EditorGUI.EndChangeCheck())
            {
                scaleProp.vector3Value = Vector3Helpers.ScaleVector(startScale, tempScale - startScale.y);
                serialTransform.ApplyModifiedPropertiesWithoutUndo();
            }
        }
    }
}
