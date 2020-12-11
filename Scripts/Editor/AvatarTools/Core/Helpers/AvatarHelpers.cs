using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.Core.Helpers
{
    public static class AvatarHelpers
    {
        public static Transform GetArmature(GameObject avatar)
        {
            if(!avatar)
                return null;
            return GetArmature(avatar.transform);
        }

        public static Transform GetArmature(Transform avatarRoot)
        {
            if(!avatarRoot)
                return null;

            Transform armature = null;
            if((armature = avatarRoot.transform.Find("Armature")) == null)
            {
                //Get the GUID of the avatar, which should also identify the mesh data
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(avatarRoot.GetComponent<Animator>(), out string guid, out long _);
                var renders = avatarRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                foreach(var ren in renders)
                {
                    AssetDatabase.TryGetGUIDAndLocalFileIdentifier(ren.sharedMesh, out string meshGuid, out long _);
                    if(guid != meshGuid)    //Find the correct SkinnedMeshRenderer by comparing this renderer's mesh to the one our animator is using
                        continue;

                    if(ren.rootBone != ren.transform.root) //Get the parent of the root bone, which should be the armature if root is hips
                        armature = ren.rootBone.parent;
                    else
                        armature = ren.rootBone;
                    break;
                }
            }

            return armature;
        }

        public static bool IsHumanoid(GameObject avatar)
        {
            Animator anim = avatar.GetComponent<Animator>();
            return !(anim == null || !anim.isHuman);
        }

        public static Vector3 GetEyePosition(GameObject avatar)
        {
            var anim = avatar.GetComponent<Animator>();
            if(anim.isHuman)
            {
                Transform eye = anim.GetBoneTransform(HumanBodyBones.LeftEye) ?? anim.GetBoneTransform(HumanBodyBones.RightEye);
                Transform head = anim.GetBoneTransform(HumanBodyBones.Head);

                return new Vector3(head.transform.position.x, eye.transform.position.y, eye.transform.position.z);
            }
            return default;
        }

        public static Vector3 GetEyePositionLocal(GameObject avatar)
        {
            return GetEyePosition(avatar) - avatar.transform.position;
        }
    }
}
