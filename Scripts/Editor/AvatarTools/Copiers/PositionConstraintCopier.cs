using Pumkin.AvatarTools.Base;
using Pumkin.Core;
using UnityEngine.Animations;

namespace Pumkin.AvatarTools.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.PositionConstraint, ParentModuleID = DefaultIDs.Modules.Copier)]
    class PositionConstraintCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => typeof(PositionConstraint).FullName;
    }
}
