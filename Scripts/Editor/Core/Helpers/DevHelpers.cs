#if UNITY_EDITOR && PUMKIN_DEV
using UnityEditor;
using UnityEngine;

namespace Pumkin.Core.Helpers
{
    public static class DevHelpers
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

        public static float[] GetHumanMusclesFromCurrentPose(GameObject avatarObj)
        {
            var anim = avatarObj.GetComponent<Animator>();
            var hph = new HumanPoseHandler(anim.avatar, anim.transform);
            var hp = new HumanPose();
            hph.GetHumanPose(ref hp);
            return hp.muscles;
        }
    }
}
#endif