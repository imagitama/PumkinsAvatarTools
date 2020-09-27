#if UNITY_EDITOR
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using Pumkin.Core.Attributes;
using Pumkin.Core.Helpers;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Modules
{
    [AutoLoad(DefaultModuleIDs.COPIER)]
    [UIDefinition("Component Copier", OrderInUI = 1)]
    class ComponentCopiersModule : UIModuleBase
    {
        bool canCopy = true;
        public static GameObject CopyFromAvatar { get; set; }
        GUIContent AvatarSelectorContent { get; set; } = new GUIContent("Copy from");

        public override void DrawContent()
        {
            EditorGUILayout.Space();
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
            {
                foreach(var copier in SubItems)
                {
                    UIHelpers.VerticalBox(() =>
                    {
                        copier?.DrawUI();
                    });
                }
            }
            EditorGUI.EndDisabledGroup();

            UIHelpers.DrawGUILine();

            EditorGUI.BeginDisabledGroup(!canCopy);
            {
                if(GUILayout.Button("Copy Selected", Styles.MediumButton))
                {
                    foreach(var copier in SubItems)
                    {
                        var c = copier as IComponentCopier;
                        if(c.Active)
                            c?.TryCopyComponents(CopyFromAvatar, PumkinTools.SelectedAvatar);
                    }
                }
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();
        }
    }
}
#endif