#if UNITY_EDITOR

using Pumkin.Core;
using Pumkin.Core.UI;

namespace Pumkin.AvatarTools2.Modules
{
    [AutoLoad(DefaultIDs.Modules.Tools)]
    class ToolsModule : UIModuleBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Tools", "Various Tools", 0);
    }
}
#endif