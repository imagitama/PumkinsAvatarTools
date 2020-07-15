using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools.SubTools
{
    [AutoLoad]
    class SetupLipsync_prop : SubToolBase
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
        Type _lipSyncStyleEnumType;

        public SetupLipsync_prop()pro
        {
            Name = "Setup Lipsync Properties";
            Description = "Sets up lipsync on your avatar. Doesn't need the VRChat SDK to be in the project to compile";
            ParentModuleID = "main_tools";
        }

        public override bool DoAction(GameObject target)
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

            if(renders.Length > 0 && SetupVisemeBlendshapes(renders, serialDesc))
            {
                //VRC.SDKBase.VRC_AvatarDescriptor.LipSyncStyle = VRC.SDKBase.VRC_AvatarDescriptor.LipSyncStyle.VisemeBlendShape;                    
                var num = Enum.Parse(LipSyncStyleEnumType, "VisemeBlendShape", true);
                lipSyncType.intValue = (int)num;
            }
            else //Jaw flap bone
            {
                //TODO: Add this
            }
            
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
