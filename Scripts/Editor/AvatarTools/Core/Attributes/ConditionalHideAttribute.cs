using Pumkin.Core.Extensions;
using UnityEngine;

namespace Pumkin.Core
{
    /// <summary>
    /// Allows you to hide fields from being drawn in the inspector based on the value of a bool property indicated by PropertyName
    /// </summary>
    public class ConditionalHideAttribute : PropertyAttribute
    {
        public string[] PropertyNames { get; set; }

        /// <summary>
        /// Allows you to hide fields from being drawn in the inspector based on the value of <paramref name="boolPropertyNames"/>
        /// Hidden if all are true
        /// </summary>
        /// <param name="boolPropertyName">Name of bool properties in the same class as the field this attribute is on, all must be true to hide</param>
        public ConditionalHideAttribute(params string[] boolPropertyNames)
        {
            PropertyNames = boolPropertyNames;
        }
    }
}