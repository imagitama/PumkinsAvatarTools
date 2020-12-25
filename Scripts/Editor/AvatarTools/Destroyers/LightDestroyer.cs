using Pumkin.Core;

namespace Pumkin.AvatarTools2.Destroyers
{
    [AutoLoad(DefaultIDs.Destroyers.Light, ParentModuleID = DefaultIDs.Modules.Destroyer)]
    class LightDestroyer : ComponentDestroyerBase
    {
        public override string[] ComponentTypesFullNames { get; } = new string[] { typeof(UnityEngine.Light).FullName };
    }
}