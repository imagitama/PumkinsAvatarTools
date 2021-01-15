#if UNITY_EDITOR
using UnityEngine;

namespace Pumkin.AvatarTools2.Interfaces
{
    public interface IComponentCopier : IComponentActor, IItem
    {
        bool Active { get; set; }

        bool TryCopyComponents(GameObject objFrom, GameObject objTo);
    }
}
#endif