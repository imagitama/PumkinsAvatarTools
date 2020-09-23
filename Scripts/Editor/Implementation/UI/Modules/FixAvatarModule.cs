#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Implementation.Modules
{
    [AutoLoad(DefaultModuleIDs.TOOLS_FIX_AVATAR, ParentModuleID = DefaultModuleIDs.TOOLS)]
    [UIDefinition("Fix Avatar", OrderInUI = 1)]
    class FixAvatarModule : UIModuleBase
    {
    }
}
#endif