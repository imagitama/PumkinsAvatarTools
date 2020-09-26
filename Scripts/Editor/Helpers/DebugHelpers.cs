#if UNITY_EDITOR && PUMKIN_DEV
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Helpers
{
    public static class DebugHelpers
    {
        public static void PrintPublicProperties(GameObject obj)
        {
            if(!obj)
                return;
            PrintPublicProperties(obj);
        }

        public static void PrintPublicProperties(SerializedObject obj)
        {
            if(obj == null)
                return;

            SerializedProperty it = obj.GetIterator().Copy();
            while(it.Next(true))
                Debug.Log(it.name);
        }

        public static void DumpDefaultGUISkin()
        {
            var old = GUI.skin;
            GUI.skin = null;
            var def = GUI.skin;
            var skin = ScriptableObject.CreateInstance<GUISkin>();
            AssetDatabase.CreateAsset(skin, @"Assets\PumkinsAvatarTools\Resources\UI\DefaultSkin.GUISkin");
            GUI.skin = old;
        }
    }
}
#endif