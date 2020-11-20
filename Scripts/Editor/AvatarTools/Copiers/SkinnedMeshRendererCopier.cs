using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core;
using UnityEngine;

namespace Pumkin.AvatarTools.Copiers
{
    [AutoLoad("copier_skinnedMeshRenderer", ParentModuleID = DefaultModuleIDs.COPIER)]
    [UIDefinition("Skinned Mesh Renderers", OrderInUI = 1)]
    class SkinnedMeshRendererCopier : ComponentCopierBase
    {
        public override string ComponentTypeNameFull => typeof(SkinnedMeshRenderer).FullName;
    }
}
