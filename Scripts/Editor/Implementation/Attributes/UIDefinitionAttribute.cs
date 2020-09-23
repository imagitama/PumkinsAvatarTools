#if UNITY_EDITOR
using Pumkin.AvatarTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]    
    class UIDefinitionAttribute : Attribute
    {
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public int OrderInUI { get; set; }
        public List<string> ModuleStyles { get; set; }

        public UIDefinitionAttribute(string friendlyName, params string[] uiModuleStyles)
        {
            FriendlyName = friendlyName;
            ModuleStyles = uiModuleStyles.ToList() ?? new List<string>();
        }
    }
}
#endif