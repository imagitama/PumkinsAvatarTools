#if UNITY_EDITOR
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;

namespace Pumkin.AvatarTools.Modules
{
    [AutoLoad(DefaultModuleIDs.DESTROYER, ParentModuleID = DefaultModuleIDs.TOOLS)]
    [UIDefinition("Remove Components", UIModuleStyles.DrawChildrenInHorizontalPairs, OrderInUI = 2)]
    class RemoveComponentsModule : UIModuleBase
    {
    }
}
#endif