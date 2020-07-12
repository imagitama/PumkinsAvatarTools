using Pumkin.AvatarTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools
{
    [AllowAutoLoad]
    class ZeroBlendshapes : SubToolBase
    {
        public ZeroBlendshapes()
        {
            Name = "Zero Blendshapes";
            Description = "Resets all Blendshapes on all SkinnedMeshRenderers to 0";
            CategoryName = "avatar";
        }

        public override bool Execute(GameObject target)
        {
            if(!Prepare(target))
                return false;

            foreach(var render in target.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                if(!render || !render.sharedMesh)
                    continue;
                
                for(int i = 0; i < render.sharedMesh.blendShapeCount; i++)                
                    render.SetBlendShapeWeight(i, 0);                
            }
            return true;
        }
    }
}
