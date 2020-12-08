using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.Settings;
using Pumkin.Core;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;

namespace Pumkin.AvatarTools.Destroyers
{
    [AutoLoad(DefaultIDs.Destroyers.Constraint, ParentModuleID = DefaultIDs.Modules.Destroyer)]
    public class ConstraintDestroyer : MultiComponentDestroyerBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Constraints");

        public override string[] ComponentTypeFullNamesAll => new string[]
        {
            typeof(PositionConstraint).FullName,
            typeof(RotationConstraint).FullName,
            typeof(ScaleConstraint).FullName,
            typeof(ParentConstraint).FullName,
            typeof(LookAtConstraint).FullName,
            typeof(AimConstraint).FullName
        };

        public override ISettingsContainer Settings => settings;
        ConstraintDestroyer_Settings settings;

        protected override void SetupSettings()
        {
            settings = ScriptableObject.CreateInstance<ConstraintDestroyer_Settings>();
        }

        protected override bool Prepare(GameObject target)
        {
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

