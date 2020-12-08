using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.SkinnedMeshRenderer, ParentModuleID = DefaultIDs.Modules.Copier)]
    class SkinnedMeshRendererCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => typeof(SkinnedMeshRenderer).FullName;

        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Skinned Mesh Renderers");
    }
}
