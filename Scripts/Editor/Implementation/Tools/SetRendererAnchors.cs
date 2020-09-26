#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Implementation.Settings;
using Pumkin.AvatarTools.Implementation.Tools.SubTools.Settings;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools.SubTools
{
    [AutoLoad("tools_setRenderAnchors", ParentModuleID = DefaultModuleIDs.TOOLS_SETUP_AVATAR)]
    [UIDefinition("Set Renderer Anchors", Description = "Sets up anchors overrides on your renderers")]
    class SetRendererAnchors : SubToolBase
    {
        public override SettingsContainerBase Settings { get => settings; }

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

            if(settings.anchorType == SetRendererAnchors_Settings.AnchorType.HumanBone)
            {
                if(!anim.isHuman)
                    Debug.LogError($"{target.name} isn't humanoid");
                bone = anim.GetBoneTransform(settings.humanBone);
                path = $"{settings.humanBone.GetType()}.{settings.humanBone.ToString()}";
            }
            else
            {
                path = settings.bonePath;
                if(!string.IsNullOrEmpty(path))
                    bone = target.transform.Find(path);
            }

            if(!bone)
            {
                Debug.LogError($"Couldn't find bone at '{path}'");
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