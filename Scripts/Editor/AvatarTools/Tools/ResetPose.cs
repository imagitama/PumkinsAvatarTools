#if UNITY_EDITOR
using Pumkin.AvatarTools2.Interfaces;
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
            = new UIDefinition("Reset Pose", "Reverts the location, rotation and scale of your avatar back to avatar definition or prefab");

        public override ISettingsContainer Settings => settings;
        ResetPose_Settings settings;

        protected override void SetupSettings()
        {
            settings = ScriptableObject.CreateInstance<ResetPose_Settings>();
        }

        bool resetPosition, resetRotation, resetScale;
        bool? resetToPrefab;

        protected override bool Prepare(GameObject target)
        {
            if(!base.Prepare(target))
                return false;

            //Decide whether or not to prefab, or choose another type if invalid
            resetToPrefab = settings.resetType == ResetPose_Settings.ResetType.ToPrefab ? true : false;

            //Get true if resetToPrefab and false if not, null if neither
            DecideIfToPrefab(ref resetToPrefab, target);
            if(resetToPrefab == null)
                return false;

            resetPosition = settings.position;
            resetRotation = settings.rotation;
            resetScale = settings.scale;

            return true;
        }

        protected override bool DoAction(GameObject target)
        {
            Type transType = typeof(Transform);
            if(!PrefabHelpers.HasPrefab(target))
            {
                PumkinTools.LogWarning($"{target} is not a prefab instance.");
                return false;
            }

            if(resetToPrefab == true)
            {
                var overrides = PrefabUtility.GetObjectOverrides(target)
                    ?.Where(t => t.instanceObject.GetType() == transType && t.instanceObject);

                foreach(var o in overrides)
                {
                    if(resetPosition && resetRotation && resetScale)
                        o.Revert();
                    else
                    {
                        var changedTransform = o.instanceObject as Transform;
                        var originalTransform = o.GetAssetObject() as Transform;

                        if(resetPosition)
                            changedTransform.localPosition = originalTransform.localPosition;
                        if(resetRotation)
                            changedTransform.localRotation = originalTransform.localRotation;
                        if(resetScale)
                            changedTransform.localScale = originalTransform.localScale;
                    }
                }
            }
            else if(resetToPrefab == false)
            {
                ResetPoseToAvatar(target.GetComponent<Animator>(), resetPosition, resetRotation, resetScale);
            }
            else
            {
                PumkinTools.LogError("Can't reset to prefab or avatar");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Decides if can reset to prefab or to avatar, or neither
        /// </summary>
        /// <param name="toPrefabChoice">Choice to try first, returns it if it's possible and the other one if it's not, or null if neither are possible</param>
        /// <param name="target"></param>
        void DecideIfToPrefab(ref bool? toPrefabChoice, GameObject target)
        {
            //Check either if preference is invalid
            if((toPrefabChoice = Check(toPrefabChoice, target)) == null)
                toPrefabChoice = Check(!toPrefabChoice, target);

            //Allows us to check twice without repeating code
            bool? Check(bool? preference, GameObject checkTarget)
            {
                if(preference == true)
                {
                    if(PrefabHelpers.HasPrefab(checkTarget))
                        preference = true;
                }
                else if(preference == false)
                {
                    var anim = checkTarget.GetComponent<Animator>();
                    if(anim && anim.isHuman)
                        preference = false;
                }
                return preference;
            }
        }


        //Dreadrith's reset to avatar definition
        public static void ResetPoseToAvatar(Animator ani, bool position, bool rotation, bool scale)
        {
            if(!ani || !ani.avatar)
            {
                Debug.LogWarning("Humanoid avatar is required to reset pose!");
                return;
            }

            // Humans IDs if not full reset, otherwise All Ids
            // ID > Path
            // ID > Element > Transform Data

            SerializedObject sAvi = new SerializedObject(ani.avatar);
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
                Transform myBone = ani.transform.Find(IdToPath[allIds.GetArrayElementAtIndex(i).longValue]);
                TransformData data = elemToTransform[idToElem[allIds.GetArrayElementAtIndex(i).longValue]];
                applyTransform(myBone, data);
            }
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
    }
}
#endif