using Pumkin.AvatarTools2.Interfaces;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools2.Settings
{
    public static class IItemExtensions
    {
        static Dictionary<Type, CustomSettingsContainerAttribute> CachedTypesAndAttributes
        {
            get
            {
                if(_cachedTypesAndAttributes == null)
                    _cachedTypesAndAttributes = TypeHelpers.GetTypesAndAttributesWithAttribute<CustomSettingsContainerAttribute>();
                return _cachedTypesAndAttributes;
            }
        }

        public static ISettingsContainer GetSettingsContainer(this IItem item)
        {
            var itemType = item.GetType();
            var settingsType = CachedTypesAndAttributes.FirstOrDefault(t => t.Value.OwnerType == itemType).Key;

            if(settingsType != null)
                return ScriptableObject.CreateInstance(settingsType) as ISettingsContainer;
            return null;
        }

        static Dictionary<Type, CustomSettingsContainerAttribute> _cachedTypesAndAttributes;
    }
}
