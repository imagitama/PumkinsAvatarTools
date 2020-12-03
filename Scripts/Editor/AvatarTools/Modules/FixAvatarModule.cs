#if UNITY_EDITOR
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;
using Pumkin.Core.UI;

namespace Pumkin.AvatarTools.Modules
{
    [AutoLoad(DefaultModuleIDs.TOOLS_FIX_AVATAR, ParentModuleID = DefaultModuleIDs.TOOLS)]
    class FixAvatarModule : UIModuleBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Fix Avatar", 1, UIModuleStyles.DrawChildrenInHorizontalPairs);
    }
}
#endif