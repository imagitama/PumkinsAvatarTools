using Pumkin.AvatarTools2.Destroyers;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.Extensions;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRM.Destroyers
{
    [AutoLoad("destroyer_vrmSpringBoneColliderGroup", "VRM", ParentModuleID = DefaultIDs.Modules.Destroyer)]
    public class VRMSpringBoneColliderGroupDestroyer : ComponentDestroyerBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Spring Bone Collider");

        protected override GUIContent CreateGUIContent() => new GUIContent(UIDefs.Name, Icons.BoneCollider);

        public override string[] ComponentTypesFullNames => new string[] { VRMTypes.VRMSpringBoneColliderGroup?.FullName };

        //TODO: Make this automatic somehow
        protected override void Finish(GameObject target)
        {
            //Clean up null references from spring bone collider arrays inside spring bone scripts
            var sbones = target.GetComponentsInChildren(VRMTypes.VRMSpringBone);
            foreach(var sb in sbones)
            {
                var serialSB = new SerializedObject(sb);
                var arr = serialSB.FindProperty("ColliderGroups");
                if(arr == null)
                    continue;

                arr.RemoveNullReferencesFromArray();
                serialSB.ApplyModifiedProperties();
            }

            base.Finish(target);
        }
    }
}
