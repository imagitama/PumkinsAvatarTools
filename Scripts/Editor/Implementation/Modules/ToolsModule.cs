using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Interfaces;
using Pumkin.UnityTools.UI;
using System.Collections.Generic;
using UnityEditor;

namespace Pumkin.UnityTools.Implementation.Modules
{
    [AutoLoad("tools")]
    class ToolsModule : UIModuleBase
    {
        public ToolsModule()
        {
            Name = "Tools";
            Description = "Various tools";
            OrderInUI = 1;
        }
    }
}