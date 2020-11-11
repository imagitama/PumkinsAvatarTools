using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core;

namespace Pumkin.AvatarTools.UI
{
    class ModuleBuilder
    {
        IUIModule module;
        Type moduleType;
        AutoLoadAttribute loadAttribute;

        ModuleBuilder() { }

        public ModuleBuilder(Type moduleType, AutoLoadAttribute loadAttribute)
        {
            this.moduleType = moduleType;
            this.loadAttribute = loadAttribute;
        }

        public IUIModule BuildModule(Dictionary<Type, AutoLoadAttribute> subItemsTypes)
        {
            //Cancel checks
            {
                if(string.IsNullOrEmpty(loadAttribute?.ID))
                {
                    PumkinTools.LogWarning($"Module '{moduleType.Name}' has no or empty ModuleID attribute.");
                    return null;
                }

                var oldMod = IDManager.GetModule(loadAttribute?.ID);
                if(oldMod != null)
                {
                    PumkinTools.LogWarning(
                        $"Can't create module type '{moduleType.Name}' with ID '{loadAttribute.ID}' as '{oldMod.Name}' already registered that ID");
                    return null;
                }
            }

            var items = new List<IItem>();
            module = Activator.CreateInstance(moduleType) as IUIModule;

            if(module == null)
            {
                PumkinTools.LogWarning($"Can't create module of type {moduleType.Name}");
                return null;
            }

            //Filter child types to ones that have this module as it's parent ID
            var childItemTypes = subItemsTypes
                .Where(kv => kv.Value?.ParentModuleID == loadAttribute.ID)
                .ToDictionary(x => x.Key, x => x.Value);

            //Loop through and assign children to our module
            foreach(var itemType in childItemTypes.Keys)
            {
                if(Activator.CreateInstance(itemType) is IItem itemInst)
                    if(IDManager.Register(itemInst))
                        items.Add(itemInst);
            }

            //Order subtools based on their OrderInUI
            module.SubItems = items.OrderBy(x => x.OrderInUI).ToList();

            if(IDManager.Register(module))
                return module;
            return null;
        }
    }
}
