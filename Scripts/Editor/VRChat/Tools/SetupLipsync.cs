#if UNITY_EDITOR
using Pumkin.AvatarTools2.Tools;
using Pumkin.Core;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Tools
{
    [AutoLoad(DefaultIDs.Tools.SetupLipsync, "VRChat", ParentModuleID = DefaultIDs.Modules.Tools_SetupAvatar)]
    class SetupLipsync : ToolBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("[VRC] Setup Lipsync", "Sets up lipsync on your avatar.");

        Type lipsyncStyleEnumType = VRChatTypes.VRC_AvatarDescriptor_LipSyncStyle;

        Type vrcDescType = VRChatTypes.VRC_AvatarDescriptor;

        List<string> RequiredVisemeNames
        {
            get
            {
                if(_requiredVisemeNames == null)
                {
                    if(lipsyncStyleEnumType == null)
                        return null;

                    //Get required viseme names from the visemes enum and remove last entry called "Count"
                    _requiredVisemeNames = Enum.GetNames(lipsyncStyleEnumType).ToList();
                    if(_requiredVisemeNames != null)
                        _requiredVisemeNames.RemoveAt(_requiredVisemeNames.Count - 1);
                }
                return _requiredVisemeNames;
            }
        }

        List<string> _requiredVisemeNames;

        protected override bool DoAction(GameObject target)
        {
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
                lipSyncType.intValue = (int)Enum.Parse(lipsyncStyleEnumType, "VisemeBlendShape", true);
            }
            else
            {
                var anim = target.GetComponent<Animator>();
                Transform jaw = null;

                if(anim.isHuman)
                    jaw = anim.GetBoneTransform(HumanBodyBones.Jaw);

                if(jaw) //Got a humanoid jaw so use that
                {
                    lipSyncType.intValue = (int)Enum.Parse(lipsyncStyleEnumType, "JawFlapBone", true);
                    serialDesc.FindProperty("lipSyncJawBone").objectReferenceValue = jaw;
                }
                else //Should have been set by default but set to 'default' in case they change the enum
                {
                    lipSyncType.intValue = (int)(Enum.Parse(lipsyncStyleEnumType, "Default", true));
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