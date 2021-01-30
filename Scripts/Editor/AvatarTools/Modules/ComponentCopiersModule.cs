using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.Extensions;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Modules
{
    [AutoLoad(DefaultIDs.Modules.Copier)]
    class ComponentCopiersModule : UIModuleBase, ISerializationCallbackReceiver
    {
        const string COPY_BUTTON_STRING = "Copy components to {0}";
        const string SAVE_FOLDER = "Configs/";

        static string LocalSavePath
        {
            get
            {
               if(string.IsNullOrWhiteSpace(_savePath))
                    _savePath = $"{SAVE_FOLDER}{ConfigurationManager.CurrentConfigurationString}/{typeof(ComponentCopiersModule).Name}.json";
                return _savePath;
            }
        }

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

            SettingsManager.SaveSettingsCallback -= SettingsManager_SaveSettingsCallback;
            SettingsManager.SaveSettingsCallback += SettingsManager_SaveSettingsCallback;

            SettingsManager.LoadSettingsCallback -= SettingsManager_LoadSettingsCallback;
            SettingsManager.LoadSettingsCallback += SettingsManager_LoadSettingsCallback;
        }

        private void SettingsManager_LoadSettingsCallback()
        {
            SettingsManager.LoadFromJson(LocalSavePath, this);
        }

        private void SettingsManager_SaveSettingsCallback()
        {
            SettingsManager.SaveToJson(this, LocalSavePath);
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
                        if(c?.Active == true)
                            c.TryCopyComponents(CopyFromAvatar, PumkinTools.SelectedAvatar);
                    }
                }
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();
        }


        private class _serial
        {
            public string[] enabled;

            public _serial(string[] enabled)
            {
                this.enabled = enabled;
            }
        }

        public void OnBeforeSerialize()
        {
            _enabledIDs = IDManager.Items
                .Where(kv => kv.Value is IComponentCopier)
                .Where(kv => (kv.Value as IComponentCopier).Active)
                .Select(kv => kv.Key)
                .ToArray();
        }

        public void OnAfterDeserialize()
        {
            foreach(var id in _enabledIDs)
            {
                if(IDManager.GetItem(id) is IComponentCopier cop)
                    cop.Active = true;
            }
        }


        static GameObject _copyFromAvatar;
        static string _savePath;
        [SerializeField] string[] _enabledIDs;
    }
}