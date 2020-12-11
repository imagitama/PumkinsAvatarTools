#if UNITY_EDITOR
using Pumkin.AvatarTools2;
using Pumkin.Core.Extensions;
using System;
using System.Linq;

namespace Pumkin.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AutoLoadAttribute : Attribute
    {
        public enum AutoLoadInMode { Editor, Play, Both };
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

        public bool IsGenericItem
            => configurationStrings.Length == 1 && configurationStrings.Contains(ConfigurationManager.DEFAULT_CONFIGURATION);

        /// <summary>
        /// The ID of the parent module, used to organize the UI
        /// </summary>
        public string ParentModuleID
        {
            get => parentModuleID;
            set => parentModuleID = string.IsNullOrWhiteSpace(value) ? null : value.ToUpperInvariant();
        }

        /// <summary>
        /// Returns true if ParentModuleID is not null or whitespace
        /// </summary>
        public bool HasParent
            => !string.IsNullOrWhiteSpace(ParentModuleID);

        //public AutoLoadInMode AutoLoadMode { get; set; } = AutoLoadInMode.Both;

        /// <summary>
        /// Configuration names, used to only load modules or tools if the selected configuration matches, ex: vrchat
        /// </summary>
        public string[] ConfigurationStrings
        {
            get => configurationStrings;
            set => configurationStrings = value.IsNullOrEmpty() ? new string[] { ConfigurationManager.DEFAULT_CONFIGURATION } : value;
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