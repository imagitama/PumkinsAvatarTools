using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools.Copiers
{
    [AutoLoad("copier_light", ParentModuleID = DefaultModuleIDs.COPIER)]
    class LightCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => typeof(Light).FullName;

        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Lights", 2);
    }
}