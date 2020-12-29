#if UNITY_EDITOR
using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Assets.PumkinsAvatarTools.Scripts.Editor.Core.Extensions;
using Pumkin.AvatarTools2;
using UnityEditor;
using UnityEngine;

namespace Pumkin.Core.Helpers
{
    public static class UIHelpers
    {

        public static bool DrawFoldout(bool value, GUIContent content, bool toggleOnClick, GUIStyle style)
        {
            //Temporary wrapper until I decide to replace this with something nicer looking
            Rect rect = GUILayoutUtility.GetRect(content, Styles.Box, GUILayout.ExpandWidth(true));
            float oldLabel = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = rect.width;
            bool result = EditorGUI.Foldout(rect, value, content, true, style);
            EditorGUIUtility.labelWidth = oldLabel;
            return result;
            //return EditorGUILayout.Foldout(value, content, toggleOnClick, style);
        }

        public static void DrawLine(float height = 1f, bool spaceBefore = true, bool spaceAfter = true)
        {
            if(spaceBefore)
                EditorGUILayout.Space();
            GUILayout.Box(GUIContent.none, Styles.EditorLine, GUILayout.ExpandWidth(true), GUILayout.Height(height));
            if(spaceAfter)
                EditorGUILayout.Space();
        }

        public static void DrawInVerticalBox(Action action, GUIStyle style = null, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(style ?? Styles.Box, options);
            action.Invoke();
            EditorGUILayout.EndVertical();
        }

        public static void DrawInHorizontalBox(Action action, GUIStyle style = null, params GUILayoutOption[] options)
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

        public static void DrawFoldoutListScrolling<T>(List<T> list, ref bool expanded, ref Vector2 scroll,
            string label) where T : class
        {
            expanded = EditorGUILayout.Foldout(expanded, label);
            EditorGUILayout.BeginScrollView(scroll);
            if(expanded)
                DrawList(list);
            EditorGUILayout.EndScrollView();
        }

        public static void DrawFoldoutList<T>(List<T> list, ref bool expanded, string label) where T : class
        {
            expanded = EditorGUILayout.Foldout(expanded, label);
            if(expanded)
                DrawList(list);
        }

        /// <summary>
        /// Draws list as object fields with size field
        /// </summary>
        /// <typeparam name="T">Type of object in object field</typeparam>
        /// <param name="list">List of elements</param>
        /// <returns>True if list was changed</returns>
        public static bool DrawList<T>(List<T> list) where T : class
        {
            bool changed = false;
            EditorGUILayout.Space();

            int size = list.Count;
            EditorGUI.BeginChangeCheck();
            size = EditorGUILayout.IntField("Size", size);
            if(EditorGUI.EndChangeCheck())
            {
                changed = true;
                list.ResizeWithDefaults(size);
            }

            return DrawListElements(list) || changed;
        }

        public static bool DrawListWithAddButtonsScrolling<T>(List<T> list, ref Vector2 scroll, float minHeight, float maxHeight) where T : class
        {
            bool changed = false;
            EditorGUILayout.BeginHorizontal();
            {
                changed = DrawListElementsScrolling(list, ref scroll, minHeight, maxHeight, false);
                DrawAddButtons(list);
            }
            EditorGUILayout.EndHorizontal();
            return changed;
        }

        static void DrawAddButtons<T>(List<T> list) where T : class
        {
            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(Styles.IconButton.fixedWidth));
            {
                if(GUILayout.Button(Icons.Add, Styles.IconButton))
                    list.ResizeWithDefaults(list.Count + 1);
                if(GUILayout.Button(Icons.Remove, Styles.IconButton))
                    list.ResizeWithDefaults(list.Count - 1);
                if(GUILayout.Button(Icons.RemoveAll, Styles.IconButton))
                    list.Clear();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Draws a list of elements as object fields with Add, Remove and Clear buttons
        /// </summary>
        /// <typeparam name="T">Type of object in object field</typeparam>
        /// <param name="list">List of elements</param>
        /// <returns>True if list was changed</returns>
        public static bool DrawListWithAddButtons<T>(List<T> list) where T : class
        {
            bool changed = false;
            EditorGUILayout.BeginHorizontal();
            {
                changed = DrawListElements(list);
                DrawAddButtons(list);
            }
            EditorGUILayout.EndHorizontal();
            return changed;
        }

        /// <summary>
        /// Draws a list of elements as object fields
        /// </summary>
        /// <typeparam name="T">Type of object in object field</typeparam>
        /// <param name="list">List of elements</param>
        /// <returns>True if list was changed</returns>
        static bool DrawListElements<T>(List<T> list, bool drawLabels = true) where T : class
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            bool changed = false;
            GUIContent gc = GUIContent.none;
            for(int i = 0; i < list.Count; i++)
            {
                if(drawLabels)
                    gc = new GUIContent($"Element {i}");
                try
                {
                    EditorGUI.BeginChangeCheck();
                    var obj = EditorGUILayout.ObjectField(gc, list[i] as UnityEngine.Object, typeof(T),
                        true, GUILayout.ExpandWidth(true));
                    if(EditorGUI.EndChangeCheck())
                    {
                        list[i] = obj as T;
                        changed = true;
                    }
                }
                catch(Exception e)
                {
                    PumkinTools.LogException(e);
                }
            }
            EditorGUILayout.EndVertical();

            return changed;
        }

        static bool DrawListElementsScrolling<T>(List<T> list, ref Vector2 scroll, float minHeight = 30f, float maxHeight = 100f, bool drawLabels = true) where T : class
        {
            scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.MinHeight(minHeight), GUILayout.MaxHeight(maxHeight));
            bool changed = DrawListElements(list, drawLabels);
            EditorGUILayout.EndScrollView();
            return changed;
        }

        /// <summary>
        /// Draws UI elements inside as disabled or enabled
        /// </summary>
        /// <param name="enabled">Enable state</param>
        /// <param name="action">Inside UI elements</param>
        public static void DrawDisabled(bool enabled, Action action)
        {
            bool old = GUI.enabled;
            GUI.enabled = enabled;
            action?.Invoke();
            GUI.enabled = old;
        }

        /// <summary>
        /// Draws a label with it's width being tight around the text
        /// </summary>
        /// <param name="content"></param>
        /// <param name="style"></param>
        /// <param name="options"></param>
        public static void DrawTightLabel(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
            float textWidth = style.CalcSize(content).x;
            float labelWidth = EditorGUIUtility.labelWidth;
            float fieldWidth = EditorGUIUtility.fieldWidth;

            EditorGUIUtility.labelWidth = textWidth - 0.1f;
            EditorGUIUtility.fieldWidth = 0.00000001f;

            EditorGUILayout.LabelField(content, options);

            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUIUtility.fieldWidth = fieldWidth;
        }

        public static void DrawTightLabel(GUIContent content, params GUILayoutOption[] options)
        {
            DrawTightLabel(content, EditorStyles.label, options);
        }

        public static void DrawTightLabel(string label, GUIStyle style, params GUILayoutOption[] options)
        {
            DrawTightLabel(new GUIContent(label), style, options);
        }

        public static void DrawTightLabel (string label, params GUILayoutOption[] options)
        {
            DrawTightLabel(new GUIContent(label), EditorStyles.label, options);
        }
    }
}
#endif