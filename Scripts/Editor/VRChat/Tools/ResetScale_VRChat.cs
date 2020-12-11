using Pumkin.AvatarTools.VRChat;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
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
