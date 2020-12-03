#if UNITY_EDITOR
using System.Collections;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Pumkin.AvatarTools.Modules
{
    static class IDManager
    {
        static readonly Dictionary<string, IUIModule> ModuleCache = new Dictionary<string, IUIModule>();
        static readonly Dictionary<string, IItem> ItemCache = new Dictionary<string, IItem>();

        public static ReadOnlyDictionary<string, IUIModule> Modules
            => new ReadOnlyDictionary<string, IUIModule>(ModuleCache);

        public static ReadOnlyDictionary<string, IItem> Items
            => new ReadOnlyDictionary<string, IItem>(ItemCache);

        public static void ClearCache()
        {
            ModuleCache.Clear();
            ItemCache.Clear();
        }

        static bool FindAndReplaceInModule(string id, IItem newItem)
        {
            var oldItem = ItemCache[id];
            if(oldItem == null)
                return false;

            //Find module that has oldItem
            IUIModule module = ModuleCache.Values
                .FirstOrDefault(m => m.SubItems.Contains(oldItem));

            if(module == default)
            {
                PumkinTools.LogVerbose($"{newItem.UIDefs.Name} is registered but doesn't belong to a module", LogType.Warning);
                return false;
            }

            return module.ReplaceSubItem(newItem, oldItem);
        }

        public static bool Register(IUIModule module)
        {
            if(module == null)
                return false;
            var typ = module.GetType();
            var attr = typ.GetCustomAttribute<AutoLoadAttribute>();

            if(attr == null)
            {
                Debug.LogWarning($"Trying to register module without AutoLoad attribute. Aborting");
                return false;
            }
            if(ModuleCache.TryGetValue(attr.ID.ToUpperInvariant(), out _))
            {
                Debug.LogWarning($"Module with ID '{attr.ID}' has already been created");
                return false;
            }

            ModuleCache.Add(attr.ID.ToUpper(), module);
            return true;
        }

        public static bool Register(IItem item)
        {
            if(item == null)
                return false;
            var typ = item.GetType();
            var attr = typ.GetCustomAttribute<AutoLoadAttribute>();

            if(attr == null)
            {
                Debug.LogWarning($"Trying to register item without AutoLoad attribute. Aborting");
                return false;
            }
            if(ItemCache.TryGetValue(attr.ID.ToUpperInvariant(), out var oldItem))
            {
                var oldAttr = oldItem.GetType().GetCustomAttribute<AutoLoadAttribute>();
                if(attr.IsGenericItem && !oldAttr.IsGenericItem) //Should only replace generic items
                {
                    Debug.LogWarning($"Item with ID '{attr.ID}' has already been created");
                    return false;
                }
                FindAndReplaceInModule(attr.ID, item);
            }

            ItemCache[attr.ID.ToUpper()] = item;
            return true;
        }

        public static IUIModule GetModule(string id)
        {
            if(id == null)
                id = "";

            ModuleCache.TryGetValue(id.ToUpperInvariant(), out IUIModule value);
            return value;
        }

        public static IItem GetItem(string id)
        {
            if(id == null)
                id = "";

            ItemCache.TryGetValue(id.ToUpperInvariant(), out IItem value);
            return value;
        }

        public static bool Registered(string id)
        {
            return ItemCache.ContainsKey(id);
        }
    }
}
#endif