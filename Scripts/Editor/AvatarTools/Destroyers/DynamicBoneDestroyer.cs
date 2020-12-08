using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core;
using Pumkin.Core.UI;

namespace Pumkin.AvatarTools.Destroyers
{
    [AutoLoad(DefaultIDs.Destroyers.DynamicBone, ParentModuleID = DefaultIDs.Modules.Destroyer)]
    class DynamicBoneDestroyer : ComponentDestroyerBase
    {
         public override string ComponentTypeFullName => "DynamicBone";
    }
}
