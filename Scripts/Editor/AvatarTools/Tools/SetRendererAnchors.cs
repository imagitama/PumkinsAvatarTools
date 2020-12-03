#if UNITY_EDITOR
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.Modules;
using Pumkin.AvatarTools.Settings;
using Pumkin.Core;
using Pumkin.Core.UI;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Tools
{
    [AutoLoad("tools_setRenderAnchors", ParentModuleID = DefaultModuleIDs.TOOLS_SETUP_AVATAR)]
    class SetRendererAnchors : ToolBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Set Renderer Anchors", "Sets up anchors overrides on your renderers");

        public override ISettingsContainer Settings { get => settings; }

        SetRendererAnchors_Settings settings;

        protected override void SetupSettings()
        {
            settings = ScriptableObject.CreateInstance<SetRendererAnchors_Settings>();
        }

        protected override bool Prepare(GameObject target)
        {
            return (settings.skinnedMeshRenderers || settings.meshRenderers) &&
                base.Prepare(target);
        }

        protected override bool DoAction(GameObject target)
        {
            var anim = target.GetComponent<Animator>();
            Transform bone = null;
            string path = null;

            var renders = new List<Component>();
            if(settings.meshRenderers)
                renders.AddRange(target.GetComponentsInChildren<MeshRenderer>(true));
            if(settings.skinnedMeshRenderers)
                renders.AddRange(target.GetComponentsInChildren<SkinnedMeshRenderer>(true));

            if(settings.anchorTransformType == SetRendererAnchors_Settings.AnchorType.HumanBone)
            {
                if(!anim.isHuman)
                    Debug.LogError($"{target.name} isn't humanoid");
                bone = anim.GetBoneTransform(settings.humanBone);
                path = $"{settings.humanBone.GetType().Name}.{settings.humanBone}";
            }
            else
            {
                path = settings.hierarchyPath;
                if(!string.IsNullOrEmpty(path))
                    bone = target.transform.Find(path);
            }

            if(!bone)
            {
                PumkinTools.LogError($"Couldn't find '{path}'");
                return false;
            }

            var so = new SerializedObject(renders.ToArray());
            var anchors = so.FindProperty("m_ProbeAnchor");
            if(anchors == null)
                return false;

            anchors.objectReferenceValue = bone;
            so.ApplyModifiedPropertiesWithoutUndo();
            return true;
        }
    }
}
#endif