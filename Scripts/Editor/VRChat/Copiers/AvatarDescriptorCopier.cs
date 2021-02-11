using Pumkin.AvatarTools2.Copiers;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.AvatarDescriptor, "VRChat", ParentModuleID = DefaultIDs.Modules.Copier)]
    public class AvatarDescriptorCopier : ComponentCopierBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Avatar Descriptor");

        public override string[] ComponentTypesFullNames => new string[]
        {
            VRChatTypes.VRC_AvatarDescriptor?.FullName,
            VRChatTypes.PipelineManager?.FullName,
        };

        protected override GUIContent CreateGUIContent() =>
            new GUIContent(UIDefs.Name, Icons.AvatarDescriptor);
    }
}