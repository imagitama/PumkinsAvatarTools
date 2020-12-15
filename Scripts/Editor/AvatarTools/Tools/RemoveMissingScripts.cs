#if UNITY_EDITOR
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    [AutoLoad(DefaultIDs.Tools.RemoveMissingScripts, ParentModuleID = DefaultIDs.Modules.Tools_FixAvatar)]
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
            PumkinTools.Log($"Removed <b>{removedCount}</b> empty script from <b>{target.name}</b>", target);
            return true;
        }
    }
}
#endif