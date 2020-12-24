using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.Modules;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using UnityEditor;

namespace Pumkin.AvatarTools2.UI
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

        public void BuildSubItems(Dictionary<Type, AutoLoadAttribute> subItemsTypes)
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
                        Type targetType = actor.FirstValidType;
                        if(targetType == null)
                        {
                            PumkinTools.LogWarning($"No valid types found in actor <b>{actor.GetType().Name}</b>");
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
            Module.SubItems = items.OrderBy(x => x?.UIDefs?.OrderInUI)?.ToList() ?? Module.SubItems;//
        }

        public bool BuildModule()
        {
            PumkinTools.LogVerbose($"Building module <b>{ModuleType?.Name ?? "Invalid type"}</b>");
            //Cancel checks
            {
                if(string.IsNullOrEmpty(LoadAttribute?.ID))
                {
                    PumkinTools.LogWarning($"Module <b>{ModuleType.Name}</b> has no or empty ModuleID attribute.");
                    return false;
                }

                var oldMod = IDManager.GetModule(LoadAttribute?.ID);
                if(oldMod != null)
                {
                    PumkinTools.LogWarning(
                        $"Can't create module type <b>{ModuleType.Name}</b> with ID <b>{LoadAttribute.ID}</b> as <b>{oldMod.UIDefs.Name}</b> already registered that ID");
                    return false;
                }

                //TODO: Figure this out later, when I have saving and loading the window state
                //Cancel if our auto load mode doesn't match current mode
                //if(LoadAttribute.AutoLoadMode != AutoLoadAttribute.AutoLoadInMode.Both)
                //{
                //    if((EditorApplication.isPlayingOrWillChangePlaymode && LoadAttribute.AutoLoadMode != AutoLoadAttribute.AutoLoadInMode.Play) ||
                //        (!EditorApplication.isPlayingOrWillChangePlaymode && LoadAttribute.AutoLoadMode != AutoLoadAttribute.AutoLoadInMode.Editor))
                //    {
                //        string modeName = Enum.GetName(typeof(AutoLoadAttribute.AutoLoadInMode), LoadAttribute.AutoLoadMode);
                //        string currentMode = EditorApplication.isPlaying ? "Play" : "Editor";

                //        PumkinTools.LogVerbose($"Module <b>{ModuleType.Name}</b> is trying to load in {currentMode} mode but it's AutoLoad attribute only allows {modeName} mode.");
                //        return false;
                //    }
                //}
            }


            Module = Activator.CreateInstance(ModuleType) as IUIModule;

            if(Module == null)
            {
                PumkinTools.LogWarning($"Can't create module of type <b>{ModuleType.Name}</b>");
                return false;
            }

            if(IDManager.Register(Module))
                return true;
            return false;
        }
    }
}
