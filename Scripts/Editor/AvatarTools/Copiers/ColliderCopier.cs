using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools2.Copier
{
    [AutoLoad(DefaultIDs.Copiers.Collider, ParentModuleID = DefaultIDs.Modules.Copier)]
    class ColliderCopier : ComponentCopierBase
    {
        public override string[] ComponentTypesFullNames => new string[]
        {
            typeof(BoxCollider).FullName,
            typeof(SphereCollider).FullName,
            typeof(CapsuleCollider).FullName,
            typeof(MeshCollider).FullName,
        };

        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Colliders");

        protected override GUIContent CreateGUIContent() =>
            new GUIContent(UIDefs.Name, Icons.GetIconTextureFromType(typeof(BoxCollider)));

    }
}
