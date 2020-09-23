using Pumkin.AvatarTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Implementation.Copiers
{
    [AutoLoad("copier_dbones", ParentModuleID = DefaultModuleIDs.COPIER)]
    [UIDefinition("Dynamic Bones", OrderInUI = 1)]
    class DynamicBoneCopier : ComponentCopierBase
    {
        public override string ComponentTypeNameFull { get => "DynamicBone"; }
    }
}
