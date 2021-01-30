using Pumkin.AvatarTools2.Settings;
using Pumkin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools2.Copiers
{
    [CustomSettingsContainer(typeof(ColliderCopier))]
    class ColliderCopier_Settings : CopierSettingsContainerBase
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

        public override PropertyDefinitions Properties => null;
    }
}