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

        public static void DrawDebugStar(Vector3 position, float size, float duration)
        {
            for(int i = 0; i < 3; i++)
            {
                float f1 = Random.Range(-size / 2, size / 2);
                float f2 = Random.Range(-size / 2, size / 2);
                float f3 = Random.Range(-size / 2, size / 2);
                Debug.DrawLine(position, position + new Vector3(f1,f2,f3), Color.white, duration);
            }
        }
    }
}