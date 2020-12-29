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
        static Dictionary<IItem, ISettingsContainer> CachedItemsAndSettings = new Dictionary<IItem, ISettingsContainer>();

        public static ISettingsContainer GetSettingsContainer(this IItem item)
        {
            var itemType = item.GetType();
            var settingsType = CachedTypesAndAttributes.FirstOrDefault(t => t.Value.OwnerType == itemType).Key;
            ISettingsContainer settings = null;

            if(settingsType != null && settings == null)
                settings = ScriptableObject.CreateInstance(settingsType) as ISettingsContainer;

            CachedItemsAndSettings[item] = settings;

            return settings;
        }

        static Dictionary<Type, CustomSettingsContainerAttribute> _cachedTypesAndAttributes;
    }
}
