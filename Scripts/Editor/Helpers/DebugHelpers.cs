#if UNITY_EDITOR
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
    }
}
#endif