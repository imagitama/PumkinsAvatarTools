#if UNITY_EDITOR
using UnityEditor;

namespace Pumkin.AvatarTools.Implementation.Tools.SubTools.Settings
{
    [CustomEditor(typeof(SetRendererAnchors_Settings))]
    internal class SetRendererAnchors_SettingsEditor : SettingsEditor
    {
        public override void OnInspectorGUI()
        {
            var anchorType = serializedObject.FindProperty(nameof(SetRendererAnchors_Settings.anchorType));
            var humanBone = serializedObject.FindProperty(nameof(SetRendererAnchors_Settings.humanBone));
            var bonePath = serializedObject.FindProperty(nameof(SetRendererAnchors_Settings.bonePath));
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