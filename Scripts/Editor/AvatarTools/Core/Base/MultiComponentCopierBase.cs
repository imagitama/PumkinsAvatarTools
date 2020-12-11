using Pumkin.AvatarTools2.Settings;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Pumkin.AvatarTools2.Copiers
{
    public abstract class MultiComponentCopierBase : ComponentCopierBase
    {
        public override string ComponentTypeFullName =>
            ComponentTypeFullNamesAll?.Length > 0 ? ComponentTypeFullNamesAll[0] : null;

        public abstract string[] ComponentTypeFullNamesAll { get; set; }

        Type[] _componentTypes;

        Dictionary<Type, CopierSettingsContainerBase> _dict;

        public MultiComponentCopierBase()
        {
            _componentTypes = new Type[ComponentTypeFullNamesAll.Length];

            for(int i = 0; i < ComponentTypeFullNamesAll.Length; i++)
            {
                string tName = ComponentTypeFullNamesAll[i];

                if(!string.IsNullOrWhiteSpace(tName))
                    _componentTypes[i] = TypeHelpers.GetType(tName);
                else
                    PumkinTools.LogVerbose($"{tName} is invalid");
            }

            if(!UIDefs)
                UIDefs = new UIDefinition(_componentTypes[0]?.Name ?? "Invalid Destroyer");
        }

        protected virtual bool DoCopyByType(GameObject objFrom, GameObject objTo, Type componentType, CopierSettingsContainerBase settings = null)
        {
            var set = Settings as CopierSettingsContainerBase;
            bool createGameObjects = set != null && set.createGameObjects;
            string[] propNames = set.PropertyNames;

            //foreach(var co in objFrom.GetComponentsInChildren(componentType, true))
            //{
            //    if(ShouldIgnoreObject(co.gameObject))
            //        continue;

            //    foreach(var coFrom in compsFrom)
            //    {
            //        if(!coFrom || ShouldIgnoreObject(coFrom.gameObject))
            //        {
            //            PumkinTools.LogVerbose($"<b>{UIDefs.Name}</b> copier: Ignoring {coFrom.gameObject.name}");
            //            continue;
            //        }

            //        var transPath = coFrom.transform.GetPathInHierarchy();
            //        var trans = objTo.transform.FindOrCreate(transPath, createGameObjects, objFrom.transform);
            //        if(!trans)
            //            continue;


            //        Component addedComp;
            //        if(propNames.IsNullOrEmpty())
            //            addedComp = CopyEverything(coFrom, trans);
            //        else
            //            addedComp = CopyProperties(coFrom, trans, propNames);

            //        FixReferences(addedComp, objTo.transform);

            //    }
            return true;
        }

        protected override bool DoCopyComponents(GameObject objFrom, GameObject objTo)
        {
            foreach(var comp in _componentTypes)
                DoCopyByType(objFrom, objTo, comp);
            return true;
        }

        protected override void Finish(GameObject objFrom, GameObject objTo)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < ComponentTypeFullNamesAll.Length; i++)
            {
                sb.Append($"{_componentTypes[i].Name}");
                if(i != _componentTypes.Length - 1)
                    sb.Append(", ");
            }

            PumkinTools.Log($"Successfully removed all <b>{sb.ToString()}</b> from <b>{objFrom.name}</b>");
        }

    }
}
