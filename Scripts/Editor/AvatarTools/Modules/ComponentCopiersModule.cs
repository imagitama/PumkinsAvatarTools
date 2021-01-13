#if UNITY_EDITOR
using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Modules
{
    [AutoLoad(DefaultIDs.Modules.Copier)]
    class ComponentCopiersModule : UIModuleBase
    {
        const string COPY_BUTTON_STRING = "Copy components to {0}";

        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Component Copier", 1);

        public static event Delegates.SelectedChangeHandler OnCopierAvatarSelectionChanged;

        public static IgnoreList IgnoreList { get; } = new IgnoreList(OnCopierAvatarSelectionChanged);

        static bool CanCopy { get; set; } = true;

        GUIContent copyButtonContent;

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

        public ComponentCopiersModule()
        {
            copyButtonContent = new GUIContent();
            PumkinTools_OnAvatarSelectionChanged(null); //So label gets assigned

            PumkinTools.OnAvatarSelectionChanged -= PumkinTools_OnAvatarSelectionChanged;
            PumkinTools.OnAvatarSelectionChanged += PumkinTools_OnAvatarSelectionChanged;
        }

        private void PumkinTools_OnAvatarSelectionChanged(GameObject newSelection)
        {
            copyButtonContent.text = string.Format(COPY_BUTTON_STRING, newSelection?.name ?? "selected");
        }

        public static void CopierAvatarSelectionChanged(GameObject newSelection)
        {
            OnCopierAvatarSelectionChanged?.Invoke(newSelection);
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

            UIHelpers.DrawLine();

            EditorGUI.BeginDisabledGroup(!PumkinTools.SelectedAvatar || !CopyFromAvatar);
            {
                foreach(var copier in SubItems)
                {
                    UIHelpers.DrawInVerticalBox(() =>
                    {
                        copier?.DrawUI(GUILayout.MinHeight(Styles.CopierTabHeight));
                    }, Styles.CopierBox);
                }
            }
            EditorGUI.EndDisabledGroup();

            UIHelpers.DrawLine();

            IgnoreList?.DrawUI();

            UIHelpers.DrawLine();

            EditorGUI.BeginDisabledGroup(!CanCopy);
            {
                if(GUILayout.Button(copyButtonContent, Styles.CopierCopyButton))
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


        static GameObject _copyFromAvatar;
    }
}
#endif