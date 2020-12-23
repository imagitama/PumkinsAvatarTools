using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.Settings;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;
using UnityEngine.Animations;

namespace Pumkin.AvatarTools2.Destroyers
{
    [AutoLoad(DefaultIDs.Destroyers.Constraint, ParentModuleID = DefaultIDs.Modules.Destroyer)]
    public class ConstraintDestroyer : ComponentDestroyerBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Constraints");

        public override string[] ComponentTypesFullNames => new string[]
        {
            typeof(PositionConstraint).FullName,
            typeof(RotationConstraint).FullName,
            typeof(ScaleConstraint).FullName,
            typeof(ParentConstraint).FullName,
            typeof(LookAtConstraint).FullName,
            typeof(AimConstraint).FullName
        };

        protected override bool Prepare(GameObject target)
        {
            var settings = Settings as ConstraintDestroyer_Settings;

            ComponentTypesAndEnabled[typeof(PositionConstraint)] = settings.positionConstraint;
            ComponentTypesAndEnabled[typeof(RotationConstraint)] = settings.rotationConstraint;
            ComponentTypesAndEnabled[typeof(ScaleConstraint)] = settings.scaleConstraint;
            ComponentTypesAndEnabled[typeof(ParentConstraint)] = settings.parentConstraint;
            ComponentTypesAndEnabled[typeof(LookAtConstraint)] = settings.lookAtConstraint;
            ComponentTypesAndEnabled[typeof(AimConstraint)] = settings.aimConstraint;

            return base.Prepare(target);
        }
    }
}

