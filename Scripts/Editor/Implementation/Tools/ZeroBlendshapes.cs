using Pumkin.UnityTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Tools
{
    [AutoLoad("tools_zeroblendshapes")]
    class ZeroBlendshapes : SubToolBase
    {
        public ZeroBlendshapes()
        {
            Name = "Zero Blendshapes";
            Description = "Resets all Blendshapes on all SkinnedMeshRenderers to 0";            
        }

        public override bool DoAction(GameObject target)
        {
            var renders = target.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            var so = new SerializedObject(renders);
            var weights = so.FindProperty("m_BlendShapeWeights");
            
            for(int i = 0; i < weights.arraySize; i++)
                weights.GetArrayElementAtIndex(i).floatValue = 0;
            
            so.ApplyModifiedPropertiesWithoutUndo();
            return true;
        }
    }
}
