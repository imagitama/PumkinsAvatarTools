#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Implementation.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools.SubTools
{
    [AutoLoad("tools_tpose", ParentModuleID = "tools_fixAvatar")]
    [UIDefinition("Force T-Pose", Description = "Sets your humanoid avatar into a T-Pose")]
    class ForceTPose : SubToolBase
    {
        readonly float[] TPOSE_MUSCLES =
        {
             -6.830189E-07f, 4.268869E-08f, 4.268868E-08f, -8.537737E-08f, 0f, 0f, 0f, 0f, 0f, 4.268868E-08f, -8.537737E-08f, 4.268868E-08f, 3.415095E-07f,
              0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.5994893f, 0.004985309f, 0.00297395f, 0.9989594f, -0.02284526f, -3.449878E-05f, -0.0015229f, -4.781132E-07f,
              0.599489f, 0.004985378f, 0.002974245f, 0.9989589f, -0.02284535f, -3.548912E-05f, -0.001522672f, -1.024528E-07f, -2.429621E-07f, 1.549688E-07f,
              0.3847253f, 0.310061f, -0.103543f, 0.9925866f, 0.159403f, -0.01539368f, 0.01405432f, 5.680533E-08f, 2.701393E-07f, 0.3847259f, 0.3100605f,
              -0.1035404f, 0.9925874f, 0.1593992f, -0.01539393f, 0.01405326f, -0.7706841f, 0.423209f, 0.6456643f, 0.6362566f, 0.6677276f, -0.4597229f,
              0.811684f, 0.8116837f, 0.6683907f, -0.5737826f, 0.8116839f, 0.8116843f, 0.6670681f, -0.6459302f, 0.8116837f, 0.8116839f, 0.666789f,
              -0.4676594f, 0.811684f, 0.8116839f, -0.7706831f, 0.4232127f, 0.6456538f, 0.6362569f, 0.6678051f, -0.4589976f, 0.8116843f, 0.8116842f,
              0.668391f, -0.5737844f, 0.811684f, 0.8116837f, 0.6669571f, -0.6492739f, 0.8116841f, 0.8116843f, 0.6667888f, -0.4676568f, 0.8116842f, 0.8116836f
        };
        Animator anim;

        protected override bool Prepare(GameObject target)
        {
            if(base.Prepare(target))
            {
                anim = target.GetComponent<Animator>(); //Check if target is humanoid
                if(anim == null || !anim.isHuman)
                {
                    Debug.LogError($"'{target.name}' is not humanoid");
                    return false;
                }
                return true;
            }
            return false;
        }

        protected override bool DoAction(GameObject target)
        {
            Vector3 pos = target.transform.position;
            Quaternion rot = target.transform.rotation;

            target.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            var humanPoseHandler = new HumanPoseHandler(anim.avatar, target.transform);

            var humanPose = new HumanPose();
            humanPoseHandler.GetHumanPose(ref humanPose);

            //Find the Armature
            Transform armature;
            if((armature = target.transform.Find("Armature")) == null)
            {
                //Get the GUID of the avatar, which should also identify the mesh data, for some reason
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(anim.avatar, out string guid, out long _);
                var renders = target.GetComponentsInChildren<SkinnedMeshRenderer>(true);
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

            if(!armature)
            {
                Debug.LogError("Can't find armature. Tried searching for 'Armature' and looking through SkinnedMeshRenderers' root bones.");
                return false;
            }

            //Change body position.y to prevent sinking too much
            if(!(armature && armature.localScale == Vector3.one))
            {
                if(humanPose.bodyPosition.y < 1 && !Mathf.Approximately(humanPose.bodyPosition.y, 1))
                    humanPose.bodyPosition.y = 1;
            }

            humanPose.muscles = TPOSE_MUSCLES;

            humanPoseHandler.SetHumanPose(ref humanPose);
            target.transform.SetPositionAndRotation(pos, rot);

            return true;
        }
    }
}
#endif