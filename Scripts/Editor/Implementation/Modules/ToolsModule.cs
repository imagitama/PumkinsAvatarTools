using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using System.Collections.Generic;
using UnityEditor;

namespace Pumkin.AvatarTools.Implementation.Modules
{
    [AllowAutoLoad]
    [ModuleID("main_tools")]
    class ToolsModule : UIModuleBase
    {
        public ToolsModule()
        {
            Name = "Tools";
            Description = "Various tools";
        }          
    }
}