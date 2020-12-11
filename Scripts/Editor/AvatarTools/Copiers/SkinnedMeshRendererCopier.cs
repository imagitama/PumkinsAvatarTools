using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools2.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.SkinnedMeshRenderer, ParentModuleID = DefaultIDs.Modules.Copier)]
    class SkinnedMeshRendererCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => typeof(SkinnedMeshRenderer).FullName;

        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Skinned Mesh Renderers");
    }
}
