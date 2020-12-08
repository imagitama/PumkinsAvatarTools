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
    class ColliderDestroyer_Settings : SettingsContainerBase
    {
        [DrawToggleLeft] public bool boxColliders = true;
        [DrawToggleLeft] public bool sphereColliders = true;
        [DrawToggleLeft] public bool capsuleColliders = true;
        [Space]
        [DrawToggleLeft] public bool meshColliders = true;
    }
}
