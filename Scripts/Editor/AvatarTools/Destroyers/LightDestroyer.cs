using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core;

namespace Pumkin.AvatarTools.Destroyers
{
    [AutoLoad(DefaultIDs.Destroyers.Light, ParentModuleID = DefaultIDs.Modules.Destroyer)]
    class LightDestroyer : ComponentDestroyerBase
    {
        public override string ComponentTypeFullName => typeof(UnityEngine.Light).FullName;
    }
}