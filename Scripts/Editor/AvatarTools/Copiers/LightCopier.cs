using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core.Attributes;
using UnityEngine;

namespace Pumkin.AvatarTools.Copiers
{
    [AutoLoad("copier_lights", ParentModuleID = DefaultModuleIDs.COPIER)]
    [UIDefinition("Lights", OrderInUI = 1)]
    class LightCopier : ComponentCopierBase
    {
        public override string ComponentTypeNameFull { get => typeof(Light).FullName; }
    }
}