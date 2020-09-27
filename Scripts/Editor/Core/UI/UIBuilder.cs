#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pumkin.AvatarTools.Interfaces;
using UnityEngine;
using Pumkin.Core.Helpers;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core.Attributes;

namespace Pumkin.AvatarTools.UI
{
    static class UIBuilder
    {
        static List<Type> typeCache;
        static List<Type> subItemTypeCache;
        static List<Type> moduleTypeCache;

        public static MainUI BuildUI()
        {
            MainUI UI = new MainUI();
            ModuleIDManager.ClearCache();

            RefreshCachedTypes(PumkinTools.ConfigurationString);

            //Build modules from typeCache
            foreach(var t in moduleTypeCache)
            {
                IUIModule mod = null;
                try
                {
                    mod = BuildModule(t);
                }
                catch(Exception e)
                {
                    Debug.LogError($"{(t?.Name ?? "Unknown module")} threw an exception: {e.Message}");
                }
                finally
                {
                    if(mod != null)
                        UI.UIModules.Add(mod);
                }
            }

            //Assign child modules to their parents
            for(int i = UI.UIModules.Count - 1; i >= 0; i--)
            {
                var attr = UI.UIModules[i].GetType().GetCustomAttribute<AutoLoadAttribute>();
                if(string.IsNullOrEmpty(attr?.ParentModuleID))
                    continue;

                if(AssignToParent(UI.UIModules[i]))
                    UI.UIModules.RemoveAt(i);
            }

            //Create and assign subtools that don't belong to any modules
            foreach(var type in subItemTypeCache)
            {
                var attr = type.GetCustomAttribute<AutoLoadAttribute>();
                if(string.IsNullOrEmpty(attr.ParentModuleID))
                {
                    var tool = Activator.CreateInstance(type) as ITool;
                    if(tool != null)
                        UI.OrphanHolder.SubItems.Add(tool);
                }
            }

            UI.OrderModules();
            return UI;
        }

        public static IUIModule BuildModule<T>() where T : IUIModule
        {
            return BuildModule(typeof(T));
        }

        public static IUIModule BuildModule(Type moduleType)
        {
            var items = new List<IItem>();
            var module = Activator.CreateInstance(moduleType) as IUIModule;

            var modAttr = module.GetType().GetCustomAttribute<AutoLoadAttribute>(false);
            if(string.IsNullOrEmpty(modAttr?.ID))
            {
                Debug.LogError($"Module '{moduleType.Name}' has no or empty ModuleID attribute.");
                return null;
            }

            //Loop through items whose ParentID matches our ID and assign them to our module
            var childItemTypes = subItemTypeCache.Where(t => t.GetCustomAttribute<AutoLoadAttribute>()?.ParentModuleID == modAttr.ID);
            foreach(var itemType in childItemTypes)
            {
                var itemInst = Activator.CreateInstance(itemType) as IItem;
                if(itemInst != null)
                    items.Add(itemInst);
            }

            //Order subtools based on their OrderInUI
            module.SubItems = items.OrderBy(x => x.OrderInUI).ToList();

            if(ModuleIDManager.RegisterModule(module))
                return module;
            return null;
        }

        public static bool AssignToParent(IUIModule module)
        {
            var attr = module.GetType().GetCustomAttribute<AutoLoadAttribute>();
            var parent = ModuleIDManager.GetModule(attr?.ParentModuleID);
            if(parent == null)
                return false;

            parent.ChildModules.Add(module);
            return true;
        }

        public static void RefreshCachedTypes(string configurationString)
        {
            typeCache = TypeHelpers.GetTypesWithAttribute<AutoLoadAttribute>()?.ToList();
            subItemTypeCache = TypeHelpers.GetChildTypesOf<IItem>(typeCache)?.ToList();
            moduleTypeCache = TypeHelpers.GetChildTypesOf<IUIModule>(typeCache)?.ToList();

            if(!string.IsNullOrEmpty(configurationString))
            {
                FilterList(ref typeCache);
                FilterList(ref subItemTypeCache);
                FilterList(ref moduleTypeCache);

                void FilterList(ref List<Type> list)
                {
                    list = list.Where((t) =>
                    {
                        var attr = t.GetCustomAttribute<AutoLoadAttribute>();
                        if(!attr)
                            return false;
                        return attr.ConfigurationStrings.Contains(configurationString, StringComparer.InvariantCultureIgnoreCase)
                            || attr.ConfigurationStrings.Contains(PumkinTools.DEFAULT_CONFIGURATION, StringComparer.InvariantCultureIgnoreCase);
                    }).ToList();
                }
            }
        }
    }
}
#endif