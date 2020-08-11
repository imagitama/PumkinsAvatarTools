using Pumkin.UnityTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.UnityTools.Implementation.Modules
{
    class OrphanHolderModule : UIModuleBase
    {
        public override bool IsHidden { get => SubTools.Count == 0 && ChildModules.Count == 0; }
        public OrphanHolderModule()
        {
            Name = "Uncategorized";
            Description = "Orphan tools go here";
        }
    }
}
