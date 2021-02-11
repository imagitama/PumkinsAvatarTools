using Pumkin.AvatarTools2.Destroyers;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Destroyers
{
    [AutoLoad(DefaultIDs.Destroyers.AvatarDescriptor, "VRChat", ParentModuleID = DefaultIDs.Modules.Destroyer)]
    class AvatarDescriptorDestroyer : ComponentDestroyerBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Avatar Descriptor");

        public override string[] ComponentTypesFullNames { get; } = new string[]
        {
            VRChatTypes.VRC_AvatarDescriptor?.FullName,
            VRChatTypes.PipelineManager?.FullName
        };

        protected override GUIContent CreateGUIContent() => new GUIContent(UIDefs.Name, Icons.AvatarDescriptor);
    }
}