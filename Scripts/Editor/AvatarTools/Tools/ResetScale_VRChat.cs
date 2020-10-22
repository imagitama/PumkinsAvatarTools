using Pumkin.AvatarTools.Base;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Tools
{
    [AutoLoad("tool_resetScale", "vrchat", ParentModuleID = "tools_fixAvatar", CanReplaceGenericItem = true)]
    [UIDefinition("Reset Scale", Description = "Resets your selected object's scale to prefab and moves the viewpoint")]
    class ResetScale_VRChat : ToolBase
    {
        protected override bool Prepare(GameObject target)
        {
            if(!base.Prepare(target))
                return false;
            return PrefabHelpers.HasPrefab(target);
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
