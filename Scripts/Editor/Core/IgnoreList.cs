using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using UnityEngine;

namespace Pumkin.AvatarTools.Core
{
    public class IgnoreList : IDisposable
    {
        Delegates.SelectedChangeHandler selectionChangeHandler;

        List<Transform> ignoredTransforms = new List<Transform>();
        List<string> ignoredTransformPaths = new List<string>();

        bool Enabled { get; set; } = true;

        public bool IncludeChildren { get; set; } = true;

        public IgnoreList(Delegates.SelectedChangeHandler selectionChangeHandler)
        {
            this.selectionChangeHandler = selectionChangeHandler;
            selectionChangeHandler += SelectionChanged;
        }

        void SelectionChanged(GameObject selection)
        {
            if(selection)
                PathsToTransformList(selection);
            else
                TransformListToPaths(selection);
        }

        void TransformListToPaths(GameObject obj)
        {
            ignoredTransformPaths.Clear();
            foreach(var t in ignoredTransforms)
            {
                if(!t)
                    continue;
                ignoredTransformPaths.Add(t.GetHierarchyPath(t));
            }
        }

        void PathsToTransformList(GameObject obj)
        {
            ignoredTransforms.Clear();
            foreach(var tPath in ignoredTransformPaths)
            {
                var t = obj.transform.Find(tPath);
                if(t)
                    ignoredTransforms.Add(t);
            }
        }

        public bool ShouldIgnoreTransform(Transform trans)
        {
            if(!Enabled || (!trans || ignoredTransforms == null) || trans == trans.root)
                return false;

            if(ignoredTransforms.Count > 0 && IncludeChildren)
            {
                var t = trans;
                do
                {
                    if(ignoredTransforms.Contains(t))
                        return true;
                    t = t.parent;
                } while(t != null && t != t.root);
                return false;
            }

            if(ignoredTransforms.Contains(trans))
                return true;
            return false;
        }

        public void Dispose()
        {
            selectionChangeHandler -= SelectionChanged;
        }
    }
}
