using Pumkin.AvatarTools2.Copiers;
using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.UI;
using Pumkin.AvatarTools2.VRChat.Settings;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.AvatarDescriptor, "vrchat", ParentModuleID = DefaultIDs.Modules.Copier)]
    class AvatarDescriptorCopier : MultiComponentPropertyCopierBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Avatar Descriptor");

        public override string[] ComponentTypeFullNamesAll => new string[]
        {
             VRChatTypes.VRC_AvatarDescriptor?.FullName,
             VRChatTypes.PipelineManager?.FullName,
        };

        protected override GUIContent CreateGUIContent() =>
            new GUIContent(UIDefs.Name, Icons.AvatarDescriptor.image);

        public override ISettingsContainer Settings => settings;

        AvatarDescriptorCopier_Settings settings;

        protected override void SetupSettings()
        {
            settings = ScriptableObject.CreateInstance<AvatarDescriptorCopier_Settings>();
        }
    }
}
