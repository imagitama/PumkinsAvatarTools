#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Implementation.Settings;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools.SubTools.Settings
{
    internal class SetRendererAnchors_Settings : SettingsContainerBase
    {
        public enum AnchorType { HumanBone, TransformPath };

        public AnchorType anchorType = AnchorType.HumanBone;

        public HumanBodyBones humanBone = HumanBodyBones.Spine;

        public string bonePath = "Armature/Hips/Spine";

        [DrawToggleLeft]
        public bool skinnedMeshRenderers = true;
        [DrawToggleLeft]
        public bool meshRenderers = true;
    }
}
#endif