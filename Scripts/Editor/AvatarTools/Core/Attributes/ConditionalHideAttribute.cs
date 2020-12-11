using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.Core
{
    using UnityEngine;
    using System;
    using System.Collections;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ConditionalHideAttribute : PropertyAttribute
    {
        /// <summary>
        /// The name of the bool field that will be in control
        /// </summary>
        public string ConditionalSourceField = "";
        
        /// <summary>
        /// TRUE = Hide in inspector / FALSE = Disable in inspector 
        /// </summary>
        public bool HideInInspector = false;

        public ConditionalHideAttribute(string conditionalSourceField)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = false;
        }

        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = hideInInspector;
        }
    }
}
