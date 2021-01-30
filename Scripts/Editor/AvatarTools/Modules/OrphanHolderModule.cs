using Pumkin.Core.UI;

namespace Pumkin.AvatarTools2.Modules
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