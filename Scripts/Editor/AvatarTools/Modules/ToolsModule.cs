#if UNITY_EDITOR

using Pumkin.AvatarTools.Base;
using Pumkin.Core;
using System;
using UnityEngine;

namespace Pumkin.AvatarTools.Modules
{
    [AutoLoad(DefaultModuleIDs.TOOLS)]
    [UIDefinition("Tools", Description = "Various tools", OrderInUI = 0)]
    class ToolsModule : UIModuleBase
    {

    }
}
#endif