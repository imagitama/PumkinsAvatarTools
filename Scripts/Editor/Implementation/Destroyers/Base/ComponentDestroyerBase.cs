using Pumkin.AvatarTools.Helpers;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
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
        public virtual GUIContent Content
        {
            get
            {
                if(_content == null)
                    _content = CreateGUIContent();
                return _content;
            }
        }
        public Type ComponentType 
        {
            get
            {
                if(_componentType == null)
                    _componentType = TypeHelpers.GetType(ComponentTypeNameFull);
                return _componentType;
            }
        }

        Type _componentType;
        GUIContent _content;        

        public ComponentDestroyerBase()
        {
            if(!string.IsNullOrWhiteSpace(ComponentTypeNameFull))
                _componentType = TypeHelpers.GetType(ComponentTypeNameFull);
            else
                throw new ArgumentNullException(ComponentTypeNameFull, $"{ComponentTypeNameFull} is invalid");

            Name = _componentType.Name;
        }

        public void Finish(GameObject target)
        {
            PumkinTools.Log($"Successfully removed all {_componentType?.Name ?? "Unknown Type"} from {target.name}");
        }

        public bool Prepare(GameObject target)
        {
            if(_componentType == null)
            {
                PumkinTools.LogWarning($"Can't use {GetType().Name} as it doesn't have a valid type.");
                return false;
            }

            if(!target)
                return false;

            Undo.RegisterCompleteObjectUndo(target, $"Destroy Components {_componentType.Name}");
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
            foreach(var co in target.GetComponentsInChildren(_componentType, true))
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

        protected virtual GUIContent CreateGUIContent()
        {
            return new GUIContent(Name, Icons.GetIconTextureFromType(ComponentType));
        }

        public void DrawUI()
        {
            if(GUILayout.Button(Content, Styles.TextIconButton))            
                TryDestroyComponents(PumkinTools.SelectedAvatar);
        }
    }
}
