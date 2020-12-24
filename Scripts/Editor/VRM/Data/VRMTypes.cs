using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools2.VRM
{
    public static class VRMTypes
    {
        public static readonly Type VRMSpringBone = TypeHelpers.GetTypeAnwhere("VRM.VRMSpringBone");
        public static readonly Type VRMSpringBoneColliderGroup = TypeHelpers.GetTypeAnwhere("VRM.VRMSpringBoneColliderGroup");
    }
}
