using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pumkin.AvatarTools2.Interfaces
{
    public interface IComponentActor
    {
        string[] ComponentTypesFullNames { get; }

        Type FirstValidType { get; }
    }
}