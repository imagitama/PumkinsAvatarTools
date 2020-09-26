#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pumkin.Extensions;

namespace Pumkin.AvatarTools.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    class AutoLoadAttribute : Attribute
    {
        private string parentModuleID;
        private string[] configurationStrings;
        private string id;

        /// <summary>
        /// The ID of the module or tool, used to organize the UI
        /// </summary>
        public string ID
        {
            get => id;
            private set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("ID cannot be null or empty");
                id = value.ToUpperInvariant();
            }
        }

        /// <summary>
        /// The iD of the parent module, used to organize the UI
        /// </summary>
        public string ParentModuleID
        {
            get => parentModuleID;
            set => parentModuleID = string.IsNullOrWhiteSpace(value) ? null : value.ToUpperInvariant();
        }

        /// <summary>
        /// Configuration names, used to only load modules or tools if the selected configuration matches, ex: vrchat
        /// </summary>
        public string[] ConfigurationStrings
        {
            get => configurationStrings;
            set => configurationStrings = value.IsNullOrEmpty() ? new string[] { PumkinTools.DEFAULT_CONFIGURATION } : value;
        }

        public AutoLoadAttribute(string id, params string[] configurationStrings)
        {
            ID = id;
            ConfigurationStrings = configurationStrings;
        }

        public static implicit operator bool(AutoLoadAttribute attr)
        {
            return !ReferenceEquals(attr, null);
        }
    }
}
#endif