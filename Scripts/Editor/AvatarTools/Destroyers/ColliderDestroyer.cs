using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.Settings;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools2.Destroyers
{
    [AutoLoad(DefaultIDs.Destroyers.Collider, ParentModuleID = DefaultIDs.Modules.Destroyer)]
    class ColliderDestroyer : MultiComponentDestroyerBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Colliders");

        public override string[] ComponentTypeFullNamesAll { get; } = new string[]
        {
            typeof(BoxCollider).FullName,
            typeof(CapsuleCollider).FullName,
            typeof(SphereCollider).FullName,
            typeof(MeshCollider).FullName,
        };

        public override ISettingsContainer Settings => settings;
        ColliderDestroyer_Settings settings;

        protected override void SetupSettings()
        {
            settings = ScriptableObject.CreateInstance<ColliderDestroyer_Settings>();
        }

        protected override bool Prepare(GameObject target)
        {
            ComponentTypesAndEnabled[typeof(BoxCollider)] = settings.boxColliders;
            ComponentTypesAndEnabled[typeof(CapsuleCollider)] = settings.capsuleColliders;
            ComponentTypesAndEnabled[typeof(SphereCollider)] = settings.sphereColliders;
            ComponentTypesAndEnabled[typeof(MeshCollider)] = settings.meshColliders;

            return base.Prepare(target);
        }
    }
}