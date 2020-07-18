using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.UnityTools.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    class AutoLoadAttribute : Attribute
    {
        public string ID { get; private set; }
        public string ParentModuleID { get; private set; }

        public AutoLoadAttribute(string id, string parentModuleID = null)
        {            
            if(id == null)
                throw new NullReferenceException("ID cannot be null");
            ID = id?.ToUpperInvariant();
            ParentModuleID = string.IsNullOrWhiteSpace(parentModuleID) ? null : parentModuleID.ToUpperInvariant();
        }
    }
}
