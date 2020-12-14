using Pumkin.AvatarTools2.Copiers;
using Pumkin.AvatarTools2.UI;
using Pumkin.AvatarTools2.VRM;
using Pumkin.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRM.Copiers
{
    [AutoLoad("copiers_VRM_SpringBones", "vrm", ParentModuleID = DefaultIDs.Modules.Copier)]
    public class VRM_SpringBoneCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => VRMTypes.VRMSpringBone?.FullName;
        protected override GUIContent CreateGUIContent()
        {
            return new GUIContent(UIDefs.Name, Icons.Bone.image);
        }
    }
}