using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Implementation.Settings;
using Pumkin.UnityTools.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Tools.SubTools
{       
    public class SetSkinnedMeshRendererAnchor_settings : SettingsContainer
    {
        public enum AnchorType { HumanBone, TransformPath };
        
        [UIDefinition("Anchor Type")]
        public AnchorType anchorType = AnchorType.HumanBone;
        
        [UIDefinition("Humanoid Bone")]
        public HumanBodyBones humanBone = HumanBodyBones.Spine;
        
        [UIDefinition("Humanoid Bone Path")]
        public string bonePath = "Armature/Hips/Spine";
    }
}
