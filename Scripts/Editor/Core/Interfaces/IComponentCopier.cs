#if UNITY_EDITOR
using UnityEngine;

namespace Pumkin.AvatarTools.Interfaces
{
    public interface IComponentCopier : IItem, IComponentActor
    {
        bool TryCopyComponents(GameObject objFrom, GameObject objTo);

        bool Active { get; set; }
    }
}
#endif