#if UNITY_EDITOR
using Pumkin.AvatarTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    class UIDefinitionAttribute : Attribute
    {
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public int OrderInUI { get; set; }
        public List<UIModuleStyles> ModuleStyles { get; set; }

        public UIDefinitionAttribute(string friendlyName, params UIModuleStyles[] uiModuleStyles)
        {
            FriendlyName = friendlyName;
            ModuleStyles = uiModuleStyles.ToList() ?? new List<UIModuleStyles>();
        }

        public bool HasStyle(UIModuleStyles style)
        {
            return ModuleStyles.Exists(t => t == style);
        }
    }
}
#endif