#if UNITY_EDITOR
using UnityEngine;

namespace Pumkin.AvatarTools2.Interfaces
{
    public interface IComponentCopier : IItem, IComponentActor
    {
        bool Active { get; set; }

        bool TryCopyComponents(GameObject objFrom, GameObject objTo);
    }
}
#endif