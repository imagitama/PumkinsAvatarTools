using Pumkin.Core;
using UnityEngine;

namespace Pumkin.AvatarTools2.Settings
{
    class ConstraintDestroyer_Settings : SettingsContainerBase
    {
        [DrawToggleLeft] public bool positionConstraint = true;
        [DrawToggleLeft] public bool rotationConstraint = true;
        [DrawToggleLeft] public bool scaleConstraint = true;
        [Space]
        [DrawToggleLeft] public bool parentConstraint = true;
        [DrawToggleLeft] public bool aimConstraint = true;
        [DrawToggleLeft] public bool lookAtConstraint = true;
    }
}
