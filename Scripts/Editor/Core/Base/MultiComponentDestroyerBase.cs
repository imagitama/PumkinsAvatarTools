using Pumkin.AvatarTools.Interfaces;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Base
{
    public abstract class MultiComponentDestroyerBase : ComponentDestroyerBase
    {
        public override string ComponentTypeFullName =>
            ComponentTypeFullNamesAll?.Length > 0 ? ComponentTypeFullNamesAll[0] : null;

        public abstract string[] ComponentTypeFullNamesAll { get; set; }

        Type[] _componentTypes;

        public MultiComponentDestroyerBase()
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

        protected virtual bool DoDestroyByType(GameObject target, Type componentType)
        {
            foreach(var co in target.GetComponentsInChildren(componentType, true))
            {
                if(ShouldIgnoreObject(co.gameObject))
                    continue;

                try
                {
                    UnityObjectHelpers.DestroyAppropriate(co);
                }
                catch(Exception e)
                {
                    PumkinTools.LogWarning(e.Message);
                }
            }
            return true;
        }

        public override void Finish(GameObject target)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < ComponentTypeFullNamesAll.Length; i++)
            {
                sb.Append($"{_componentTypes[i].Name}");
                if(i != _componentTypes.Length - 1)
                    sb.Append(", ");
            }

            PumkinTools.Log($"Successfully removed all <b>{sb.ToString()}</b> from <b>{target.name}</b>");
        }

        protected override bool DoDestroyComponents(GameObject target)
        {
            foreach(var comp in _componentTypes)
                DoDestroyByType(target, comp);
            return true;
        }
    }
}
