using UnityEngine;

namespace Pumkin.AvatarTools.Interfaces
{
    public interface IComponentDestroyer
    {
        string ComponentTypeNameFull { get; }
        bool Prepare(GameObject target);
        bool TryDestroyComponents(GameObject target);
        void Finish(GameObject target);
    }
}
