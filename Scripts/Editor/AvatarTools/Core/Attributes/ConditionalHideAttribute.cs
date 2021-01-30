using UnityEngine;

namespace Pumkin.Core
{
    /// <summary>
    /// Allows you to hide fields from being drawn in the inspector based on the value of a bool property indicated by PropertyName
    /// </summary>
    public class ConditionalHideAttribute : PropertyAttribute
    {
        public string PropertyName { get; set; }

        /// <summary>
        /// Allows you to hide fields from being drawn in the inspector based on the value of <paramref name="boolPropertyName"/>
        /// </summary>
        /// <param name="boolPropertyName">Name of bool property on the same object as the object this attribute's field is on</param>
        public ConditionalHideAttribute(string boolPropertyName)
        {
            PropertyName = boolPropertyName;
        }
    }
}