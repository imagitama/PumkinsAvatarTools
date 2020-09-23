#if UNITY_EDITOR
using Pumkin.AvatarTools.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pumkin.Interfaces.ComponentCopier
{
    public interface IComponentCopier : ISubItem
    {
        string ComponentTypeNameFull { get; }
        bool TryCopyComponents(GameObject objFrom, GameObject objTo);

        bool Active { get; set; }
    }
}
#endif