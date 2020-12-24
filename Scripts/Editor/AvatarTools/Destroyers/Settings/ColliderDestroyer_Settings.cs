using Pumkin.AvatarTools2.Settings;
using Pumkin.Core;
using UnityEngine;

namespace Pumkin.AvatarTools2.Destroyers
{
    [CustomSettingsContainer(typeof(ColliderDestroyer))]
    class ColliderDestroyer_Settings : SettingsContainerBase
    {
        [DrawToggleLeft][TypeEnablerField(typeof(BoxCollider))]
        public bool boxColliders = true;

        [DrawToggleLeft][TypeEnablerField(typeof(SphereCollider))]
        public bool sphereColliders = true;

        [DrawToggleLeft][TypeEnablerField(typeof(CapsuleCollider))]
        public bool capsuleColliders = true;

        [Space]

        [DrawToggleLeft][TypeEnablerField(typeof(MeshCollider))]
        public bool meshColliders = true;
    }
}
