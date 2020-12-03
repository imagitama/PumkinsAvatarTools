#if UNITY_EDITOR
using Pumkin.AvatarTools.Base;
using Pumkin.Core.UI;

namespace Pumkin.AvatarTools.Modules
{
    class OrphanHolderModule : UIModuleBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Uncategorized", "Orphan tools go here");

        public OrphanHolderModule()
        {
            UIDefs.IsHidden = SubItems.Count == 0 && ChildModules.Count == 0;
        }
    }
}
#endif