using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.Core
{
    /// <summary>
    /// Tells a SettingsContainer what IItem it should be created for
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CustomSettingsContainerAttribute : Attribute
    {
        public Type OwnerType { get; private set; }

        public CustomSettingsContainerAttribute(Type usedByType)
        {
            OwnerType = usedByType;
        }
    }
}
