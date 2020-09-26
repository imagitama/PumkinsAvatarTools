using Pumkin.AvatarTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Copiers
{
    [AutoLoad("copier_lights", ParentModuleID = DefaultModuleIDs.COPIER)]
    [UIDefinition("Lights", OrderInUI = 1)]
    class LightCopier : ComponentCopierBase
    {
        public override string ComponentTypeNameFull { get => typeof(Light).FullName; }
    }
}