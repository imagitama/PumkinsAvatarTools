#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Implementation.Settings;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools.SubTools
{
    [AutoLoad("tools_setRenderAnchors", ParentModuleID = DefaultModuleIDs.TOOLS_SETUP_AVATAR)]
    [UIDefinition("Set Renderer Anchors", Description = "Sets up anchors overrides on your renderers")]    
    class SetRendererAnchors : SubToolBase
    {        
        public override SettingsContainer Settings { get => settings; }

        SetRendererAnchors_Settings settings;           

        protected override void SetupSettings()
        {
            settings = ScriptableObject.CreateInstance<SetRendererAnchors_Settings>();                        
        }

        protected override bool Prepare(GameObject target)
        {
            return (settings.setSkinnedMeshRenderers || settings.setMeshRenderers) &&
                base.Prepare(target);
        }

        protected override bool DoAction(GameObject target)
        {
            var anim = target.GetComponent<Animator>();
            Transform bone = null;
            string path = null;
            
            var renders = new List<Component>();
            if(settings.setMeshRenderers)
                renders.AddRange(target.GetComponentsInChildren<MeshRenderer>(true));
            if(settings.setSkinnedMeshRenderers)
                renders.AddRange(target.GetComponentsInChildren<SkinnedMeshRenderer>(true));            

            if(settings.anchorType == SubTools.SetRendererAnchors_Settings.AnchorType.HumanBone)
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

    internal class SetRendererAnchors_Settings : SettingsContainer
    {
        public enum AnchorType { HumanBone, TransformPath };

        public AnchorType anchorType = AnchorType.HumanBone;

        public HumanBodyBones humanBone = HumanBodyBones.Spine;

        public string bonePath = "Armature/Hips/Spine";

        public bool setSkinnedMeshRenderers = true;
        public bool setMeshRenderers = true;
    }

    [CustomEditor(typeof(SetRendererAnchors_Settings))]
    internal class SetRendererAnchors_SettingsEditor : SettingsEditor
    {
        public override void OnInspectorGUI()
        {
            var anchorType = serializedObject.FindProperty(nameof(SetRendererAnchors_Settings.anchorType));
            var humanBone = serializedObject.FindProperty(nameof(SetRendererAnchors_Settings.humanBone));
            var bonePath = serializedObject.FindProperty(nameof(SetRendererAnchors_Settings.bonePath));
            var setSkinnedMeshRenderers = serializedObject.FindProperty(nameof(SetRendererAnchors_Settings.setSkinnedMeshRenderers));
            var setMeshRenderers = serializedObject.FindProperty(nameof(SetRendererAnchors_Settings.setMeshRenderers));

            EditorGUILayout.PropertyField(anchorType);
            if(anchorType.enumValueIndex == (int)SetRendererAnchors_Settings.AnchorType.HumanBone)
                EditorGUILayout.PropertyField(humanBone);
            else
                EditorGUILayout.PropertyField(bonePath);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(setSkinnedMeshRenderers);
            EditorGUILayout.PropertyField(setMeshRenderers);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif