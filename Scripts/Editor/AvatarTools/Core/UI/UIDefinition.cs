using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.Core.UI
{
    /// <summary>
    /// Used to define UI related stuff.
    /// Note that not all of these are applicable to every UI item
    /// </summary>
    public class UIDefinition
    {
        /// <summary>
        /// The name of this item in the UI
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of this item in the UI
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The order of which this will item will be drawn in the UI
        /// </summary>
        public int OrderInUI { get; set; }

        /// <summary>
        /// Module styles for this item. Only works with UI Modules
        /// </summary>
        public List<UIModuleStyles> ModuleStyles { get; set; }

        /// <summary>
        /// Whether or not this item should be enabled in the UI
        /// </summary>
        public bool EnabledInUI { get; set; }

        /// <summary>
        /// Whether or not this item should be expanded in the UI
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Whether or not this item should be hidden in the UI
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Whether or not this items should have it's settings expanded in the UI
        /// </summary>
        public bool ExpandSettings { get; set; }

        public UIDefinition(string name, string description, int orderInUI, params UIModuleStyles[] moduleStyles)
        {
            Name = name;
            Description = description;
            OrderInUI = orderInUI;
            ModuleStyles = moduleStyles?.ToList() ?? new List<UIModuleStyles>();

            EnabledInUI = true;
            IsExpanded = false;
            IsHidden = false;
            ExpandSettings = false;
        }

        public UIDefinition(string name, string description, params UIModuleStyles[] moduleStyles)
            : this(name, description, 0, null) { }

        public UIDefinition(string name, int orderInUI, params UIModuleStyles[] moduleStyles)
            : this(name, null, orderInUI, moduleStyles) { }

        public UIDefinition(string name, params UIModuleStyles[] moduleStyles)
            : this(name, null, 0, moduleStyles) { }

        /// <summary>
        /// Forces IItem to draw alone even when DrawChildrenInHorizontalPairs is defined in module. Only works for IItem
        /// </summary>
        public bool DrawAlone { get; set; }

        public bool HasStyle(UIModuleStyles style) =>
            ModuleStyles.Exists(t => t == style);

        public static implicit operator bool(UIDefinition uid)
        {
            return !ReferenceEquals(uid, null);
        }
    }
}
