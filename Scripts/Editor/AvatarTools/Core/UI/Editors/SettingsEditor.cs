#if UNITY_EDITOR
using Pumkin.AvatarTools2.Settings;
using UnityEditor;

namespace Pumkin.AvatarTools2.UI
{
    /// <summary>
    /// Base editor that should be inherited when creating an editor for settings
    /// Draws default editor but without the script field by default
    /// </summary>
    [CustomEditor(typeof(SettingsContainerBase))]
    public class SettingsEditor : Editor
    {
        protected bool hideScriptField = true;

        public override void OnInspectorGUI()
        {
            if(hideScriptField)
            {
                serializedObject.Update();
                EditorGUI.BeginChangeCheck();
                {
                    DrawPropertiesExcluding(serializedObject, "m_Script");
                }
                if(EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
            }
            else
            {
                base.OnInspectorGUI();
            }
        }
    }
}
#endif