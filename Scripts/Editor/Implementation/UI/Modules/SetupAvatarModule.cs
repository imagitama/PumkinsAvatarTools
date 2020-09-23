#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Implementation.Modules;
using Pumkin.AvatarTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Implementation.Modules
{
    [AutoLoad(DefaultModuleIDs.TOOLS_SETUP_AVATAR, ParentModuleID = DefaultModuleIDs.TOOLS)]
    [UIDefinition("Setup Avatar", OrderInUI = 0)]
    class SetupAvatarModule : UIModuleBase
    {

    }
}
#endif