using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using System.Collections.Generic;
using UnityEditor;

namespace Pumkin.AvatarTools.Implementation.Tools
{
    [AllowAutoLoad]
    class ToolsModule : UIModuleBase
    {   
        public ToolsModule(List<ISubTool> tools)
        {
            Name = "Tools";
            Description = "Various tools";
            SubTools = tools;
        }
    }
}