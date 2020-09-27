#if UNITY_EDITOR
using Pumkin.AvatarTools.Base;
using Pumkin.Core.Attributes;

namespace Pumkin.AvatarTools.Settings
{
    internal class DynamicBoneCopier_Settings : CopierSettingsContainerBase
    {
        [DrawToggleLeft]
        public bool removeOldBones = false;
        [DrawToggleLeft]
        public bool createGameObjects = false;
    }
}
#endif