using Pumkin.AvatarTools2.Settings;
using Pumkin.Core.Extensions;
using System;
using UnityEditor;

namespace Pumkin.AvatarTools2.UI
{
    /// <summary>
    /// Base editor that should be inherited when creating an editor for settings
    /// Draws default editor but without the script field by default
    /// </summary>
    [CustomEditor(typeof(SettingsContainerBase), true)]
    public class SettingsEditor : Editor
    {
        protected bool hideScriptField = true;
        protected bool refreshSettings = false;

        private void OnEnable()
        {
            refreshSettings = true;
            try
            {
                OnInspectorGUI();
            }
            catch { }
            finally
            {
                refreshSettings = false;
            }
        }

        public override void OnInspectorGUI()
        {
            if(hideScriptField)
            {
                serializedObject.Update();
                EditorGUI.BeginChangeCheck();
                {
                    try
                    {
                        DrawPropertiesExcluding(serializedObject, "m_Script");
                    }
                    catch { }
                    //{
                    //    UnityEngine.Debug.Log("WHY!?");
                    //}
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