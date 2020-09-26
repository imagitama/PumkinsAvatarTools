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
    [AutoLoad("tool_resetScale", ParentModuleID = "tools_fixAvatar")]
    [UIDefinition("Reset Scale", Description = "Resets your selected object's scale to prefab")]
    class ResetScale : SubToolBase
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

            target.transform.localScale = pref.transform.localScale;
            return true;
        }
    }
}
#endif