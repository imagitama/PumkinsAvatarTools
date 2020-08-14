#if UNITY_EDITOR
using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Helpers;
using Pumkin.UnityTools.Implementation.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Tools.SubTools
{
    [AutoLoad("tools_lipsync", ParentModuleID = "tools_setupAvatar")]
    class SetupLipsync : SubToolBase
    {
        Type LipSyncStyleEnumType
        {
            get
            {
                return _lipSyncStyleEnumType ?? (_lipSyncStyleEnumType = TypeHelpers.GetType($"{VRCDescriptorType.FullName}+LipSyncStyle"));
            }
        }
        Type VRCDescriptorType
        {
            get => _vrcDescriptorType ?? (_vrcDescriptorType = TypeHelpers.GetType("VRC.SDKBase.VRC_AvatarDescriptor"));
        }
        List<string> RequiredVisemeNames
        {
            get
            {
                if(_requiredVisemeNames == null)
                {
                    var visemeNamesType = TypeHelpers.GetType($"{VRCDescriptorType.FullName}+Viseme");
                    if(visemeNamesType == null)
                        return null;

                    //Get required viseme names from the visemes enum and remove last entry called "Count"
                    _requiredVisemeNames = Enum.GetNames(visemeNamesType).ToList();
                    if(_requiredVisemeNames != null)
                        _requiredVisemeNames.RemoveAt(_requiredVisemeNames.Count - 1);
                }
                return _requiredVisemeNames;
            }
        }        

        List<string> _requiredVisemeNames;
        Type _vrcDescriptorType;

        //VRC.SDKBase.VRC_AvatarDescriptor+LipSyncStyle enum: { Default = 0, JawFlapBone = 1, JawFlapBlendShape = 2, VisemeBlendShape = 3 }
        Type _lipSyncStyleEnumType;

        public SetupLipsync()
        {
            Name = "Setup Lipsync";
            Description = "Sets up lipsync on your avatar.";
        }

        protected override bool DoAction(GameObject target)
        {
            var vrcDescType = Helpers.TypeHelpers.GetType("VRC.SDKBase.VRC_AvatarDescriptor");
            if(vrcDescType == null)
            {
                Debug.LogError("VRC.SDKBase.VRC_AvatarDescriptor not found in project");
                return false;
            }

            var descriptor = target.GetComponent(vrcDescType) ?? target.AddComponent(vrcDescType);
            var renders = target.GetComponentsInChildren<SkinnedMeshRenderer>();

            var serialDesc = new SerializedObject(descriptor);


            //Decide on a lipsync type
            var lipSyncType = serialDesc.FindProperty("lipSync");

            if(renders.Length > 0 && SetupVisemeBlendshapes(renders, serialDesc))   //Got required visemes so VisemeBlendShape style
            {
                lipSyncType.intValue = (int)Enum.Parse(LipSyncStyleEnumType, "VisemeBlendShape", true);
            }
            else
            {
                var anim = target.GetComponent<Animator>();
                Transform jaw = null;

                if(anim.isHuman)
                    jaw = anim.GetBoneTransform(HumanBodyBones.Jaw);

                if(jaw) //Got a humanoid jaw so use that
                {
                    lipSyncType.intValue = (int)Enum.Parse(LipSyncStyleEnumType, "JawFlapBone", true);
                    serialDesc.FindProperty("lipSyncJawBone").objectReferenceValue = jaw;
                }
                else //Should have been set by default but set to 'default' in case they change the enum
                {
                    lipSyncType.intValue = (int)(Enum.Parse(LipSyncStyleEnumType, "Default", true));
                }
            }   //Ignore JawFlapBlendShape because nobody uses it

            serialDesc.ApplyModifiedPropertiesWithoutUndo();
            return true;
        }

        bool SetupVisemeBlendshapes(SkinnedMeshRenderer[] renders, SerializedObject serialDesc)
        {
            var descVisemes = serialDesc.FindProperty("VisemeBlendShapes");
            var visemeMesh = serialDesc.FindProperty("VisemeSkinnedMesh");
            descVisemes.arraySize = RequiredVisemeNames.Count;

            for(int iRender = 0; iRender < renders.Length; iRender++)
            {
                bool foundShapeInRenderer = false;
                for(int iReqViseme = 0; iReqViseme < RequiredVisemeNames.Count; iReqViseme++)
                {
                    string shapeName = "-none-";
                    for(int iShape = 0; iShape < renders[iRender].sharedMesh.blendShapeCount; iShape++)
                    {
                        string tempShape = renders[iRender].sharedMesh.GetBlendShapeName(iShape);

                        if(tempShape.EndsWith(RequiredVisemeNames[iReqViseme], StringComparison.InvariantCultureIgnoreCase)
                            || tempShape.StartsWith(RequiredVisemeNames[iReqViseme], StringComparison.InvariantCultureIgnoreCase))
                        {
                            shapeName = tempShape;
                            foundShapeInRenderer = true;
                            break;
                        }
                    }
                    visemeMesh.objectReferenceValue = renders[iRender];
                    descVisemes.GetArrayElementAtIndex(iReqViseme).stringValue = shapeName;
                }
                if(foundShapeInRenderer)
                    return true;
            }
            return false;
        }
    }
}
#endif