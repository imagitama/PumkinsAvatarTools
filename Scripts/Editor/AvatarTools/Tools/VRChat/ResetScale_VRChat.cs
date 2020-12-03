using Pumkin.AvatarTools.Base;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Tools
{
    [AutoLoad("tool_resetScale", "vrchat", ParentModuleID = "tools_fixAvatar")]
    class ResetScale_VRChat : ToolBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("[VRC] Reset Scale", "Resets your selected object's scale to prefab and moves the viewpoint");

        protected override bool Prepare(GameObject target)
        {
            return base.Prepare(target) && PrefabHelpers.HasPrefab(target);
        }

        protected override bool DoAction(GameObject target)
        {
            var pref = PrefabUtility.GetCorrespondingObjectFromSource(target);
            if(!pref)
                return false;

            VRChatHelpers.SetAvatarScale(target, pref.transform.localScale, true);
            return true;
        }
    }
}
