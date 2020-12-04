using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Destroyers
{
    [AutoLoad("destroyers_dynamicBoneCollider", ParentModuleID = DefaultModuleIDs.DESTROYER)]
    class DynamicBoneColliderDestroyer : ComponentDestroyerBase
    {
        public override string ComponentTypeFullName => "DynamicBoneCollider";
    }
}
