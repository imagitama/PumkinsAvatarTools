using Pumkin.Core;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.UI.PropertyDrawers
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