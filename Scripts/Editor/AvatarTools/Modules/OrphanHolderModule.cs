#if UNITY_EDITOR
using Pumkin.AvatarTools.Base;

namespace Pumkin.AvatarTools.Modules
{
    class OrphanHolderModule : UIModuleBase
    {
        public override bool IsHidden { get => SubItems.Count == 0 && ChildModules.Count == 0; }
        public OrphanHolderModule()
        {
            Name = "Uncategorized";
            Description = "Orphan tools go here";
        }
    }
}
#endif