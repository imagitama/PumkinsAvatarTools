#if UNITY_EDITOR
using System.Collections.Generic;
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Core;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;
using Pumkin.Core.Extensions;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Modules
{
    [AutoLoad(DefaultModuleIDs.COPIER)]
    class ComponentCopiersModule : UIModuleBase
    {
        public static event Delegates.SelectedChangeHandler OnAvatarSelectionChanged;

        public static IgnoreList IgnoreList { get; } = new IgnoreList(OnAvatarSelectionChanged);

        static bool CanCopy { get; set; } = true;

        public static GameObject CopyFromAvatar
        {
            get => _copyFromAvatar;

            set
            {
                if(_copyFromAvatar == value)
                    return;

                _copyFromAvatar = value;
                CopierAvatarSelectionChanged(_copyFromAvatar);
            }

        }

        GUIContent AvatarSelectorContent { get; set; } = new GUIContent("Copy from");

        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Component Copier", 1);

        static GameObject _copyFromAvatar;

        public static void CopierAvatarSelectionChanged(GameObject newSelection)
        {
            OnAvatarSelectionChanged?.Invoke(newSelection);
        }

        public override void DrawContent()
        {
            GUILayout.Space(16);

            if(!string.IsNullOrEmpty(UIDefs.Description))
                EditorGUILayout.HelpBox($"{UIDefs.Description}", MessageType.Info);

            CopyFromAvatar = EditorGUILayout.ObjectField(AvatarSelectorContent, CopyFromAvatar, typeof(GameObject), true) as GameObject;

            if(GUILayout.Button("Select from Scene"))
                CopyFromAvatar = Selection.activeGameObject ?? CopyFromAvatar;

#if PUMKIN_DEV
            if(GUILayout.Button("Auto Select"))
            {
                CopyFromAvatar = GameObject.Find("copyFrom") ?? GameObject.Find("Copy From") ?? GameObject.Find("from");
                PumkinTools.SelectedAvatar = GameObject.Find("copyTo") ?? GameObject.Find("Copy To") ?? GameObject.Find("to");
            }
#endif

            UIHelpers.DrawGUILine();

            EditorGUI.BeginDisabledGroup(!PumkinTools.SelectedAvatar || !CopyFromAvatar);
            {
                foreach(var copier in SubItems)
                {
                    UIHelpers.VerticalBox(() =>
                    {
                        copier?.DrawUI(GUILayout.MinHeight(Styles.CopierTabHeight));
                    }, Styles.CopierBox);
                }
            }
            EditorGUI.EndDisabledGroup();

            UIHelpers.DrawGUILine();

            IgnoreList?.DrawUI();

            UIHelpers.DrawGUILine();

            EditorGUI.BeginDisabledGroup(!CanCopy);
            {
                if(GUILayout.Button("Transfer Selected", Styles.CopierCopyButton))
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