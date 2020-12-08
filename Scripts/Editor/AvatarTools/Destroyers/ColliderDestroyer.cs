using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.Settings;
using Pumkin.Core;
using Pumkin.Core.UI;
using System;
using UnityEngine;

namespace Pumkin.AvatarTools.Destroyers
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
            componentTypesAndEnabled[typeof(BoxCollider)] = settings.boxColliders;
            componentTypesAndEnabled[typeof(CapsuleCollider)] = settings.capsuleColliders;
            componentTypesAndEnabled[typeof(SphereCollider)] = settings.sphereColliders;
            componentTypesAndEnabled[typeof(MeshCollider)] = settings.meshColliders;

            return base.Prepare(target);
        }
    }
}