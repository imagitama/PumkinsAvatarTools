using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pumkin.AvatarTools2.Copiers;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRM.Copiers
{
    [AutoLoad("copiers_VRMSpringBoneColliderGroup", "VRM", ParentModuleID = DefaultIDs.Modules.Copier)]
    public class VRMSpringColliderGroupCopier : ComponentCopierBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Spring Bone Collider", -2);

        public override string[] ComponentTypesFullNames => new[]{ VRMTypes.VRMSpringBoneColliderGroup?.FullName };
        protected override GUIContent CreateGUIContent()
        {
            return new GUIContent(UIDefs.Name, Icons.BoneCollider);
        }
    }
}