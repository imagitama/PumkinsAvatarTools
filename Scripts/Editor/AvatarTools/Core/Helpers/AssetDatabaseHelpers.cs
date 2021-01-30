using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Pumkin.Core.Helpers
{
    public static class AssetDatabaseHelpers
    {
        public static bool IsBuiltInAsset(UnityEngine.Object obj)
        {
            return AssetDatabase.GetAssetPath(obj) == "Resources/unity_builtin_extra";
        }

        public static void SelectAndPing(UnityEngine.Object obj)
        {
            if(obj == null)
                return;

            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }

        public static void SelectAndPing(string assetPath)
        {
            if(string.IsNullOrWhiteSpace(assetPath))
                return;

            if(assetPath[assetPath.Length - 1] == '/')
                assetPath = assetPath.Substring(0, assetPath.Length - 1);

            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Object));
            SelectAndPing(obj);
        }
    }
}