#if UNITY_EDITOR
using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Implementation.Modules;
using Pumkin.UnityTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.UnityTools.Implementation.Modules
{
    [AutoLoad("tools_setupAvatar", ParentModuleID = "tools")]
    [UIDefinition("Setup Avatar", OrderInUI = 0)]
    class SetupAvatarModule : UIModuleBase
    {

    }
}
#endif