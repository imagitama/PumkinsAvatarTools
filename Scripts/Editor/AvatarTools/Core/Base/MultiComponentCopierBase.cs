using Pumkin.AvatarTools2.Settings;
using Pumkin.Core.Extensions;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pumkin.AvatarTools2.Copiers
{
    /// <summary>
    /// Base copier class that copies all components of multiple types by their full name. Also fixes references.
    /// </summary>
    public abstract class MultiComponentCopierBase : ComponentCopierBase
    {
        public override string ComponentTypeFullName =>
            ComponentTypeFullNamesAll?.Length > 0 ? ComponentTypeFullNamesAll[0] : null;

        public abstract string[] ComponentTypeFullNamesAll { get; set; }

        protected Type[] componentTypes;

        Dictionary<Type, CopierSettingsContainerBase> _dict;

        public MultiComponentCopierBase()
        {
            //Get types of components and filter out nulls
            componentTypes = ComponentTypeFullNamesAll
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => TypeHelpers.GetType(c))
                 .ToArray();

            for(int i = 0; i < ComponentTypeFullNamesAll.Length; i++)
            {
                string tName = ComponentTypeFullNamesAll[i];

                if(!string.IsNullOrWhiteSpace(tName))
                    componentTypes[i] = TypeHelpers.GetType(tName);
                else
                    PumkinTools.LogVerbose($"{tName} is invalid");
            }

            if(!UIDefs)
                UIDefs = new UIDefinition(componentTypes[0]?.Name ?? "Invalid Destroyer");
        }

        protected virtual bool DoCopyByType(GameObject objFrom, GameObject objTo, Type componentType, CopierSettingsContainerBase settings = null)
        {
            //TODO: Finish this
            var set = Settings as CopierSettingsContainerBase;
            bool createGameObjects = set != null && set.createGameObjects;
            string[] propNames = set.PropertyNames;

            foreach(var comp in objFrom.GetComponentsInChildren(componentType, true))
            {
                if(ShouldIgnoreObject(comp.gameObject))
                    continue;

                if(!comp || ShouldIgnoreObject(comp.gameObject))
                {
                    PumkinTools.LogVerbose($"<b>{UIDefs.Name}</b> copier: Ignoring {comp.gameObject.name}");
                    continue;
                }

                var transPath = comp.transform.GetPathInHierarchy();
                var trans = objTo.transform.FindOrCreate(transPath, createGameObjects, objFrom.transform);
                if(!trans)
                    continue;


                Component addedComp;
                if(propNames.IsNullOrEmpty())
                    addedComp = CopyEverything(comp, trans);
                else
                    addedComp = CopyProperties(comp, trans, propNames);

                FixReferences(addedComp, objTo.transform, set.createGameObjects);
            }
            return true;
        }

        protected override bool DoCopyComponents(GameObject objFrom, GameObject objTo)
        {
            foreach(var comp in componentTypes)
                DoCopyByType(objFrom, objTo, comp);
            return true;
        }

        protected override void Finish(GameObject objFrom, GameObject objTo)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < componentTypes.Length; i++)
            {
                sb.Append($"{componentTypes[i].Name}");
                if(i != componentTypes.Length - 1)
                    sb.Append(", ");
            }

            PumkinTools.Log($"Successfully copied all <b>{sb.ToString()}</b> from <b>{objFrom.name}</b>");
        }

    }
}
