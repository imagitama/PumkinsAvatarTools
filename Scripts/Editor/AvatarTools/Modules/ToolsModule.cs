#if UNITY_EDITOR

using Pumkin.AvatarTools.Base;
using Pumkin.Core.Attributes;

namespace Pumkin.AvatarTools.Modules
{
    [AutoLoad(DefaultModuleIDs.TOOLS)]
    [UIDefinition("Tools", Description = "Various tools", OrderInUI = 0)]
    class ToolsModule : UIModuleBase
    {

    }
}
#endif