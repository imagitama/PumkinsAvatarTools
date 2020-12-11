using Pumkin.Core;
using UnityEngine.Animations;

namespace Pumkin.AvatarTools2.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.PositionConstraint, ParentModuleID = DefaultIDs.Modules.Copier)]
    class PositionConstraintCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => typeof(PositionConstraint).FullName;
    }
}
