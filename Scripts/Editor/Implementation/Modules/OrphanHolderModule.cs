#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Implementation.Modules
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