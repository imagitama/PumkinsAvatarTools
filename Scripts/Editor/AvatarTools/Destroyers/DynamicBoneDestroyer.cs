using Pumkin.Core;

namespace Pumkin.AvatarTools2.Destroyers
{
    [AutoLoad(DefaultIDs.Destroyers.DynamicBone, ParentModuleID = DefaultIDs.Modules.Destroyer)]
    class DynamicBoneDestroyer : ComponentDestroyerBase
    {
        public override string[] ComponentTypesFullNames { get; } = new string[] { GenericTypes.DynamicBone?.FullName };
    }
}
