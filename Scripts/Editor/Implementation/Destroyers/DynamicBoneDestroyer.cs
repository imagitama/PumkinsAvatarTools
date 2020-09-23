using Pumkin.AvatarTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Implementation.Destroyers
{
    [AutoLoad("destroyers_dynamicBone", ParentModuleID = DefaultModuleIDs.DESTROYER)]
    class DynamicBoneDestroyer : ComponentDestroyerBase
    {
        public override string ComponentTypeNameFull => "DynamicBone";
    }
}
