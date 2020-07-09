using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools
{
    class RevertBlendshapes : SubToolBase
    {
        public RevertBlendshapes()
        {
            Name = "Revert Blendshapes";
            Description = "Reverts blendshapes on your avatar to prefab";
            CategoryName = "avatar";
        }

        public override bool Execute(GameObject avatar)
        {
            Debug.Log("Faking Reverting blendshapes");
            return true;
        }
    }
}
