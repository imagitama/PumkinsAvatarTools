#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pumkin.AvatarTools.Interfaces;
using UnityEngine;
using Pumkin.Core.Helpers;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core;

namespace Pumkin.AvatarTools.UI
{
    static class UIBuilder
    {
        static Dictionary<Type, AutoLoadAttribute> typeCache;
        static Dictionary<Type, AutoLoadAttribute> subItemTypeCache;
        static Dictionary<Type, AutoLoadAttribute> moduleTypeCache;

        public static MainUI BuildUI()
        {
            MainUI ui = new MainUI();
            IDManager.ClearCache();
            RefreshCachedTypes(ConfigurationManager.CurrentConfigurationString);

            try
            {
                var modules = new List<IUIModule>();

                foreach(var kv in moduleTypeCache)
                {
                    var builder = new ModuleBuilder(kv.Key, kv.Value);
                    var mod = builder.BuildModule(subItemTypeCache);
                    if(mod != null)
                        modules.Add(mod);
                }

                //Assign child modules to their parents
                for(int i = modules.Count - 1; i >= 0; i--)
                {
                    var attr = modules[i].GetType().GetCustomAttribute<AutoLoadAttribute>();
                    if(string.IsNullOrEmpty(attr?.ParentModuleID))
                        continue;

                    if(AssignToParentByID(modules[i]))
                        modules.RemoveAt(i);
                }

                //Create and assign subtools that don't belong to any modules
                foreach(var kv in subItemTypeCache)
                {
                    if(string.IsNullOrEmpty(kv.Value.ParentModuleID))
                    {
                        if(Activator.CreateInstance(kv.Key) is ITool tool)
                            ui.OrphanHolder.SubItems.Add(tool);
                    }
                }

                ui.UIModules = modules;
                ui.OrderModules();
            }
            catch(Exception e)
            {
                PumkinTools.LogException(e);
            }
            return ui;
        }

        public static bool AssignToParentByID(IUIModule module)
        {
            var attr = moduleTypeCache[module.GetType()];
            var parent = IDManager.GetModule(attr?.ParentModuleID);
            if(parent == null)
                return false;

            parent.ChildModules.Add(module);
            return true;
        }

        public static void RefreshCachedTypes(string configurationString)
        {
            typeCache = TypeHelpers.GetTypesAndAttributesWithAttribute<AutoLoadAttribute>();
            subItemTypeCache = TypeHelpers.GetChildTypesOfWithAttribute<IItem, AutoLoadAttribute>(typeCache.Keys);
            moduleTypeCache = TypeHelpers.GetChildTypesOfWithAttribute<IUIModule, AutoLoadAttribute>(typeCache.Keys);

            if(!string.IsNullOrEmpty(configurationString))
            {
                FilterTypeDictionary(ref typeCache);
                FilterTypeDictionary(ref subItemTypeCache);
                FilterTypeDictionary(ref moduleTypeCache);

                void FilterTypeDictionary(ref Dictionary<Type, AutoLoadAttribute> dict)
                {
                    //Remove all types that aren't in the current or default configuration then select non default ones if available
                    dict = dict.Where((kv) =>
                    {
                        return kv.Value.ConfigurationStrings.Contains(configurationString,
                                   StringComparer.InvariantCultureIgnoreCase)
                               || kv.Value.ConfigurationStrings.Contains(ConfigurationManager.DEFAULT_CONFIGURATION,
                                   StringComparer.InvariantCultureIgnoreCase);
                    })
                        .OrderBy(kv => kv.Value.IsGenericItem)
                        .GroupBy(kv => kv.Value.ID)
                        .Select(g => g.First())
                        .ToDictionary(k => k.Key, v => v.Value);
                }
            }
        }
    }
}
#endif