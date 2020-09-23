using Pumkin.AvatarTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Implementation.Destroyers
{
    [AutoLoad("destroyers_light", ParentModuleID = DefaultModuleIDs.DESTROYER)]
    class LightDestroyer : ComponentDestroyerBase
    {
        public override string ComponentTypeNameFull => typeof(UnityEngine.Light).FullName;
    }
}