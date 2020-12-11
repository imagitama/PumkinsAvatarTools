using Pumkin.Core;
using UnityEngine.Animations;

namespace Pumkin.AvatarTools2.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.ParentConstraint, ParentModuleID = DefaultIDs.Modules.Copier)]
    class ParentConstraintCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => typeof(ParentConstraint).FullName;
    }
}
