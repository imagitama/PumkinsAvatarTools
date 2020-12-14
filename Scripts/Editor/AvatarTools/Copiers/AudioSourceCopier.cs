using Pumkin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools2.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.AudioSource, ParentModuleID = DefaultIDs.Modules.Copier)]
    public class AudioSourceCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => typeof(AudioSource).FullName;
    }
}
