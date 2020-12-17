#if UNITY_EDITOR
using Pumkin.Core;
using Pumkin.Core.UI;

namespace Pumkin.AvatarTools2.Modules
{
    [AutoLoad(DefaultIDs.Modules.Destroyer)]
    class RemoveComponentsModule : UIModuleBase
    {
        public static IgnoreList IgnoreList { get; } = new IgnoreList(PumkinTools.AvatarSelectionChanged);

        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Component Remover", 2, UIModuleStyles.DrawChildrenInHorizontalPairs);

        public override void DrawContent()
        {
            base.DrawContent();
            IgnoreList.DrawUI();
        }
    }
}
#endif