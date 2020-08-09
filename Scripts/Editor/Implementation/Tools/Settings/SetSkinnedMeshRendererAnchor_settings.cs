using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Implementation.Settings;
using Pumkin.UnityTools.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Tools.SubTools
{       
    public class SetSkinnedMeshRendererAnchor_settings : SettingsContainer
    {
        public enum AnchorType { HumanBone, TransformPath };
        
        [UIDefinition("Anchor Type")]
        public AnchorType anchorType = AnchorType.HumanBone;
        
        [UIDefinition("Humanoid Bone")]
        public HumanBodyBones humanBone = HumanBodyBones.Spine;
        
        [UIDefinition("Humanoid Bone Path")]
        public string bonePath = "Armature/Hips/Spine";
        
        public bool setSkinnedMeshRenderers = true;
        public bool setMeshRenderers = true;
    }

    [CustomEditor(typeof(SetSkinnedMeshRendererAnchor_settings))]
    public class SetSkinnedMeshRenderer_settingsEditor : Editor
    {
        SetSkinnedMeshRendererAnchor_settings settings;
        SerializedObject serialSettings;

        private void OnEnable()
        {
            settings = (SetSkinnedMeshRendererAnchor_settings)target;
            serialSettings = new SerializedObject(settings);
        }

        public override void OnInspectorGUI()
        {
            var anchorType = serialSettings.FindProperty(nameof(SetSkinnedMeshRendererAnchor_settings.anchorType));
            var humanBone = serialSettings.FindProperty(nameof(SetSkinnedMeshRendererAnchor_settings.humanBone));
            var bonePath = serialSettings.FindProperty(nameof(SetSkinnedMeshRendererAnchor_settings.bonePath));
            var setSkinnedMeshRenderers = serialSettings.FindProperty(nameof(SetSkinnedMeshRendererAnchor_settings.setSkinnedMeshRenderers));
            var setMeshRenderers = serialSettings.FindProperty(nameof(SetSkinnedMeshRendererAnchor_settings.setMeshRenderers));

            EditorGUILayout.PropertyField(anchorType);
            if(settings.anchorType == SetSkinnedMeshRendererAnchor_settings.AnchorType.HumanBone)
                EditorGUILayout.PropertyField(humanBone);
            else
                EditorGUILayout.PropertyField(bonePath);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(setSkinnedMeshRenderers);
            EditorGUILayout.PropertyField(setMeshRenderers);

            serialSettings.ApplyModifiedProperties();
        }
    }
}
