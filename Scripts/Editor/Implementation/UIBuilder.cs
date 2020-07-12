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

namespace Pumkin.AvatarTools.UI
{
    static class UIBuilder
    {
        static IEnumerable<Type> typeCache;

        public static MainUI BuildUI()
        {
            List<IUIModule> modules = new List<IUIModule>();

            //Get all types implementing IUIModule that have the AllowAutoLoad attribute
            typeCache = TypeHelpers.GetTypesWithAttribute<AllowAutoLoadAttribute>();
            
            var moduleTypes = TypeHelpers.GetChildTypesOf<IUIModule>()
                ?.Where(t => t.IsDefined(typeof(AllowAutoLoadAttribute))) ?? null;

            foreach(var t in moduleTypes)
            {
                var mod = BuildModule(t);
                if(mod != null && mod.SubTools.Count != 0)
                    modules.Add(mod);
            }

            return new MainUI(modules);
        }

        public static IUIModule BuildModule<T>() where T : IUIModule
        {
            return BuildModule(typeof(T));
        }
        
        public static IUIModule BuildModule(Type t)
        {
            var tools = new List<ISubTool>();            
            
            var types = TypeHelpers.GetChildTypesOf<ISubTool>(typeCache);
            foreach(var tool in types)
            {
                var inst = Activator.CreateInstance(tool) as ISubTool;
                tools.Add(inst);
            }

            return Activator.CreateInstance(t, tools) as IUIModule;
        }
    }
}
