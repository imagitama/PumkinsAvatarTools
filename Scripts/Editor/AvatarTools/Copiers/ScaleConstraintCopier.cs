using Pumkin.AvatarTools.Base;
using Pumkin.Core;
using UnityEngine.Animations;

namespace Pumkin.AvatarTools.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.ScaleConstraint, ParentModuleID = DefaultIDs.Modules.Copier)]
    class ScaleConstraintCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => typeof(ScaleConstraint).FullName;
    }
}
