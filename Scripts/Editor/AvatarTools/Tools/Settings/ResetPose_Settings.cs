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

        [Space]
        [DrawToggleLeft] public bool position = false;
        [DrawToggleLeft] public bool rotation = true;
        [DrawToggleLeft] public bool scale = false;

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

            settings.position = EditorGUILayout.ToggleLeft("Position", settings.position);
            settings.rotation = EditorGUILayout.ToggleLeft("Rotation", settings.rotation);
            settings.scale = EditorGUILayout.ToggleLeft("Scale", settings.scale);

            EditorGUI.EndDisabledGroup();
        }
    }
}