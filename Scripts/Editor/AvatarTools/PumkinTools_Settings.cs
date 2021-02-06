using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Pumkin.AvatarTools2.Settings
{
    //No [CustomSettingsContainer] here because we create it manually
    class PumkinTools_Settings : SettingsContainerBase
    {
        public bool enableVerboseLogging = false;
    }

    [CustomEditor(typeof(PumkinTools_Settings))]
    class PumkinTools_SettingsEditor : SettingsEditor
    {
        public override void OnInspectorGUI()
        {
            var settings = target as PumkinTools_Settings;
            if(!settings)
                return;

            try
            {
                ConfigurationManager.CurrentConfigurationIndex = EditorGUILayout.Popup("Configuration", ConfigurationManager.CurrentConfigurationIndex, ConfigurationManager.Configurations);
            }
            catch(NullReferenceException ex)
            {
                UnityEngine.Debug.LogException(ex);
            }

            EditorGUILayout.HelpBox("Currently only SDK2 or SDK3 is supported, not both.\nIf yours is missing in the above configurations, change the define constraint on the Pumkin.AvatarTools2.VRChat.asmdef asset in your project tab", MessageType.Warning);

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            settings.enableVerboseLogging = EditorGUILayout.ToggleLeft("Enable Verbose Logging", settings.enableVerboseLogging);
            if(EditorGUI.EndChangeCheck() || refreshSettings)
                PumkinTools.VerboseLogger.logEnabled = settings.enableVerboseLogging;
        }
    }
}
