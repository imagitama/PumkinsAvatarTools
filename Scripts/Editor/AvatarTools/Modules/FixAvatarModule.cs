#if UNITY_EDITOR
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.UI;
using Pumkin.Core.Attributes;

namespace Pumkin.AvatarTools.Modules
{
    [AutoLoad(DefaultModuleIDs.TOOLS_FIX_AVATAR, ParentModuleID = DefaultModuleIDs.TOOLS)]
    [UIDefinition("Fix Avatar", UIModuleStyles.DrawChildrenInHorizontalPairs, OrderInUI = 1)]
    class FixAvatarModule : UIModuleBase
    {
    }
}
#endif