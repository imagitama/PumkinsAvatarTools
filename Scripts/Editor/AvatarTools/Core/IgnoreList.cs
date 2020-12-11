using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.PumkinsAvatarTools.Scripts.Editor.Core.Extensions;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.Extensions;
using Pumkin.Core.Helpers;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pumkin.AvatarTools2.Core
{
    public class IgnoreList : IDisposable
    {
        Delegates.SelectedChangeHandler selectionChangeHandler;

        List<Transform> ignoredTransforms = new List<Transform>();
        List<string> ignoredTransformPaths = new List<string>();

        public bool enabled = true;
        public bool includeChildren = true;
        bool expanded = false;

        Vector2 scrollPos = Vector2.zero;
        static float minHeight = 30f;
        static float maxHeight = 100f;
        static string label = "Ignore objects";
        static string childrenLabel = "Include children";

        SerializedObject serializedIgnoreList;

        public IgnoreList(Delegates.SelectedChangeHandler selectionChangeHandler)
        {
            this.selectionChangeHandler = selectionChangeHandler;
            selectionChangeHandler += SelectionChanged;

            if(ignoredTransforms.Count == 0)
                ignoredTransforms.ResizeWithDefaults(1);
        }

        void SelectionChanged(GameObject selection)
        {
            PumkinTools.LogVerbose($"IgnoreList selection changed to {selection}");

            if(selection)
                PathsToTransformList(selection);
            else
                TransformListToPaths();
        }

        void PathsToTransformList(GameObject obj)
        {
            ignoredTransforms.Clear();
            foreach(var path in ignoredTransformPaths)
            {
                var t = obj.transform.Find(path);
                if(t)
                    ignoredTransforms.Add(t);
            }
        }

        void TransformListToPaths()
        {
            ignoredTransformPaths.Clear();
            foreach(var trans in ignoredTransforms)
                ignoredTransformPaths.Add(trans.GetPathInHierarchy());
        }

        public bool ShouldIgnoreTransform(Transform trans)
        {
            if(!enabled || ignoredTransforms.Count == 0 || (!trans || ignoredTransforms == null))
                return false;

            var t = trans;
            string path = t.GetPathInHierarchy();
            if(ignoredTransforms.Count > 0 && includeChildren)
            {
                do
                {
                    if(ignoredTransformPaths.Contains(path, StringComparer.InvariantCultureIgnoreCase))
                        return true;
                    t = t.parent;
                    path = t.GetPathInHierarchy();
                }
                while(t != null);
                return false;
            }

            if(ignoredTransformPaths.Contains(path, StringComparer.InvariantCultureIgnoreCase))
                return true;
            return false;
        }

        public void DrawUI()
        {
            UIHelpers.VerticalBox(() =>
            {
                EditorGUILayout.BeginHorizontal();
                enabled = EditorGUILayout.ToggleLeft(label, enabled);
                expanded = GUILayout.Toggle(expanded, Icons.Options, Styles.Icon);
                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginDisabledGroup(!enabled);
                if(expanded)
                {
                    UIHelpers.DrawGUILine();
                    if(UIHelpers.DrawListWithAddButtonsScrolling(ignoredTransforms, ref scrollPos, minHeight, maxHeight))
                        TransformListToPaths();

                    UIHelpers.DrawGUILine();
                    includeChildren = GUILayout.Toggle(includeChildren, childrenLabel);
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.Space();
            }, Styles.CopierBox);
        }

        public void Dispose()
        {
            selectionChangeHandler -= SelectionChanged;
        }
    }
}
