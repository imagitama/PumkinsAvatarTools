using Pumkin.Core;
using UnityEngine.Animations;

namespace Pumkin.AvatarTools2.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.RotationConstraint, ParentModuleID = DefaultIDs.Modules.Copier)]
    class RotationConstraintCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => typeof(RotationConstraint).FullName;
    }
}
