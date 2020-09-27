#if UNITY_EDITOR
using UnityEngine;

namespace Pumkin.AvatarTools.Interfaces
{
    public interface IComponentCopier : IItem
    {
        string ComponentTypeNameFull { get; }
        bool TryCopyComponents(GameObject objFrom, GameObject objTo);

        bool Active { get; set; }
    }
}
#endif