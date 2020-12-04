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
using Pumkin.Core.Helpers;

namespace Pumkin.AvatarTools.UI
{
    class ModuleBuilder
    {
        public IUIModule Module { get; private set; }
        public Type ModuleType { get; private set; }
        public AutoLoadAttribute LoadAttribute { get; private set; }

        Dictionary<Type, AutoLoadAttribute> subToolTypes;

        ModuleBuilder() { }

        public ModuleBuilder(Type moduleType, AutoLoadAttribute loadAttribute)
        {
            this.ModuleType = moduleType;
            this.LoadAttribute = loadAttribute;
        }

        public void BuildSubTools(Dictionary<Type, AutoLoadAttribute> subItemsTypes)
        {
            var items = new List<IItem>();

            //Filter child types to ones that have this module as it's parent ID
            subToolTypes = subItemsTypes
                .Where(kv => kv.Value?.ParentModuleID == LoadAttribute.ID)
                .ToDictionary(x => x.Key, x => x.Value);

            Type iActor = typeof(IComponentActor);
            //Loop through and assign children to our module
            foreach(var itemType in subToolTypes.Keys)
            {
                try
                {
                    var itemInst = Activator.CreateInstance(itemType) as IItem;
                    //Check if item is a copier or destroyer then check if their targetted type is valid in project
                    if(itemInst is IComponentActor actor)
                    {
                        Type targetType = TypeHelpers.GetType(actor.ComponentTypeFullName);
                        if(targetType == null)
                        {
                            PumkinTools.LogVerbose($"Type {actor.ComponentTypeFullName} now found in project for {itemInst.GetType()}");
                            continue;
                        }
                    }

                    if(IDManager.Register(itemInst))
                            items.Add(itemInst);
                }
                catch(Exception e)
                {
                    PumkinTools.LogException(e);
                }
            }

            //Order SubTools based on their OrderInUI
            Module.SubItems = items.OrderBy(x => x?.UIDefs?.OrderInUI)?.ToList() ?? Module.SubItems;
        }

        public bool BuildModule()
        {
            //Cancel checks
            {
                if(string.IsNullOrEmpty(LoadAttribute?.ID))
                {
                    PumkinTools.LogWarning($"Module '{ModuleType.Name}' has no or empty ModuleID attribute.");
                    return false;
                }

                var oldMod = IDManager.GetModule(LoadAttribute?.ID);
                if(oldMod != null)
                {
                    PumkinTools.LogWarning(
                        $"Can't create module type '{ModuleType.Name}' with ID '{LoadAttribute.ID}' as '{oldMod.UIDefs.Name}' already registered that ID");
                    return false;
                }
            }

            Module = Activator.CreateInstance(ModuleType) as IUIModule;

            if(Module == null)
            {
                PumkinTools.LogWarning($"Can't create module of type {ModuleType.Name}");
                return false;
            }

            if(IDManager.Register(Module))
                return true;
            return false;
        }
    }
}
