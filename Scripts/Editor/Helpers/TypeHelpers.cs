using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Helpers
{
    class TypeHelpers
    {
        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if(type != null)
                return type;
            foreach(var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if(type != null)
                    return type;
            }
            return null;
        }
    }
}
