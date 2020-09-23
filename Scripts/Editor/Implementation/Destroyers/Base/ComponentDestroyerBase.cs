using Pumkin.AvatarTools.Helpers;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.Interfaces.ComponentDestroyer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Destroyers
{
    abstract class ComponentDestroyerBase : IComponentDestroyer, ISubItem
    {
        public abstract string ComponentTypeNameFull { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GameConfigurationString { get; set; }
        public int OrderInUI { get; set; }

        Type type;

        public ComponentDestroyerBase()
        {
            if(!string.IsNullOrWhiteSpace(ComponentTypeNameFull))
                type = TypeHelpers.GetType(ComponentTypeNameFull);
            else
                throw new ArgumentNullException(ComponentTypeNameFull, $"{ComponentTypeNameFull} is invalid");

            Name = type.Name;
        }

        public void Finish(GameObject target)
        {
            PumkinTools.Log($"Successfully removed all {type?.Name ?? "Unknown Type"} from {target.name}");
        }

        public bool Prepare(GameObject target)
        {
            if(type == null)
            {
                PumkinTools.LogWarning($"Can't use {GetType().Name} as it doesn't have a valid type.");
                return false;
            }

            if(!target)
                return false;

            Undo.RegisterCompleteObjectUndo(target, $"Destroy Components {type.Name}");
            return true;
        }

        public bool TryDestroyComponents(GameObject target)
        {
            try
            {
                if(Prepare(target) && DoDestroyComponents(target))
                {
                    Finish(target);
                    return true;
                }
            }
            catch(Exception e) 
            {
                Debug.LogException(e);
            }
            return false;
        }

        protected bool DoDestroyComponents(GameObject target)
        {
            foreach(var co in target.GetComponentsInChildren(type, true))
            {
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

        public void DrawUI()
        {
            if(GUILayout.Button(Name))            
                TryDestroyComponents(PumkinTools.SelectedAvatar);
        }
    }
}
