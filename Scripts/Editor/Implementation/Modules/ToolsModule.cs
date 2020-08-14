#if UNITY_EDITOR
using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Interfaces;
using Pumkin.UnityTools.UI;
using System.Collections.Generic;
using UnityEditor;

namespace Pumkin.UnityTools.Implementation.Modules
{
    [AutoLoad("tools")]
    [UIDefinition("Tools", Description = "Various tools", OrderInUI = 1)]
    class ToolsModule : UIModuleBase
    {
        
    }
}
#endif