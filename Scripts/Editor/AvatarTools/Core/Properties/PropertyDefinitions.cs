using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pumkin.AvatarTools2.Settings
{
    public class PropertyDefinitions
    {
        const string ALL_PROPERTIES = "__all__";

        public Dictionary<Type, PropertyGroup[]> TypeProperties { get; private set; }

        public PropertyDefinitions(Dictionary<Type, PropertyGroup[]> typeProperties)
        {
            TypeProperties = typeProperties;
        }

        public PropertyDefinitions(Type type, params PropertyGroup[] propertyGroups)
        {
            TypeProperties = new Dictionary<Type, PropertyGroup[]>()
            {
                { type, propertyGroups }
            };
        }

        public PropertyDefinitions(params PropertyGroup[] propertyGroups) : this(default(Type), propertyGroups) { }

        public PropertyDefinitions(Type type, PropertyGroup singleGroup)
        {
            TypeProperties = new Dictionary<Type, PropertyGroup[]>()
            {
                {type, new PropertyGroup[] { singleGroup } }
            };
        }

        //public PropertyDefinitions(params PropertyGroup[] properties) : this(default(Type), properties) { }

        //public PropertyDefinitions(string fullTypeName, params PropertyGroup[] properties)
        //    : this(TypeHelpers.GetTypeAnywhere(fullTypeName), properties) { }

        //public PropertyDefinitions(Type type)
        //    : this(type, new PropertyGroup(ALL_PROPERTIES, null)) { }

        /// <summary>
        /// Returns true if property named ALL_PROPERTIES is defined or all property groups are enabled for <paramref name="type"/>
        /// </summary>
        /// <param name="type">Type to check for</param>
        /// <returns>False if type is not found or the above condition isn't met</returns>
        public bool AllPropertiesEnabledForType(Type type)
        {
            if(!TypeProperties.ContainsKey(type))
                return false;

            var propGroups = TypeProperties.Where(t => t.Key == type).SelectMany(kv => kv.Value);
            return propGroups.FirstOrDefault(prop => prop.Name == ALL_PROPERTIES) != null
                || propGroups.All(t => t.Enabled);
        }

        public bool AnyPropertiesEnabledForType(Type type)
        {
            if(!TypeProperties.ContainsKey(type))
                return false;

            return TypeProperties
                .Where(t => t.Key == type)
                .SelectMany(kv => kv.Value)
                .Any(p => p.Enabled);
        }

        public PropertyGroup[] this[Type key]
        {
            get => TypeProperties[key];
        }
    }
}