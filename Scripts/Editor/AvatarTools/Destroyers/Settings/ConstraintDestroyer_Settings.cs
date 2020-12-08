using Pumkin.AvatarTools.Base;
using Pumkin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Settings
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
