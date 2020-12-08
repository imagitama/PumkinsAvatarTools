using Pumkin.AvatarTools.Base;
using Pumkin.Core;
using Pumkin.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

namespace Pumkin.AvatarTools.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.ParentConstraint, ParentModuleID = DefaultIDs.Modules.Copier)]
    class ParentConstraintCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => typeof(ParentConstraint).FullName;
    }
}
