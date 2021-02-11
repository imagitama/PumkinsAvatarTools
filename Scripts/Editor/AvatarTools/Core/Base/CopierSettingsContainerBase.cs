using Pumkin.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Settings
{
    [Serializable]
    public abstract class CopierSettingsContainerBase : SettingsContainerBase
    {
        protected virtual bool ShowRemoveAll { get; } = true;
        protected virtual bool ShowCreateGameObjects { get; } = true;
        protected virtual bool ShowOnlyAllowOne { get; } = false;

        [ConditionalHide(nameof(ShowRemoveAll))]
        public bool removeAllBeforeCopying = false;

        [ConditionalHide(nameof(ShowCreateGameObjects))]
        public bool createGameObjects = false;

        [ConditionalHide(nameof(ShowOnlyAllowOne))]
        public bool onlyAllowOneComponentOfType = false;

        public virtual PropertyDefinitions Properties { get; } = null;

        // This needs to be here to put a space before the settings of the object that inherits us
        [ConditionalHide(nameof(ShowCreateGameObjects), nameof(ShowRemoveAll))]
        [SerializeField]UISpacer _spacer;

        public override void DrawUI()
        {
            base.DrawUI();
            if(Properties != null)
                DrawPropertyGroups();
        }

        void DrawPropertyGroups()
        {
            int index = 0;
            int count = Properties.TypeProperties.Count;
            foreach(var type_propGroup in Properties.TypeProperties)
            {
                foreach(var group in type_propGroup.Value)
                {
                    EditorGUI.BeginChangeCheck();
                    bool enabled = EditorGUILayout.ToggleLeft(group.Name, group.Enabled);
                    if(EditorGUI.EndChangeCheck())
                    {
                        group.Enabled = enabled;
                    }
                }

                if(index < count)
                    EditorGUILayout.Space();
                index++;
            }
        }
    }
}