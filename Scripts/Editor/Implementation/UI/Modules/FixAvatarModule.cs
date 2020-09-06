#if UNITY_EDITOR
using Pumkin.UnityTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.UnityTools.Implementation.Modules
{
    [AutoLoad("tools_fixAvatar", ParentModuleID = "tools")]
    [UIDefinition("Fix Avatar", OrderInUI = 1)]
    class FixAvatarModule : UIModuleBase
    {
    }
}
#endif