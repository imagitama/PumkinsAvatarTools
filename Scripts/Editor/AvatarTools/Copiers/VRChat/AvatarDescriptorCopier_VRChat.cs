using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;
using UnityEngine;

namespace Pumkin.AvatarTools.Copiers
{
    [UIDefinition("Avatar Descriptor")]
    [AutoLoad("copier_avatarDescriptor", "vrchat", ParentModuleID = DefaultModuleIDs.COPIER)]
    class AvatarDescriptorCopier_VRChat : ComponentCopierBase
    {
        public override string ComponentTypeNameFull => "VRC.SDKBase.VRC_AvatarDescriptor";
        protected override GUIContent CreateGUIContent()
        {//TODO: Sort this one out
            return new GUIContent(Name, Icons.AvatarDescriptor.image);
        }
    }
}
