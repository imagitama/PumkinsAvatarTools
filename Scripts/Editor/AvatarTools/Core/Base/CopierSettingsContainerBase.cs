using Pumkin.Core;
using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Settings
{
    [Serializable]
    public abstract class CopierSettingsContainerBase : SettingsContainerBase
    {
        protected virtual bool ShowRemoveAll { get; } = true;
        protected virtual bool ShowCreateGameObjects { get; } = true;

        [ConditionalHide(nameof(ShowRemoveAll))]
        public bool removeAllBeforeCopying = false;

        [ConditionalHide(nameof(ShowCreateGameObjects))]
        public bool createGameObjects = false;
        public abstract PropertyDefinitions Properties { get; }

        [SerializeField]UISpacer _spacer;
    }

    public class PropertyDefinitions
    {
        public Type Type { get; private set; }
        public List<PropertyGroup> Properties { get; private set; }

        public PropertyDefinitions(Type type, params PropertyGroup[] properties)
        {
            Type = type;
            Properties = properties.ToList();
        }

        public PropertyDefinitions(params PropertyGroup[] properties) : this(default(Type), properties) { }

        public PropertyDefinitions(string fullTypeName, params PropertyGroup[] properties)
            : this(TypeHelpers.GetTypeAnywhere(fullTypeName), properties) { }

        public PropertyDefinitions(Type type)
            : this(type, new PropertyGroup(null, "__all__")) { }
    }

    public class PropertyGroup
    {
        public string Name { get; set; }
        public bool Enabled { get; set; } = true;
        public string[] PropertyNames { get; set; }


        public PropertyGroup(string name, bool enabled, params string[] propertyNames)
        {
            Name = name;
            Enabled = enabled;
            PropertyNames = propertyNames;
        }

        public PropertyGroup(string name, params string[] propertyNames)
            : this(name, true, propertyNames) { }
    }
}