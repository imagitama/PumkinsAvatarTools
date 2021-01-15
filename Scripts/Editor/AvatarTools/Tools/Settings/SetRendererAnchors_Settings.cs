using Pumkin.AvatarTools2.Settings;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    [CustomSettingsContainer(typeof(SetRendererAnchors))]
    internal class SetRendererAnchors_Settings : SettingsContainerBase
    {
        public enum AnchorType { HumanBone, TransformPath };

        public AnchorType anchorTransformType = AnchorType.HumanBone;

        public HumanBodyBones humanBone = HumanBodyBones.Spine;

        public string hierarchyPath = "Armature/Hips/Spine";

        public bool skinnedMeshRenderers = true;
        public bool meshRenderers = true;
    }

    [CustomEditor(typeof(SetRendererAnchors_Settings))]
    internal class SetRendererAnchors_SettingsEditor : SettingsEditor
    {
        public override void OnInspectorGUI()
        {
            if(target == null)
                return;

            var anchorType = serializedObject.FindProperty(nameof(SetRendererAnchors_Settings.anchorTransformType));
            var humanBone = serializedObject.FindProperty(nameof(SetRendererAnchors_Settings.humanBone));
            var bonePath = serializedObject.FindProperty(nameof(SetRendererAnchors_Settings.hierarchyPath));
            var skinnedMeshRenderers = serializedObject.FindProperty(nameof(SetRendererAnchors_Settings.skinnedMeshRenderers));
            var meshRenderers = serializedObject.FindProperty(nameof(SetRendererAnchors_Settings.meshRenderers));


            EditorGUILayout.PropertyField(anchorType);
            if(anchorType.enumValueIndex == (int)SetRendererAnchors_Settings.AnchorType.HumanBone)
                EditorGUILayout.PropertyField(humanBone);
            else
                EditorGUILayout.PropertyField(bonePath);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(skinnedMeshRenderers);
            EditorGUILayout.PropertyField(meshRenderers);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
