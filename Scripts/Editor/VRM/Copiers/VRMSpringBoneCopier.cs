using Pumkin.AvatarTools2.Copiers;
using Pumkin.AvatarTools2.UI;
using Pumkin.AvatarTools2.VRM;
using Pumkin.Core;
using Pumkin.Core.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRM.Copiers
{
    [AutoLoad("copier_VRMSpringBone", "VRM", ParentModuleID = DefaultIDs.Modules.Copier)]
    public class VRMSpringBoneCopier : ComponentCopierBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Spring Bone", -1);

        public override string ComponentTypeFullName => VRMTypes.VRMSpringBone?.FullName;
        protected override GUIContent CreateGUIContent()
        {
            return new GUIContent(UIDefs.Name, Icons.Bone);
        }
    }
}