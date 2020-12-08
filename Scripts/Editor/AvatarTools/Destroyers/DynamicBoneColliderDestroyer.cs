using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.AvatarTools.Types;
using Pumkin.Core;
using Pumkin.Core.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Pumkin.Core.Extensions;

namespace Pumkin.AvatarTools.Destroyers
{
    [AutoLoad(DefaultIDs.Destroyers.DynamicBoneCollider, ParentModuleID = DefaultIDs.Modules.Destroyer)]
    class DynamicBoneColliderDestroyer : ComponentDestroyerBase
    {
        public override string ComponentTypeFullName => "DynamicBoneCollider";

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
