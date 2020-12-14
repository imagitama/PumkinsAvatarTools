#if UNITY_EDITOR
using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    [AutoLoad(DefaultIDs.Tools.RevertTransforms, ParentModuleID = DefaultIDs.Modules.Tools_FixAvatar)]
    class RevertTransforms : ToolBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Reset Pose", "Reverts the location, rotation and scale of your avatar back to prefab");
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