#if UNITY_EDITOR
using Pumkin.AvatarTools.Base;
using Pumkin.Core.Attributes;
using Pumkin.Core.Helpers;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Tools
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