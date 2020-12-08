#if UNITY_EDITOR
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;
using Pumkin.Core.UI;

namespace Pumkin.AvatarTools.Modules
{
    [AutoLoad(DefaultIDs.Modules.Tools_SetupAvatar, ParentModuleID = DefaultIDs.Modules.Tools)]
    class SetupAvatarModule : UIModuleBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Setup Avatar", 0, UIModuleStyles.DrawChildrenInHorizontalPairs);
    }
}
#endif