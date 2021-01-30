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

        public static ISettingsContainer GetOrCreateSettingsContainer(this IItem item)
        {
            var itemType = item.GetType();
            var settingsType = CachedTypesAndAttributes.FirstOrDefault(t => t.Value.OwnerType == itemType).Key;
            ISettingsContainer settings = null;

            if(settingsType != null && settings == null)
            {
                //Clean up if exists
                settings = GameObject.FindObjectOfType(settingsType) as ISettingsContainer;
                UnityObjectHelpers.DestroyAppropriate(settings as ScriptableObject);
            }
            settings = ScriptableObject.CreateInstance(settingsType) as ISettingsContainer;

            CachedItemsAndSettings[item] = settings;

            //if(CachedItemsAndSettings.ContainsKey(item) && settings == null)
            //    CachedItemsAndSettings.Remove(item);

            //if(settings != null)
            //    CachedItemsAndSettings[item] = settings;

            return settings;
        }

        static Dictionary<Type, CustomSettingsContainerAttribute> _cachedTypesAndAttributes;
    }
}