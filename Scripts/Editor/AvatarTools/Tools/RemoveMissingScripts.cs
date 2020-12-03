#if UNITY_EDITOR
using Pumkin.AvatarTools.Base;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Tools
{
    [AutoLoad("tools_removeMissingScripts", ParentModuleID = "tools_fixAvatar")]
    class RemoveMissingScripts : ToolBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Remove Missing Scripts");

        protected override bool DoAction(GameObject target)
        {
            var components = target.GetComponents<Component>();
            int removedCount = 0;

            for(var i = 0; i < components.Length; i++)
            {
                if(components[i] != null)
                    continue;

                var serializedObject = new SerializedObject(target);
                var prop = serializedObject.FindProperty("m_Component");
                prop.DeleteArrayElementAtIndex(i - removedCount);
                removedCount++;
                serializedObject.ApplyModifiedProperties();
            }
            PumkinTools.Log($"Removed {removedCount} empty script from {target.name}", target);
            return true;
        }
    }
}
#endif