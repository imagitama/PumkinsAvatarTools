#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Pumkin.Extensions
{
    public static class SerializedObjectExtensions
    {
        /// <summary>
        /// Invokes <paramref name="action"/> for each visible property of <paramref name="so"/>
        /// </summary>
        /// <param name="so">Object to iterate properties of</param>
        /// <param name="action"></param>
        public static void ForEachPropertyVisible(this SerializedObject so, bool enterChildren, Action<SerializedProperty> action)
        {
            SerializedProperty it = so.GetIterator().Copy();
            ForEachPropertyVisible(it, enterChildren, action);
        }

        /// <summary>
        /// Invokes <paramref name="action"/> for each property of <paramref name="so"/>
        /// </summary>
        /// <param name="so">Object to iterate properties of</param>
        /// <param name="action"></param>
        public static void ForEachProperty(this SerializedObject so, bool enterChildren, Action<SerializedProperty> action)
        {
            SerializedProperty it = so.GetIterator().Copy();
            ForEachProperty(it, enterChildren, action);
        }

        /// <summary>
        /// Invokes <paramref name="action"/> for each property starting with <paramref name="startProperty"/> inclusive.
        /// </summary>
        /// <param name="startProperty">Property to start from</param>
        /// <param name="action"></param>
        public static void ForEachProperty(this SerializedProperty startProperty, bool enterChildren, Action<SerializedProperty> action)
        {
            var prop = startProperty.Copy();
            while(prop.Next(enterChildren))
                action.Invoke(prop);
        }

        /// <summary>
        /// Invokes <paramref name="action"/> for each visible property starting with <paramref name="startProperty"/> inclusive.
        /// </summary>
        /// <param name="startProperty">Property to start from</param>
        /// <param name="action"></param>
        public static void ForEachPropertyVisible(this SerializedProperty startProperty, bool enterChildren, Action<SerializedProperty> action)
        {
            var prop = startProperty.Copy();
            while(prop.NextVisible(enterChildren))
                action.Invoke(prop);
        }
    }
}
#endif