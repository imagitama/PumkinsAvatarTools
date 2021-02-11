using Pumkin.Core;

namespace Pumkin.AvatarTools2.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.DynamicBone, ParentModuleID = DefaultIDs.Modules.Copier)]
    class DynamicBoneCopier : ComponentCopierBase
    {
        public override string[] ComponentTypesFullNames => new string[] { GenericTypes.DynamicBone?.FullName };
    }
}
