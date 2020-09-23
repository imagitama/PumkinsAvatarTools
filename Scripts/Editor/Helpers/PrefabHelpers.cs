#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Helpers
{
    public static class PrefabHelpers
    {
        public static bool HasPrefab(GameObject obj)
        {
            if(!obj)
                return false;

            var prefType = PrefabUtility.GetPrefabAssetType(obj);
            if(prefType == PrefabAssetType.MissingAsset || prefType == PrefabAssetType.NotAPrefab)
            {
                PumkinTools.Log("Not a prefab");
                return false;
            }
            return true;
        }
    }
}
#endif