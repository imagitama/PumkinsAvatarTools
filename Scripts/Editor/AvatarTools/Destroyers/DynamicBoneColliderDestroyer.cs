using Pumkin.Core;
using UnityEditor;
using UnityEngine;
using Pumkin.Core.Extensions;

namespace Pumkin.AvatarTools2.Destroyers
{
    [AutoLoad(DefaultIDs.Destroyers.DynamicBoneCollider, ParentModuleID = DefaultIDs.Modules.Destroyer)]
    class DynamicBoneColliderDestroyer : ComponentDestroyerBase
    {
        public override string[] ComponentTypesFullNames => new string[] { GenericTypes.DynamicBoneCollider?.FullName };

        protected override void Finish(GameObject target)
        {
            //Clean up null references from dynamic bone collider arrays inside dynamic bone scripts
            var dbones = target.GetComponentsInChildren(GenericTypes.DynamicBone);
            foreach(var db in dbones)
            {
                var serialDb = new SerializedObject(db);
                var arr = serialDb.FindProperty("m_Colliders");
                if(arr == null)
                    continue;

                arr.RemoveNullReferencesFromArray();
                serialDb.ApplyModifiedProperties();
            }

            base.Finish(target);
        }
    }
}
