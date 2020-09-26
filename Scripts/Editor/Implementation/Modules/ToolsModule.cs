#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using System.Collections.Generic;
using UnityEditor;

namespace Pumkin.AvatarTools.Implementation.Modules
{
    [AutoLoad(DefaultModuleIDs.TOOLS)]
    [UIDefinition("Tools", Description = "Various tools", OrderInUI = 0)]
    class ToolsModule : UIModuleBase
    {

    }
}
#endif