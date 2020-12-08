using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Types
{
    /// <summary>
    /// Cached VRChat types. Null if not VRC SDK in project
    /// </summary>
    static class VRChatTypes
    {
        public static readonly Type VRC_AvatarDescriptor = TypeHelpers.GetType("VRC.SDKBase.VRC_AvatarDescriptor");
        public static readonly Type PipelineManager = TypeHelpers.GetType("VRC.Core.PipelineManager");
        public static readonly Type VRC_AvatarDescriptor_LipSyncStyle = TypeHelpers.GetType("VRC.SDKBase.VRC_AvatarDescriptor+LipSyncStyle");
    }

    /// <summary>
    /// Cached generic types. Null if missing from project
    /// </summary>
    static class GenericTypes
    {
        public static readonly Type DynamicBone = TypeHelpers.GetType("DynamicBone");
        public static readonly Type DynamicBoneCollider = TypeHelpers.GetType("DynamicBone");
    }
}
