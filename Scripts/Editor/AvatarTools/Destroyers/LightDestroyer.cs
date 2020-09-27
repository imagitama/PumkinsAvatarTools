using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core.Attributes;

namespace Pumkin.AvatarTools.Destroyers
{
    [AutoLoad("destroyers_light", ParentModuleID = DefaultModuleIDs.DESTROYER)]
    class LightDestroyer : ComponentDestroyerBase
    {
        public override string ComponentTypeNameFull => typeof(UnityEngine.Light).FullName;
    }
}