using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Implementation.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools.SubTools
{    
    class RandomPose : SubToolBase
    {
        public RandomPose()
        {
            Name = "Random Pose";
            Description = "Set a random pose for testing";
            ParentModuleID = "main_tools";
        }

        public override bool DoAction(GameObject target)
        {
            var rng = new System.Random();
            int range = 5;

            Vector3 vec = new Vector3(rng.Next(-range, range), rng.Next(-range, range), rng.Next(-range, range));

            foreach(var t in target.GetComponentsInChildren<Transform>(true))
            {
                if(t == t.root || t.root.Find(t.name)) //Skip root and first children
                    continue;
                t.localRotation = Quaternion.Euler(vec);
            }

            return true;
        }
    }
}
