using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Pumkin.UnityTools;
using Pumkin.UnityTools.Interfaces;
using Pumkin.UnityTools.Attributes;
using UnityEngine;
using Pumkin.UnityTools.Helpers;
using Pumkin.UnityTools.Implementation.Modules;

namespace Pumkin.UnityTools.UI
{
    static class UIBuilder
    {
        static List<Type> typeCache;
        static List<Type> subToolTypeCache;
        static List<Type> moduleTypeCache;
        
        public static MainUI BuildUI()
        {
            MainUI UI = new MainUI();
            ModuleIDManager.ClearCache();

            RefreshCachedTypes();
            
            //Build modules from typeCache
            foreach(var t in moduleTypeCache)
            {
                var mod = BuildModule(t);
                if(mod != null)
                    UI.UIModules.Add(mod);
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
            foreach(var type in subToolTypeCache)
            {
                var attr = type.GetCustomAttribute<AutoLoadAttribute>();
                if(string.IsNullOrEmpty(attr.ParentModuleID))
                {
                    var tool = Activator.CreateInstance(type) as ISubTool;
                    if(tool != null)
                        UI.OrphanHolder.SubTools.Add(tool);
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
            var tools = new List<ISubTool>();            
            var module = Activator.CreateInstance(moduleType) as IUIModule;

            var modAttr = module.GetType().GetCustomAttribute<AutoLoadAttribute>(false);
            if(string.IsNullOrEmpty(modAttr?.ID))
            {
                Debug.LogError($"Module '{moduleType.Name}' has no or empty ModuleID attribute.");
                return null;
            }

            //Loop through tools whose ParentID matches our ID and assign them to our module
            var childToolTypes = subToolTypeCache.Where(t => t.GetCustomAttribute<AutoLoadAttribute>()?.ParentModuleID == modAttr.ID);
            foreach(var toolType in childToolTypes)
            {                
                var toolInst = Activator.CreateInstance(toolType) as ISubTool;
                if(toolInst != null)
                    tools.Add(toolInst);                   
            }

            module.SubTools = tools.OrderBy(x => x.OrderInUI).ToList();

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

        public static void RefreshCachedTypes()
        {
            typeCache = TypeHelpers.GetTypesWithAttribute<AutoLoadAttribute>()?.ToList();
            subToolTypeCache = TypeHelpers.GetChildTypesOf<ISubTool>(typeCache)?.ToList();
            moduleTypeCache = TypeHelpers.GetChildTypesOf<IUIModule>(typeCache)?.ToList();
        }
    }
}
