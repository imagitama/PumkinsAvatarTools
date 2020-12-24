using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.Settings;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools2.Destroyers
{
    [AutoLoad(DefaultIDs.Destroyers.Collider, ParentModuleID = DefaultIDs.Modules.Destroyer)]
    class ColliderDestroyer : ComponentDestroyerBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Colliders");

        public override string[] ComponentTypesFullNames { get; } = new string[]
        {
            typeof(BoxCollider).FullName,
            typeof(CapsuleCollider).FullName,
            typeof(SphereCollider).FullName,
            typeof(MeshCollider).FullName,
        };

        //protected override bool Prepare(GameObject target)
        //{
        //    var settings = Settings as ColliderDestroyer_Settings;

        //    ComponentTypesAndEnabled[typeof(BoxCollider)] = settings.boxColliders;
        //    ComponentTypesAndEnabled[typeof(CapsuleCollider)] = settings.capsuleColliders;
        //    ComponentTypesAndEnabled[typeof(SphereCollider)] = settings.sphereColliders;
        //    ComponentTypesAndEnabled[typeof(MeshCollider)] = settings.meshColliders;

        //    return base.Prepare(target);
        //}
    }
}