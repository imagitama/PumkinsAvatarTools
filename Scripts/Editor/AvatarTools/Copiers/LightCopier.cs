using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.Light, ParentModuleID = DefaultIDs.Modules.Copier)]
    class LightCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => typeof(Light).FullName;

        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Lights", 2);
    }
}