using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    public abstract class ToggleUpdateToolBase : ToolBase
    {
        public ToggleUpdateToolBase()
        {
            CanUpdate = false;
        }

        public override void DrawUI(params GUILayoutOption[] options)
        {
            CanUpdate = EditorGUILayout.Toggle(Content, CanUpdate);
        }

        protected override bool DoAction(GameObject target)
        {
            return true;
        }
    }
}
