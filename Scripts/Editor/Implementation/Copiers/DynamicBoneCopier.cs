using Pumkin.UnityTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.UnityTools.Implementation.Copiers
{
    [AutoLoad("copier_dbones", ParentModuleID = "copiers")]
    [UIDefinition("Dynamic Bone Copier")]
    class DynamicBoneCopier : ComponentCopierBase
    {
        public override string ComponentTypeNameFull { get => "DynamicBone"; }
    }
}
