using Pumkin.AvatarTools2.Settings;
using Pumkin.Core;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    [CustomSettingsContainer(typeof(ResetPose))]
    class ResetPose_Settings : SettingsContainerBase
    {
        public ResetType resetType = ResetType.ToAvatarDefinition;

        public bool position = false;
        public bool rotation = true;
        public bool scale = false;

        public enum ResetType { ToAvatarDefinition, ToPrefab, ToTPose, ToAPose };
    }

    [CustomEditor(typeof(ResetPose_Settings))]
    class ResetPose_SettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var settings = target as ResetPose_Settings;
            settings.resetType = (ResetPose_Settings.ResetType)EditorGUILayout.EnumPopup("Reset type", settings.resetType);

            EditorGUILayout.Space();

            bool disablePosRotSc =
                settings.resetType == ResetPose_Settings.ResetType.ToTPose ||
                settings.resetType == ResetPose_Settings.ResetType.ToAPose;

            EditorGUI.BeginDisabledGroup(disablePosRotSc);

            settings.position = EditorGUILayout.Toggle("Position", settings.position);
            settings.rotation = EditorGUILayout.Toggle("Rotation", settings.rotation);
            settings.scale = EditorGUILayout.Toggle("Scale", settings.scale);

            EditorGUI.EndDisabledGroup();
        }
    }
}