using Pumkin.AvatarTools.Base;
using Pumkin.Core.Attributes;

namespace Pumkin.AvatarTools.Settings
{
    class ForceTPose_Settings : SettingsContainerBase
    {
        [DrawToggleLeft]
        public bool forceAPose = false;
    }
}
