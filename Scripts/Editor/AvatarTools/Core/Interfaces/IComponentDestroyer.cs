using System;
using UnityEngine;

namespace Pumkin.AvatarTools2.Interfaces
{
    public interface IComponentDestroyer : IComponentActor, IItem
    {
        bool TryDestroyComponents(GameObject target);
        bool DoDestroyByType(GameObject target, Type type);
    }
}