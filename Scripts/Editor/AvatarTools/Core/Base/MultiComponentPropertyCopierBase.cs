using Pumkin.AvatarTools2.Copiers;

namespace Pumkin.AvatarTools2.Copiers
{
    //TODO: Figure this one out
    public abstract class MultiComponentPropertyCopierBase : ComponentCopierBase
    {
        public override string ComponentTypeFullName => ComponentTypeFullNamesAll[0];

        public abstract string[] ComponentTypeFullNamesAll { get; }
    }
}