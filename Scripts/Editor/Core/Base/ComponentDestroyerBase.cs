using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using Pumkin.Core.Helpers;
using System;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Base   //TODO: Check if component type exists before trying to instantiate this
{
    public abstract class ComponentDestroyerBase : IComponentDestroyer, IItem
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

        public virtual ISettingsContainer Settings => null;

        public bool EnabledInUI { get; set; } = true;

        Type _componentType;
        GUIContent _content;

        public ComponentDestroyerBase()
        {
            if(!string.IsNullOrWhiteSpace(ComponentTypeNameFull))
                _componentType = TypeHelpers.GetType(ComponentTypeNameFull);
            else
                throw new ArgumentNullException(ComponentTypeNameFull, $"{ComponentTypeNameFull} is invalid");

            Name = _componentType?.Name ?? "Invalid Destroyer";
        }

        public void Finish(GameObject target)
        {
            PumkinTools.Log($"Successfully removed all <b>{_componentType?.Name ?? "Unknown Type"}</b> from <b>{target.name}</b>");
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

        public void DrawUI(params GUILayoutOption[] options)
        {
            if(GUILayout.Button(Content, Styles.TextIconButton, options))
                TryDestroyComponents(PumkinTools.SelectedAvatar);
        }
    }
}
