#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using Pumkin.Interfaces.ComponentDestroyer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Modules
{
    [AutoLoad(DefaultModuleIDs.DESTROYER, ParentModuleID = DefaultModuleIDs.TOOLS)]
    [UIDefinition("Remove Components", OrderInUI = 2)]
    class RemoveComponentsModule : UIModuleBase
    {
    }
}
#endif