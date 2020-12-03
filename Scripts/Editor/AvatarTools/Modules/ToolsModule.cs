#if UNITY_EDITOR

using Pumkin.AvatarTools.Base;
using Pumkin.Core;
using Pumkin.Core.UI;
using System;
using UnityEngine;

namespace Pumkin.AvatarTools.Modules
{
    [AutoLoad(DefaultModuleIDs.TOOLS)]
    class ToolsModule : UIModuleBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Tools", "Various Tools", 0);
    }
}
#endif