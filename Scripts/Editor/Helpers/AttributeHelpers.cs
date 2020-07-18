using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.UnityTools.Helpers
{
    public static class AttributeHelpers
    {
        public static Attribute GetAttribute<TAttribute>(object obj) where TAttribute : Attribute
        {
            return obj.GetType().GetCustomAttribute<TAttribute>();
        }
    }
}
