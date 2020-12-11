#if UNITY_EDITOR
using Pumkin.AvatarTools2.Settings;
using Pumkin.Core.Helpers;
using UnityEditor;

namespace Pumkin.AvatarTools2.UI.Editors
{
    [CustomEditor(typeof(SetRendererAnchors_Settings))]
    internal class SetRendererAnchors_SettingsEditor : SettingsEditor
    {
        public override void OnInspectorGUI()
        {
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
#endif