using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools2.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.Light, ParentModuleID = DefaultIDs.Modules.Copier)]
    class LightCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => typeof(Light).FullName;
    }
}