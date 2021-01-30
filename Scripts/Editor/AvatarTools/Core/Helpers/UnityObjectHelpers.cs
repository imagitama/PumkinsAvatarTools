using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Pumkin.Core.Helpers
{
    public static class UnityObjectHelpers
    {
        public static void DestroyAppropriate(UnityEngine.Object obj, bool registerUndo = false)
        {
            if(EditorApplication.isPlaying)
                UnityEngine.Object.Destroy(obj);
            else if(registerUndo)
                Undo.DestroyObjectImmediate(obj);
            else
                UnityEngine.Object.DestroyImmediate(obj);
        }
    }
}