using Pumkin.AvatarTools2.Settings;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using UnityEditor;

namespace Pumkin.AvatarTools2.Tools
{
    class SetTPose_Settings : SettingsContainerBase
    {
        [DrawToggleLeft]
        public bool useAPose = false;
    }
}
