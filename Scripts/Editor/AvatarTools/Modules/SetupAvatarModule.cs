#if UNITY_EDITOR
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;

namespace Pumkin.AvatarTools.Modules
{
    [AutoLoad(DefaultModuleIDs.TOOLS_SETUP_AVATAR, ParentModuleID = DefaultModuleIDs.TOOLS)]
    [UIDefinition("Setup Avatar", UIModuleStyles.DrawChildrenInHorizontalPairs, OrderInUI = 0)]
    class SetupAvatarModule : UIModuleBase
    {

    }
}
#endif