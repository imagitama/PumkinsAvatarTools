using UnityEngine;

namespace Pumkin.AvatarTools2.Interfaces
{
    public interface IComponentDestroyer : IComponentActor, IItem
    {
        bool TryDestroyComponents(GameObject target);
    }
}