using Pumkin.UnityTools.Attributes;
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
        public SetSkinnedMeshRendererAnchors()
        {
            Name = "Set Renderer anchors to Spine";            
        }

        public override bool DoAction(GameObject target)
        {            
            var renders = target.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            var anim = target.GetComponent<Animator>();
            bool isHuman = anim?.isHuman ?? false;            
            var humanBone = HumanBodyBones.Spine;

            if(isHuman)
            {
                var bone = anim.GetBoneTransform(humanBone);
                if(!bone)
                    return false;
                
                var so = new SerializedObject(renders);
                var anchors = so.FindProperty("m_ProbeAnchor");
                if(anchors == null)
                    return false;
                
                anchors.objectReferenceValue = bone;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
            
            return true;
        }
    }
}
