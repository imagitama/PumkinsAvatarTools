#if UNITY_EDITOR
using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Helpers;
using Pumkin.UnityTools.Interfaces;
using Pumkin.UnityTools.UI;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Modules
{
    [AutoLoad("copiers")]
    [UIDefinition("Component Copiers", OrderInUI = 2)]
    class ComponentCopiersModule : UIModuleBase
    {
        public static GameObject CopyFromAvatar { get; set; }
        GUIContent AvatarSelectorContent { get; set; } = new GUIContent("Copy from");

        public override void DrawContent()
        {
            EditorGUILayout.Space();

            if(!string.IsNullOrEmpty(Description))
                EditorGUILayout.HelpBox($"{Description}", MessageType.Info);

            CopyFromAvatar = EditorGUILayout.ObjectField(AvatarSelectorContent, CopyFromAvatar, typeof(GameObject), true) as GameObject;

            if(GUILayout.Button("Select from Scene"))
                CopyFromAvatar = Selection.activeGameObject ?? CopyFromAvatar;

#if PUMKIN_DEV
            if(GUILayout.Button("Autoselect"))
            {
                CopyFromAvatar = GameObject.Find("copyFrom");
                PumkinTools.SelectedAvatar = GameObject.Find("copyTo");
            }
#endif


            UIHelpers.DrawGUILine();            

            EditorGUI.BeginDisabledGroup(!PumkinTools.SelectedAvatar || !CopyFromAvatar);
            foreach(var copier in SubItems)            
                copier?.DrawUI();
            EditorGUI.EndDisabledGroup();
        }
    }
}
#endif