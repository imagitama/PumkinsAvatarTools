#if UNITY_EDITOR
using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Helpers;
using Pumkin.UnityTools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Modules
{
    static class ModuleIDManager
    {
        static Dictionary<string, IUIModule> IDCache = new Dictionary<string, IUIModule>();
        
        public static bool RegisterModule(IUIModule module)
        {
            if(module == null)
                return false;
            var typ = module.GetType();
            var attr = typ.GetCustomAttribute<AutoLoadAttribute>();
            
            //var attr = module.GetType().GetCustomAttribute<AutoLoadAttribute>() ?? null;
            if(attr != null && IDCache.TryGetValue(attr.ID.ToUpperInvariant(), out _))
            {
                Debug.LogWarning($"Module with ID '{attr.ID}' has already been created");
                return false;
            }

            IDCache.Add(attr.ID.ToUpper(), module);
            return true;
        }

        public static IUIModule GetModule(string moduleID)
        {
            if(moduleID == null)
                moduleID = "";
            IDCache.TryGetValue(moduleID.ToUpperInvariant(), out IUIModule value);
            return value;
        }

        public static void ClearCache()
        {
            IDCache.Clear();
        }
    }
}
#endif