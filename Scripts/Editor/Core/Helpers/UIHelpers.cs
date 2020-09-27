#if UNITY_EDITOR
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using Pumkin.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.Core.Helpers
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

        public static void VerticalBox(Action action, GUIStyle style = null, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(style ?? Styles.Box, options);
            action.Invoke();
            EditorGUILayout.EndVertical();
        }

        public static void HorizontalBox(Action action, GUIStyle style = null, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(style ?? Styles.Box, options);
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

        public static void DrawUIPairs(List<IItem> items)
        {
            if(items.Count == 0)
                return;

            float width = GUILayoutUtility.GetLastRect().width / 2;
            var options = new GUILayoutOption[] { GUILayout.Width(width), GUILayout.ExpandWidth(true) };

            using(var sub = items.GetFlipFlopEnumerator())
            {
                bool beganHorizontal = false;
                while(sub.MoveNext())
                {
                    if(beganHorizontal && sub.Current.Settings != null) //End pair if second item has settings
                    {
                        EditorGUILayout.EndHorizontal();
                        beganHorizontal = false;
                    }
                    else if(!sub.FlipState && sub.Current.Settings == null)  //Don't start a pair if current item has settings
                    {
                        EditorGUILayout.BeginHorizontal();
                        beganHorizontal = true;
                    }

                    sub.Current?.DrawUI(options);

                    if(sub.FlipState && beganHorizontal)
                    {
                        EditorGUILayout.EndHorizontal();
                        beganHorizontal = false;
                    }
                }

                if(beganHorizontal)
                    EditorGUILayout.EndHorizontal();
            }
        }
    }
}
#endif