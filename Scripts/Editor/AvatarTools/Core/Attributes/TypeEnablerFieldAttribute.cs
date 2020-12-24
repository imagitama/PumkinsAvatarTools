using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.Core
{
    /// <summary>
    /// Allows a bool field to decide whether a type is enabled or disabled in a copier or destroyer
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class TypeEnablerFieldAttribute : Attribute
    {
        public Type EnabledType { get; private set; }

        public TypeEnablerFieldAttribute(Type enabledType)
        {
            EnabledType = enabledType;
        }

        public TypeEnablerFieldAttribute(string enabledTypeFullName)
        {
            EnabledType = TypeHelpers.GetTypeAnwhere(enabledTypeFullName);
        }
    }
}
