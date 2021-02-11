using Pumkin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools2.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.DynamicBoneCollider, ParentModuleID = DefaultIDs.Modules.Copier)]
    public class DynamicBoneColliderCopier : ComponentCopierBase
    {
        public override string[] ComponentTypesFullNames => new string[] { GenericTypes.DynamicBoneCollider?.FullName };
    }
}
