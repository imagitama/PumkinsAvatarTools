using Pumkin.UnityTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Tools.SubTools
{
    [AutoLoad("tools_removeMissingScripts", ParentModuleID = "tools_fixAvatar")]
    [UIDefinition("Remove Missing Scripts")]
    class RemoveMissingScripts : SubToolBase
    {
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
