#if UNITY_EDITOR
using Pumkin.UnityTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.UnityTools.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]    
    class UIDefinitionAttribute : Attribute
    {
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public int OrderInUI { get; set; }
        public List<string> ModuleStyles { get; set; }

        public UIDefinitionAttribute(string friendlyName, params string[] styles)
        {
            FriendlyName = friendlyName;
            ModuleStyles = styles.ToList() ?? new List<string>();
        }
    }
}
#endif