#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Helpers;
using Pumkin.AvatarTools.Implementation.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools.SubTools
{
    [AutoLoad("tools_reverTransforms", ParentModuleID = "tools_fixAvatar")]
    [UIDefinition("Reset Pose", Description = "Reverts the location, rotation and scale of your avatar back to prefab")]
    class RevertTransforms : SubToolBase
    {
        protected override bool Prepare(GameObject target)
        {
            if(!base.Prepare(target))
                return false;
            return PrefabHelpers.HasPrefab(target);
        }

        protected override bool DoAction(GameObject target)
        {
            Type transType = typeof(Transform);
            var overrides = PrefabUtility.GetObjectOverrides(target)
                ?.Where(t => t.instanceObject.GetType() == transType && t.instanceObject);

            foreach(var o in overrides)
                o.Revert();

            return true;
        }
    }
}
#endif