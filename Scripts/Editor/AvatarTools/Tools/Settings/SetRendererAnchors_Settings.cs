#if UNITY_EDITOR
using Pumkin.AvatarTools.Base;
using Pumkin.Core.Attributes;
using UnityEngine;

namespace Pumkin.AvatarTools.Settings
{
    internal class SetRendererAnchors_Settings : SettingsContainerBase
    {
        public enum AnchorType { HumanBone, TransformPath };

        public AnchorType anchorTransformType = AnchorType.HumanBone;

        public HumanBodyBones humanBone = HumanBodyBones.Spine;

        public string hierarchyPath = "Armature/Hips/Spine";

        [DrawToggleLeft]
        public bool skinnedMeshRenderers = true;
        [DrawToggleLeft]
        public bool meshRenderers = true;
    }
}
#endif