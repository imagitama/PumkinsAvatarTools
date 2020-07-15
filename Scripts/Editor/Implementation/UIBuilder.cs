using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Pumkin.AvatarTools;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.Attributes;
using UnityEngine;
using Pumkin.AvatarTools.Helpers;
using Pumkin.AvatarTools.Implementation.Modules;

namespace Pumkin.AvatarTools.UI
{
    static class UIBuilder
    {
        static IEnumerable<Type> typeCache;

        public static MainUI BuildUI()
        {
            ModuleIDManager.ClearCache();
            List<IUIModule> modules = new List<IUIModule>();

            //Get all types implementing IUIModule that have the AllowAutoLoad attribute
            typeCache = TypeHelpers.GetTypesWithAttribute<AutoLoadAttribute>();            
            
            var moduleTypes = TypeHelpers.GetChildTypesOf<IUIModule>()
                ?.Where(t => t.IsDefined(typeof(AutoLoadAttribute))) ?? null;

            //Instantiate modules from typeCache
            foreach(var t in moduleTypes)
            {
                var mod = BuildModule(t);
                if(mod != null)
                    modules.Add(mod);
            }

            //Assign child modules to their parents
            for(int i = modules.Count - 1; i >= 0; i--)
            {
                if(string.IsNullOrEmpty(modules[i].ParentModuleID))
                    continue;

                if(AssignToParent(modules[i]))
                    modules.RemoveAt(i);
            }            

            return new MainUI(modules.OrderBy(c => c.OrderInUI).ToList());
        }        

        public static IUIModule BuildModule<T>() where T : IUIModule
        {
            return BuildModule(typeof(T));
        }
        
        public static IUIModule BuildModule(Type t)
        {
            var tools = new List<ISubTool>();            
            
            var types = TypeHelpers.GetChildTypesOf<ISubTool>(typeCache);
            var module = Activator.CreateInstance(t) as IUIModule;

            var attr = module.GetType().GetCustomAttribute<ModuleIDAttribute>(false);
            if(attr == null || string.IsNullOrEmpty(attr.ID))
            {
                Debug.Log($"Module {t.Name} has no or empty ModuleID attribute.");
                return null;
            }            

            foreach(var tool in types)
            {
                var toolInst = Activator.CreateInstance(tool) as ISubTool;
                if(toolInst.ParentModuleID.Equals(attr.ID, StringComparison.InvariantCultureIgnoreCase))
                    tools.Add(toolInst);
            }

            module.SubTools = tools.OrderBy(x => x.OrderInUI).ToList();

            if(ModuleIDManager.RegisterModule(module))
                return module;
            return null;
        }

        public static bool AssignToParent(IUIModule module)
        {
            if(string.IsNullOrEmpty(module.ParentModuleID))
                return false;

            var parent = ModuleIDManager.GetModule(module.ParentModuleID);
            if(parent == null)
                return false;

            parent.ChildModules.Add(module);
            return true;
        }        
    }
}
