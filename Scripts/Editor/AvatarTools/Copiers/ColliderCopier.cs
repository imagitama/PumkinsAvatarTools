using Pumkin.Core;

namespace Pumkin.AvatarTools2.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.Collider)]
    class ColliderCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => "Collider";
    }
}
