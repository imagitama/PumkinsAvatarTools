#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pumkin.AvatarTools2.Interfaces;
using UnityEngine;
using Pumkin.Core.Helpers;
using Pumkin.AvatarTools2.Modules;
using Pumkin.Core;

namespace Pumkin.AvatarTools2.UI
{
    static class UIBuilder
    {
        public static event Action BeforeUIBuildCallback;
        public static event Action OnUIBuildFinished;
        public static bool IsBuilding { get; private set; }

        static Dictionary<Type, AutoLoadAttribute> typeCache;
        static Dictionary<Type, AutoLoadAttribute> subItemTypeCache;
        static Dictionary<Type, AutoLoadAttribute> moduleTypeCache;

        public static bool BuildUI(out MainUI ui)
        {
            PumkinTools.LogVerbose("Building UI...");
            ui = new MainUI();

            if(IsBuilding)
                return false;

            BeforeUIBuildCallback?.Invoke();

            IsBuilding = true;

            var builders = new List<ModuleBuilder>();
            var modules = new List<IUIModule>();

            try
            {
                IDManager.ClearCache();
                RefreshCachedTypes(ConfigurationManager.CurrentConfigurationString);

                //Create all builders and their modules
                foreach(var kv in moduleTypeCache)
                {
                    var builder = new ModuleBuilder(kv.Key, kv.Value);
                    if(builder.BuildModule())
                        builders.Add(builder);
                }
            }
            catch(Exception e)
            {
                PumkinTools.LogException(e);
            }


            //Assign modules to their parents
            try
            {
                foreach(var kv in IDManager.Modules)
                {
                    string id = moduleTypeCache[kv.Value.GetType()]?.ParentModuleID;
                    if(kv.Key == id)
                        PumkinTools.LogError($"Can't make module {id} a parent of itself.");
                    else
                        AssignToParentByID(kv.Value, id);
                }
            }
            catch(Exception e)
            {
                PumkinTools.LogException(e);
            }


            //Build SubTools for all modules and add to main ui modules, but only if it's a root module
            try
            {
                foreach(var builder in builders)
                {
                    builder.BuildSubItems(subItemTypeCache);
                    if(string.IsNullOrWhiteSpace(builder.LoadAttribute.ParentModuleID))
                        modules.Add(builder.Module);
                }
            }
            catch(Exception e)
            {
                PumkinTools.LogException(e);
            }


            //Create and assign SubTools that don't belong to any modules
            try
            {
                foreach(var kv in subItemTypeCache)
                {
                    if(string.IsNullOrEmpty(kv.Value.ParentModuleID))
                    {
                        if(Activator.CreateInstance(kv.Key) is ITool tool)
                            ui.OrphanHolder.SubItems.Add(tool);
                    }
                }
            }
            catch(Exception e)
            {
                PumkinTools.LogException(e);
            }

            ui.UIModules = modules;
            ui.OrderModules();

            IsBuilding = false;
            OnUIBuildFinished?.Invoke();
            return true;
        }

        public static bool AssignToParentByID(IUIModule module, string parentID)
        {
            var parent = IDManager.GetModule(parentID);
            if(parent == null)
                return false;

            parent.ChildModules.Add(module);
            return true;
        }

        public static bool AssignToParent(IUIModule module)
        {
            var attr = moduleTypeCache[module.GetType()];
            return AssignToParentByID(module, attr?.ParentModuleID);
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