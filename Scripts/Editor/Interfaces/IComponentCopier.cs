#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pumkin.Interfaces.ComponentCopier
{
    public interface IComponentCopier
    {
        string ComponentName { get; set; }
        bool TryCopyComponents(GameObject objFrom, GameObject objTo);
    }
}
#endif