using Pumkin.AvatarTools2.Settings;
using Pumkin.Core;
using UnityEngine;

namespace Pumkin.AvatarTools2.Destroyers
{
    [CustomSettingsContainer(typeof(ColliderDestroyer))]
    class ColliderDestroyer_Settings : SettingsContainerBase
    {
        [DrawToggleLeft] public bool boxColliders = true;
        [DrawToggleLeft] public bool sphereColliders = true;
        [DrawToggleLeft] public bool capsuleColliders = true;
        [Space]
        [DrawToggleLeft] public bool meshColliders = true;
    }
}
