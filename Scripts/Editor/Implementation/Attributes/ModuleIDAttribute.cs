using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    class ModuleIDAttribute : Attribute
    {
        public string ID { get; private set; }
        public ModuleIDAttribute(string idString)
        {
            ID = idString;
        }
    }
}
