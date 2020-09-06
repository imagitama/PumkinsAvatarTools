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
        public static GameObject CopyToAvatar { get; set; }
        GUIContent AvatarSelectorContent { get; set; } = new GUIContent("Copy to");

        public override void DrawContent()
        {
            EditorGUILayout.Space();

            if(!string.IsNullOrEmpty(Description))
                EditorGUILayout.HelpBox($"{Description}", MessageType.Info);

            CopyToAvatar = EditorGUILayout.ObjectField(AvatarSelectorContent, CopyToAvatar, typeof(GameObject), true) as GameObject;

            if(GUILayout.Button("Select from Scene"))
                CopyToAvatar = Selection.activeGameObject ?? CopyToAvatar;
            
            UIHelpers.DrawGUILine();
        }
    }
}
#endif