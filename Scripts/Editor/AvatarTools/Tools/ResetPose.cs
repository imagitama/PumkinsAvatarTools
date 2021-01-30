using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    [AutoLoad(DefaultIDs.Tools.ResetPose, ParentModuleID = DefaultIDs.Modules.Tools_FixAvatar)]
    class ResetPose : ToolBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Reset Pose", "Reverts the location, rotation and scale of your avatar back to avatar definition, prefab, T-Pose or A-Pose");

        protected override bool DoAction(GameObject target)
        {
            var settings = Settings as ResetPose_Settings;
            switch(settings.resetType)
            {
                case ResetPose_Settings.ResetType.ToAvatarDefinition:
                    ResetPoseToAvatarDefinition(target.GetComponent<Animator>(), settings.position, settings.rotation, settings.scale);
                    break;
                case ResetPose_Settings.ResetType.ToPrefab:
                    return ResetToPrefab(target, settings.position, settings.rotation, settings.scale);
                case ResetPose_Settings.ResetType.ToTPose:
                    return ResetToPose(target, true);
                case ResetPose_Settings.ResetType.ToAPose:
                    return ResetToPose(target, false);
            }
            return true;
        }

        /// <summary>
        /// Dreadrith's reset to avatar definition
        /// </summary>
        /// <param name="anim"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        public static void ResetPoseToAvatarDefinition(Animator anim, bool position, bool rotation, bool scale)
        {
            if(!anim || !anim.avatar)
            {
                Debug.LogWarning("Humanoid avatar is required to reset pose!");
                return;
            }

            // Humans IDs if not full reset, otherwise All Ids
            // ID > Path
            // ID > Element > Transform Data

            SerializedObject sAvi = new SerializedObject(anim.avatar);
            SerializedProperty humanIds = sAvi.FindProperty("m_Avatar.m_Human.data.m_Skeleton.data.m_ID");
            SerializedProperty allIds = sAvi.FindProperty("m_Avatar.m_AvatarSkeleton.data.m_ID");
            SerializedProperty defaultPose = sAvi.FindProperty("m_Avatar.m_DefaultPose.data.m_X");
            SerializedProperty tos = sAvi.FindProperty("m_TOS");

            var idToElem = new Dictionary<long, int>();
            var elemToTransform = new Dictionary<int, TransformData>();
            var IdToPath = new Dictionary<long, string>();

            for(int i = 0; i < allIds.arraySize; i++)
                idToElem.Add(allIds.GetArrayElementAtIndex(i).longValue, i);

            for(int i = 0; i < defaultPose.arraySize; i++)
                elemToTransform.Add(i, new TransformData(defaultPose.GetArrayElementAtIndex(i)));

            for(int i = 0; i < tos.arraySize; i++)
            {
                SerializedProperty currProp = tos.GetArrayElementAtIndex(i);
                IdToPath.Add(currProp.FindPropertyRelative("first").longValue, currProp.FindPropertyRelative("second").stringValue);
            }

            System.Action<Transform, TransformData> applyTransform = (transform, data) =>
            {
                if(transform)
                {
                    if(position)
                        transform.localPosition = data.pos;
                    if(rotation)
                        transform.localRotation = data.rot;
                    if(scale)
                        transform.localScale = data.scale;
                }
            };

            for(int i = 0; i < allIds.arraySize; i++)
            {
                Transform myBone = anim.transform.Find(IdToPath[allIds.GetArrayElementAtIndex(i).longValue]);
                TransformData data = elemToTransform[idToElem[allIds.GetArrayElementAtIndex(i).longValue]];
                applyTransform(myBone, data);
            }
        }

        bool ResetToPose(GameObject target, bool useTPose)
        {
            Vector3 pos = target.transform.position;
            Quaternion rot = target.transform.rotation;
            Animator anim = target.GetComponent<Animator>();

            target.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            var humanPoseHandler = new HumanPoseHandler(anim.avatar, target.transform);

            var humanPose = new HumanPose();
            humanPoseHandler.GetHumanPose(ref humanPose);

            //Find the Armature
            Transform armature = AvatarHelpers.GetArmature(target);

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

            humanPose.muscles = useTPose ? TPOSE_MUSCLES : APOSE_MUSCLES;

            humanPoseHandler.SetHumanPose(ref humanPose);
            target.transform.SetPositionAndRotation(pos, rot);

            return true;
        }

        bool ResetToPrefab(GameObject target, bool position, bool rotation, bool scale)
        {
            var settings = Settings as ResetPose_Settings;

            var overrides = PrefabUtility.GetObjectOverrides(target)
                    ?.Where(t => t.instanceObject.GetType() == _transformType && t.instanceObject);

            foreach(var o in overrides)
            {
                if(settings.position && settings.rotation && settings.scale)
                    o.Revert();
                else
                {
                    var changedTransform = o.instanceObject as Transform;
                    var originalTransform = o.GetAssetObject() as Transform;

                    if(position)
                        changedTransform.localPosition = originalTransform.localPosition;
                    if(rotation)
                        changedTransform.localRotation = originalTransform.localRotation;
                    if(scale)
                        changedTransform.localScale = originalTransform.localScale;
                }
            }
            return true;
        }

        struct TransformData
        {
            public Vector3 pos;
            public Quaternion rot;
            public Vector3 scale;
            public TransformData(SerializedProperty t)
            {
                SerializedProperty tProp = t.FindPropertyRelative("t");
                SerializedProperty qProp = t.FindPropertyRelative("q");
                SerializedProperty sProp = t.FindPropertyRelative("s");
                pos = new Vector3(tProp.FindPropertyRelative("x").floatValue, tProp.FindPropertyRelative("y").floatValue, tProp.FindPropertyRelative("z").floatValue);
                rot = new Quaternion(qProp.FindPropertyRelative("x").floatValue, qProp.FindPropertyRelative("y").floatValue, qProp.FindPropertyRelative("z").floatValue, qProp.FindPropertyRelative("w").floatValue);
                scale = new Vector3(sProp.FindPropertyRelative("x").floatValue, sProp.FindPropertyRelative("y").floatValue, sProp.FindPropertyRelative("z").floatValue);
            }
        }

        static readonly float[] TPOSE_MUSCLES =
        {
            -6.403302E-07f , 1.28066E-07f , -8.537736E-08f , -1.908333E-15f , 4.268868E-08f , -1.28066E-07f , 0f , 0f , 0f , 4.268868E-08f , 6.361108E-16f , -4.268868E-08f , 3.415095E-07f , 0f ,
            0f , 0f , 0f , 0f , 0f , 0f , 0f , 0.5994843f , 0.004883454f , 0.001518556f , 0.9990439f , -0.02189369f , -3.493093E-05f ,
            -0.001662011f , -4.098113E-07f , 0.5994844f , 0.004883243f , 0.001518792f , 0.9990438f , -0.02189348f , -3.622868E-05f , -0.001662011f , -3.415094E-08f , -1.353976E-06f , 1.298447E-07f , 0.3844599f ,
            0.3100845f , -0.05168834f , 0.9894112f , 0.10771f , -0.01572796f , 0.01416865f , -1.633175E-06f , -1.686203E-06f , 0.3844602f , 0.3100847f , -0.05168702f , 0.9894045f , 0.1077081f ,
            -0.01572749f , 0.01416702f , -0.7706841f , 0.4232089f , 0.6456648f , 0.6362566f , 0.6677278f , -0.459723f , 0.8116836f , 0.8116835f , 0.6683906f , -0.5737829f , 0.8116837f ,
            0.8116842f , 0.6670679f , -0.6459293f , 0.8116834f , 0.8116842f , 0.666789f , -0.4676591f , 0.8116842f , 0.811684f , -0.770683f , 0.423213f , 0.645654f , 0.6362573f ,
            0.6678046f , -0.4589979f , 0.8116842f , 0.811684f , 0.668391f , -0.5737841f , 0.8116837f , 0.8116836f , 0.6669568f , -0.6492749f , 0.8116842f , 0.8116841f , 0.6667889f ,
            -0.467657f , 0.8116837f , 0.8116836f
        };

        static readonly float[] APOSE_MUSCLES =
        {
            -4.268867E-08f, 1.28066E-07f, 1.28066E-07f, -5.549529E-07f, 1.28066E-07f, -4.268868E-08f, 0f, 0f, 0f, 3.841981E-07f, -8.537734E-08f, -8.537736E-08f, 2.561321E-07f,
            -1.707547E-07f, -5.122641E-07f, 0f, 0f, 0f, 0f, 0f, 0f, 0.5970011f, 0.004342091f, 0.1613977f, 0.9840602f, -0.1569021f, -0.001237102f, -0.00803107f, 2.390566E-07f,
            0.5970009f, 0.00434206f, 0.1613968f, 0.9840602f, -0.1569014f, -0.001237238f, -0.008031525f, 2.390566E-07f, -0.169051f, -2.399202E-07f, 0.05232846f, 0.2786168f,
            0.1080032f, 0.9999952f, 0.003693818f, 0.02690749f, 1.443212E-05f, -0.1695286f, 9.827965E-07f, 0.05244817f, 0.278621f, 0.1079397f, 0.999998f, 0.003850663f,
            0.02686211f, 1.545362E-05f, -1.16825f, -0.08935567f, 0.5429555f, 0.6461703f, 0.562494f, -0.5145388f, 0.8116717f, 0.8116717f, 0.5101264f, -0.7967317f,
            0.8116744f, 0.8116746f, 0.485648f, -0.335764f, 0.8115476f, 0.8115493f, 0.5786369f, -0.3972031f, 0.6878238f, 0.8116769f, -1.168189f, -0.08936769f,
            0.542962f, 0.6461728f, 0.5624895f, -0.5145475f, 0.8116716f, 0.8116723f, 0.5101225f, -0.796772f, 0.8116748f, 0.8116751f, 0.4856391f, -0.3357929f,
            0.811549f, 0.8115487f, 0.5786426f, -0.3972253f, 0.6878179f, 0.8116763f
        };

        Type _transformType = typeof(Transform);
    }
}