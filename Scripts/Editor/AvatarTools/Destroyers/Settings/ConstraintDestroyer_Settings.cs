using Pumkin.AvatarTools2.Settings;
using Pumkin.Core;
using UnityEngine;
using UnityEngine.Animations;

namespace Pumkin.AvatarTools2.Destroyers
{
    [CustomSettingsContainer(typeof(ConstraintDestroyer))]
    class ConstraintDestroyer_Settings : SettingsContainerBase
    {
        [DrawToggleLeft][TypeEnablerField(typeof(PositionConstraint))]
        public bool positionConstraint = true;
        [DrawToggleLeft][TypeEnablerField(typeof(RotationConstraint))]
        public bool rotationConstraint = true;
        [DrawToggleLeft][TypeEnablerField(typeof(ScaleConstraint))]
        public bool scaleConstraint = true;

        [Space]

        [DrawToggleLeft][TypeEnablerField(typeof(ParentConstraint))]
        public bool parentConstraint = true;
        [DrawToggleLeft][TypeEnablerField(typeof(AimConstraint))]
        public bool aimConstraint = true;
        [DrawToggleLeft][TypeEnablerField(typeof(LookAtConstraint))]
        public bool lookAtConstraint = true;
    }
}