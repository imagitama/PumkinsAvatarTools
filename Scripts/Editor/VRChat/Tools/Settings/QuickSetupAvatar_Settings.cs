using Pumkin.AvatarTools2.Settings;
using Pumkin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Tools
{
    [CustomSettingsContainer(typeof(QuickSetupAvatar))]
    public class QuickSetupAvatar_Settings : SettingsContainerBase
    {
        [DrawToggleLeft] public bool setImportSettings = true;
        [Space]
        [DrawToggleLeft] public bool setAnchorsToSpine = true;
    }
}