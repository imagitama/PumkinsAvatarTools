using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.Modules;
using Pumkin.AvatarTools.Settings;
using Pumkin.AvatarTools.Types;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Destroyers
{
    [AutoLoad(DefaultIDs.Copiers.AvatarDescriptor, "vrchat", ParentModuleID = DefaultIDs.Modules.Destroyer)]
    class AvatarDescriptorDestroyer_VRChat : MultiComponentDestroyerBase
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