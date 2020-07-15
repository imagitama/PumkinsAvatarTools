using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Helpers;
using Pumkin.AvatarTools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Modules
{
    static class ModuleIDManager
    {
        static Dictionary<string, IUIModule> IDCache = new Dictionary<string, IUIModule>();

        public static bool RegisterModule(IUIModule module)
        {
            if(module == null)
                return false;

            string id = module.GetType().GetCustomAttribute<ModuleIDAttribute>()?.ID ?? null;            
            if(!string.IsNullOrEmpty(id) && IDCache.TryGetValue(id, out _))
            {
                Debug.Log($"Module with ID {id} has already been created");
                return false;
            }

            IDCache.Add(id, module);
            return true;
        }

        public static IUIModule GetModule(string moduleID)
        {
            IDCache.TryGetValue(moduleID, out IUIModule value);
            return value;
        }

        public static void ClearCache()
        {
            IDCache.Clear();
        }
    }
}
