using UnityEngine;

namespace Pumkin.AvatarTools.Interfaces
{
    public interface IComponentDestroyer : IComponentActor, IItem
    {
        bool TryDestroyComponents(GameObject target);
    }
}
