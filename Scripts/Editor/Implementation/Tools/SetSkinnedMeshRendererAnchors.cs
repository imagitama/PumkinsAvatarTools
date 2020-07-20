using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Implementation.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Tools.SubTools
{
    [AutoLoad("tools_setRenderAnchors", "tools")]
    class SetSkinnedMeshRendererAnchors : SubToolBase
    {
        enum AnchorType { HumanBone, TransformPath };
        
        AnchorType anchorType = AnchorType.HumanBone;
        HumanBodyBones humanBone = HumanBodyBones.Spine;
        string bonePath = "Armature/Hips/Spine";
        
        public SetSkinnedMeshRendererAnchors()
        {
            Name = "Set Renderer anchors to {0}";
            Settings.RegisterSetting(new Setting("anchorType", anchorType, "Anchor Type"));
        }

        protected override bool DoAction(GameObject target)
        {
            var renders = target.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            var anim = target.GetComponent<Animator>();

            Transform bone = null;
            string path = null;
            anchorType = Settings.GetSettingValue<AnchorType>("anchorType");

            if(anchorType == AnchorType.HumanBone)
            {
                if(!anim.isHuman)
                    Debug.LogError($"{target.name} isn't humanoid");
                bone = anim.GetBoneTransform(humanBone);
                path = $"{humanBone.GetType()}.{humanBone.ToString()}";
            }
            else
            {
                path = bonePath;
                if(!string.IsNullOrEmpty(path))
                    bone = target.transform.Find(path);
            }

            if(!bone)
            {
                Debug.LogError($"Couldn't find bone at '{path}'");
                return false;
            }                        
                
            var so = new SerializedObject(renders);
            var anchors = so.FindProperty("m_ProbeAnchor");
            if(anchors == null)
                return false;
                
            anchors.objectReferenceValue = bone;
            so.ApplyModifiedPropertiesWithoutUndo();            
            return true;
        }
    }
}
