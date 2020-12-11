using Pumkin.Core.Helpers;
using System;

namespace Pumkin.AvatarTools2.Types
{
    /// <summary>
    /// Cached VRChat types. Null if not VRC SDK in project
    /// </summary>
    public static class VRChatTypes
    {
        public static readonly Type VRC_AvatarDescriptor = TypeHelpers.GetType("VRC.SDKBase.VRC_AvatarDescriptor");
        public static readonly Type PipelineManager = TypeHelpers.GetType("VRC.Core.PipelineManager");
        public static readonly Type VRC_AvatarDescriptor_LipSyncStyle = TypeHelpers.GetType("VRC.SDKBase.VRC_AvatarDescriptor+LipSyncStyle");
    }
}
