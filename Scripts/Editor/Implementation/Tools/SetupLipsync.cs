using Pumkin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Pumkin.AvatarTools.Helpers;

namespace Pumkin.AvatarTools.Implementation.Tools.SubTools
{
    class SetupLipsync : SubToolBase
    {
        public SetupLipsync() : base()
        {
            Name = "Setup Lipsync";
            Description = "Sets up lipsync on your avatar";
            CategoryName = "Avatar";
        }   

        public override bool Execute(GameObject avatar)
        {
            if(!avatar)
            {
                //Debug.LogError("No avatar selected");
                AvatarTools.Log("No avatar selected", avatar);
                return false;
            }            

            var descriptor = avatar.GetOrAddComponent<VRCSDK2.VRC_AvatarDescriptor>();

            //Get required viseme names from the visemes enum and remove last entry called "Count"
            var requiredVisemes = Enum.GetNames(typeof(VRCSDK2.VRC_AvatarDescriptor.Viseme)).ToList();
            requiredVisemes.RemoveAt(requiredVisemes.Count - 1);

            if(descriptor.VisemeBlendShapes == null || descriptor.VisemeBlendShapes.Length != requiredVisemes.Count)            
                descriptor.VisemeBlendShapes = new string[requiredVisemes.Count];            

            var renders = avatar.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            bool foundShape = false;            

            //Look for a renderer with one matching viseme and asign it as the face mesh renderer 
            //After that search through all visemes and assign the matching ones
            for(int i = 0; i < renders.Length; i++)
            {
                for(int j = 0; j < requiredVisemes.Count; j++)
                {
                    string shapeName = "-none-";
                    for(int k = 0; k < renders[i].sharedMesh.blendShapeCount; k++)
                    {                        
                        string tempShape = renders[i].sharedMesh.GetBlendShapeName(k);

                        if(tempShape.EndsWith(requiredVisemes[j], StringComparison.InvariantCultureIgnoreCase) 
                            || tempShape.StartsWith(requiredVisemes[j], StringComparison.InvariantCultureIgnoreCase))
                        {
                            shapeName = tempShape;
                            foundShape = true;
                            break;
                        }
                    }
                    descriptor.VisemeBlendShapes[j] = shapeName;

                    if(foundShape)
                        descriptor.VisemeSkinnedMesh = renders[i];
                }
            }

            //Decided what kind of lip sync type to set depending on what we found
            if(descriptor.VisemeSkinnedMesh == null)
            {                
                Debug.LogError("No SkinnedMesh found");
                return false;
            }
            else
            {                                
                if(foundShape)
                {
                    descriptor.lipSync = VRCSDK2.VRC_AvatarDescriptor.LipSyncStyle.VisemeBlendShape;                    
                }
                else
                {
                    var anim = avatar.GetComponent<Animator>();
                    if(anim && anim.isHuman)
                    {
                        var jaw = anim.GetBoneTransform(HumanBodyBones.Jaw);
                        if(jaw)
                        {
                            descriptor.lipSync = VRCSDK2.VRC_AvatarDescriptor.LipSyncStyle.JawFlapBone;
                            descriptor.lipSyncJawBone = jaw;
                        }
                        else
                        {                            
                            descriptor.lipSync = VRCSDK2.VRC_AvatarDescriptor.LipSyncStyle.Default;
                        }                        
                    }
                    else
                    {                        
                        descriptor.lipSync = VRCSDK2.VRC_AvatarDescriptor.LipSyncStyle.Default;                        
                    }                    
                    Debug.LogError("Mesh has no Blendshapes");
                    return false;
                }
            }
            return true;
        }
    }
}
