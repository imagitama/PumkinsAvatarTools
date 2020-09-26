﻿#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.UI.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(DrawToggleLeftAttribute))]
    public class DrawToggleLeftAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(property.propertyType != SerializedPropertyType.Boolean)
            {
                EditorGUI.LabelField(position, label, new GUIContent("Use DrawToggleLeft with bool"));
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();
            bool newValue = EditorGUI.ToggleLeft(position, label, property.boolValue);
            if(EditorGUI.EndChangeCheck())
                property.boolValue = newValue;

            EditorGUI.EndProperty();
        }
    }
}
#endif