using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core;
using UnityEngine;

namespace Pumkin.AvatarTools.Copiers
{
    [AutoLoad("copier_light", ParentModuleID = DefaultModuleIDs.COPIER)]
    [UIDefinition("Lights", OrderInUI = 1)]
    class LightCopier : ComponentCopierBase
    {
        public override string ComponentTypeNameFull => typeof(Light).FullName;
    }
}