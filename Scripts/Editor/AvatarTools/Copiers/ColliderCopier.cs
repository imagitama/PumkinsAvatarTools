using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools;
using Pumkin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.Collider)]
    class ColliderCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => "Collider";
    }
}
