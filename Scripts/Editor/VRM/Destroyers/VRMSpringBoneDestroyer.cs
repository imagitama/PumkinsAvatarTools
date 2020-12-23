using Pumkin.AvatarTools2.Destroyers;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRM.Destroyers
{
    [AutoLoad("destroyer_vrmSpringBones", "VRM", ParentModuleID = DefaultIDs.Modules.Destroyer)]
    public class VRMSpringBoneDestroyer : ComponentDestroyerBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Spring Bone");

        protected override GUIContent CreateGUIContent() => new GUIContent(UIDefs.Name, Icons.Bone);

        public override string[] ComponentTypesFullNames => new string[] { VRMTypes.VRMSpringBone?.FullName };
    }
}
