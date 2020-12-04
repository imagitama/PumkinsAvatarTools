using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core;

namespace Pumkin.AvatarTools.Destroyers
{
    [AutoLoad("destroyers_light", ParentModuleID = DefaultModuleIDs.DESTROYER)]
    class LightDestroyer : ComponentDestroyerBase
    {
        public override string ComponentTypeFullName => typeof(UnityEngine.Light).FullName;
    }
}