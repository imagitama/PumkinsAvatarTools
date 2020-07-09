using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools.SubTools
{
    class ResetPose : SubToolBase
    {
        public ResetPose()
        {
            Name = "Reset Pose";
            Description = "Resets the pose of your avatar";
            CategoryName = "pose stuff";
        }

        public override bool Execute(GameObject avatar)
        {
            Debug.Log("Gottem");
            return true;
        }
    }
}
