using Pumkin.AvatarTools2.Copiers;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.AudioSource, "VRChat", ParentModuleID = DefaultIDs.Modules.Copier)]
    public class AudioSourceCopier : ComponentCopierBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("[VRC] Audio Source Copier");
        public override string[] ComponentTypesFullNames { get; } = new string[]
        {
            typeof(AudioSource).FullName,
            VRChatTypes.ONSPAudioSource?.FullName,
            VRChatTypes.VRC_SpacialAudioSource?.FullName,
        };
    }
}
