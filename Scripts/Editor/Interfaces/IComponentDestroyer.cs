using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.Interfaces.ComponentDestroyer
{
    public interface IComponentDestroyer
    {
        string ComponentTypeNameFull { get; }
        bool Prepare(GameObject target);
        bool TryDestroyComponents(GameObject target);
        void Finish(GameObject target);
    }
}
