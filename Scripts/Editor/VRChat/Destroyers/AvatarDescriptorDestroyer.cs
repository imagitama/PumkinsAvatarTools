using Pumkin.AvatarTools2.Destroyers;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Destroyers
{
    [AutoLoad(DefaultIDs.Copiers.AvatarDescriptor, "vrchat", ParentModuleID = DefaultIDs.Modules.Destroyer)]
    class AvatarDescriptorDestroyer : MultiComponentDestroyerBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Avatar Descriptor");

        public override string[] ComponentTypeFullNamesAll { get; } = new string[]
        {
            VRChatTypes.VRC_AvatarDescriptor?.FullName,
            VRChatTypes.PipelineManager?.FullName
        };

        protected override GUIContent CreateGUIContent()
        {
            return new GUIContent(UIDefs.Name, Icons.AvatarDescriptor.image);
        }
    }
}