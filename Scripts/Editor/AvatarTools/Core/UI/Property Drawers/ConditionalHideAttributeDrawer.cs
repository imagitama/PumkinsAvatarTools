using Pumkin.Core;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.UI.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
    public class ConditionalHideAttributeDrawer : PropertyDrawer
    {
        SerializedProperty serialProp = null;
        bool? CanDraw
        {
            get
            {
                if(serialProp == null)
                    return null;

                if(canDraw == null || canDraw == false)
                {
                    var attr = attribute as ConditionalHideAttribute;
                    var props = fieldInfo.DeclaringType.GetProperties(
                        BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

                    foreach(var prop in props)
                    {
                        if(prop == null
                            || prop.PropertyType != typeof(bool)
                            || !attr.PropertyNames.Contains(prop.Name, StringComparer.OrdinalIgnoreCase))
                            continue;

                        var val = prop.GetValue(serialProp.serializedObject.targetObject);
                        canDraw = val as bool? ?? false;
                        if(canDraw == false)
                            break;
                    }
                }
                return canDraw;
            }
        }
        bool? canDraw = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            serialProp = property;
            if(CanDraw == null || CanDraw == false)
                return;

            if(property.propertyType != SerializedPropertyType.Boolean)
            {
                if(property.type == typeof(UISpacer).Name)
                    label = GUIContent.none;

                EditorGUI.PropertyField(position, property, label);
                return;
            }

            //Draw toggle on the left if it's a bool
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            bool newValue = EditorGUI.ToggleLeft(position, label, property.boolValue);
            if(EditorGUI.EndChangeCheck())
                property.boolValue = newValue;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return CanDraw == true ? base.GetPropertyHeight(property, label) : 0;
        }
    }
}