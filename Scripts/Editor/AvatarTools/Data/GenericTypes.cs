
using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools2
{
    /// <summary>
    /// Cached generic types. Null if missing from project
    /// </summary>
    public static class GenericTypes
    {
        public static readonly Type DynamicBone = TypeHelpers.GetTypeAnwhere("DynamicBone");
        public static readonly Type DynamicBoneCollider = TypeHelpers.GetTypeAnwhere("DynamicBoneCollider");
    }
}
