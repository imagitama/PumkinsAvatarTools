using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pumkin.Interfaces.ComponentCopier
{
    public interface IComponentCopier
    {
        void CopyComponents(GameObject objFrom, GameObject objTo);
    }
}