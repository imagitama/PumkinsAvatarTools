#if UNITY_EDITOR
using UnityEngine;

namespace Pumkin.AvatarTools2.Interfaces
{
    public interface IComponentCopier : IItem, IComponentActor
    {
        bool TryCopyComponents(GameObject objFrom, GameObject objTo);

        bool Active { get; set; }
    }
}
#endif