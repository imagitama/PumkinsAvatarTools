#if UNITY_EDITOR
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
    }
}
#endif