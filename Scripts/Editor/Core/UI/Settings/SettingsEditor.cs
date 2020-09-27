#if UNITY_EDITOR
using UnityEditor;

namespace Pumkin.AvatarTools.UI.Editors
{
    /// <summary>
    /// Base editor that should be inherited when creating an editor for settings
    /// Draws default editor but without the script field by default
    /// </summary>
    public abstract class SettingsEditor : Editor
    {
        protected bool hideScriptField = true;

        public override void OnInspectorGUI()
        {
            if(hideScriptField)
            {
                serializedObject.Update();
                EditorGUI.BeginChangeCheck();
                DrawPropertiesExcluding(serializedObject, "m_Script");
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