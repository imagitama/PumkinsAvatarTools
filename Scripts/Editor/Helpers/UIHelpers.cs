#if UNITY_EDITOR
using Pumkin.AvatarTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Helpers
{
    public static class UIHelpers
    {
        public static bool DrawFoldout(bool value, GUIContent content, bool toggleOnClick, GUIStyle style)
        {
            return EditorGUILayout.Foldout(value, content, toggleOnClick, style);
        }

        public static void DrawGUILine(float height = 1f, bool spaceBefore = true, bool spaceAfter = true)
        {
            if(spaceBefore)
                EditorGUILayout.Space();
            GUILayout.Box(GUIContent.none, Styles.EditorLine, GUILayout.ExpandWidth(true), GUILayout.Height(height));
            if(spaceAfter)
                EditorGUILayout.Space();
        }

        public static void VerticalBox(Action action, GUIStyle style = null)
        {
            EditorGUILayout.BeginVertical(style ?? Styles.Box);
            action.Invoke();
            EditorGUILayout.EndVertical();
        }
        public static void HorizontalBox(Action action, GUIStyle style = null)
        {
            EditorGUILayout.BeginHorizontal(style ?? Styles.Box);
            action.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawIndented(int indentLevel, Action action)
        {
            int old = EditorGUI.indentLevel;
            EditorGUI.indentLevel = indentLevel;
            action.Invoke();
            EditorGUI.indentLevel = old;
        }
    }
}
#endif