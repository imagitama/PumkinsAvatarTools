using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.Settings;
using Pumkin.AvatarTools2.Tools;
using Pumkin.Core;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Tools
{
    [AutoLoad("tools_quickSetupAvatar", "VRChat", ParentModuleID = DefaultIDs.Modules.Tools_SetupAvatar)]
    public class QuickSetupAvatar : ToolBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Quick Setup Avatar", -1);

        protected override bool DoAction(GameObject target)
        {
            return true;
        }
    }

    [CustomSettingsContainer(typeof(QuickSetupAvatar))]
    public class QuickSetupAvatar_Settings : SettingsContainerBase
    {

    }
}
