using Pumkin.AvatarTools2.VRChat;
using Pumkin.AvatarTools2.Tools;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Tools
{
    [AutoLoad(DefaultIDs.Tools.ResetScale, "VRChat", ParentModuleID = DefaultIDs.Modules.Tools_FixAvatar)]
    class ResetScale : ToolBase
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

            VRChatHelpers.SetAvatarScale(target, pref.transform.localScale, true, out _);
            return true;
        }
    }
}
