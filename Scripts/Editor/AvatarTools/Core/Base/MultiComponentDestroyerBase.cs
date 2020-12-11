using Pumkin.AvatarTools2.Interfaces;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools2.Destroyers
{
    public abstract class MultiComponentDestroyerBase : ComponentDestroyerBase
    {
        public override string ComponentTypeFullName =>
            ComponentTypeFullNamesAll?.Length > 0 ? ComponentTypeFullNamesAll[0] : null;

        public abstract string[] ComponentTypeFullNamesAll { get; }

        protected Dictionary<Type, bool> ComponentTypesAndEnabled { get; set; } = new Dictionary<Type, bool>();

        public MultiComponentDestroyerBase()
        {
            ComponentTypesAndEnabled = new Dictionary<Type, bool>(ComponentTypeFullNamesAll.Length);

            for(int i = 0; i < ComponentTypeFullNamesAll.Length; i++)
            {
                string tName = ComponentTypeFullNamesAll[i];

                if(!string.IsNullOrWhiteSpace(tName))
                {
                    var type = TypeHelpers.GetType(tName);
                    if(type != null)
                        ComponentTypesAndEnabled[type] = true;
                }
                else
                    PumkinTools.LogVerbose($"{tName} is invalid");
            }

            if(!UIDefs)
            {
                string name = ComponentTypesAndEnabled.First().Key.Name;
                UIDefs = new UIDefinition(StringHelpers.ToTitleCase(name) ?? "Invalid Destroyer");
            }
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

        protected override void Finish(GameObject target)
        {
            var sb = new StringBuilder();
            foreach(var kv in ComponentTypesAndEnabled)
                if(kv.Value)
                    sb.Append($"{kv.Key.Name}s, ");

            if(sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            string names = sb.ToString();
            if(string.IsNullOrWhiteSpace(names))
                names = UIDefs.Name;

            PumkinTools.Log($"Successfully removed all <b>{names}</b> from <b>{target.name}</b>");
        }

        protected override bool DoDestroyComponents(GameObject target)
        {
            foreach(var comp in ComponentTypesAndEnabled)
                if(comp.Value)
                    DoDestroyByType(target, comp.Key);
            return true;
        }
    }
}
