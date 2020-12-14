using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    /// <summary>
    /// Base tool that draws a toggle and runs it's Update method based on the toggle.
    /// </summary>
    public abstract class ToggleUpdateToolBase : ToolBase
    {
        public ToggleUpdateToolBase()
        {
            CanUpdate = false;
        }

        public override void DrawUI(params GUILayoutOption[] options)
        {
            CanUpdate = EditorGUILayout.ToggleLeft(Content, CanUpdate, options);
        }

        protected override bool DoAction(GameObject target)
        {
            return true;
        }
    }
}
