#if UNITY_EDITOR
using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.Modules;
using Pumkin.AvatarTools2.Settings;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.Extensions;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Destroyers
{
    /// <summary>
    /// Base class that destroys all components of type by it's fullname
    /// </summary>
    public abstract class ComponentDestroyerBase : IComponentDestroyer, IItem
    {
        public abstract string[] ComponentTypesFullNames { get; }

        Dictionary<Type, bool> ComponentTypesAndEnabled { get; set; } = new Dictionary<Type, bool>();

        public bool EnabledInUI { get; set; }

        public virtual GUIContent Content
        {
            get
            {
                if(_content == null)
                    _content = CreateGUIContent();
                return _content;
            }
        }

        public virtual UIDefinition UIDefs { get; set; }

        public ISettingsContainer Settings { get; private set; }

        public Type FirstValidType { get; private set; }


        public ComponentDestroyerBase()
        {
            if(ComponentTypesFullNames == null)
                return;

            ComponentTypesAndEnabled = new Dictionary<Type, bool>(ComponentTypesFullNames.Length);

            for(int i = 0; i < ComponentTypesFullNames.Length; i++)
            {
                string tName = ComponentTypesFullNames[i];

                if(!string.IsNullOrWhiteSpace(tName))
                {
                    var type = TypeHelpers.GetTypeAnywhere(tName);
                    if(type != null)
                        ComponentTypesAndEnabled[type] = true; //TODO: Why did I decide to disable them all by default?
                }
            }

            FirstValidType = ComponentTypesAndEnabled.FirstOrDefault(c => c.Key != null).Key;

            if(!UIDefs)
            {
                string name = FirstValidType?.Name;
                UIDefs = new UIDefinition(StringHelpers.ToTitleCase(name) ?? "Invalid Destroyer");
            }

            Settings = this.GetOrCreateSettingsContainer();
        }

        protected virtual GUIContent CreateGUIContent()
        {
            return new GUIContent(UIDefs.Name, Icons.GetIconTextureFromType(FirstValidType));
        }

        public bool TryDestroyComponents(GameObject target)
        {
            try
            {
                if(Prepare(target) && DoDestroyComponents(target))
                {
                    Finish(target);
                    return true;
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
            return false;
        }

        public virtual void DrawUI(params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal();
            {
                if(GUILayout.Button(Content, Styles.SubToolButton, options))
                    TryDestroyComponents(PumkinTools.SelectedAvatar);
                if(Settings != null)
                    UIDefs.ExpandSettings = GUILayout.Toggle(UIDefs.ExpandSettings, Icons.Options, Styles.MediumIconButton);
            }
            EditorGUILayout.EndHorizontal();

            //Draw settings here
            if(Settings != null && UIDefs.ExpandSettings)
            {
                UIHelpers.DrawInVerticalBox(() =>
                {
                    EditorGUILayout.Space();
                    Settings?.DrawUI();
                });
            }
        }

        protected virtual bool Prepare(GameObject target)
        {
            if(ComponentTypesAndEnabled.Count == 0)
            {
                PumkinTools.LogError($"Can't use {GetType().Name} as it doesn't have any valid types.");
                return false;
            }

            if(!target)
                return false;

            Undo.RegisterCompleteObjectUndo(target, $"Destroy Components - {UIDefs.Name}");

            //Enable and disable components based on bool fields with TypeEnablerFieldAttribute in Settings
            if(Settings != null)
            {
                var fields = Settings.GetType().GetFields()?.Where(t => t.FieldType == typeof(bool));
                foreach(var f in fields)
                {
                    var attr = f.GetCustomAttributes(false).FirstOrDefault(a => a is TypeEnablerFieldAttribute) as TypeEnablerFieldAttribute;
                    if(attr == null)
                        continue;

                    bool? enabled = f.GetValue(Settings) as bool?;
                    if(enabled == null)
                        continue;

                    if(attr.EnabledType != null && ComponentTypesAndEnabled.ContainsKey(attr.EnabledType))
                        ComponentTypesAndEnabled[attr.EnabledType] = (bool)enabled;
                }
            }
            return true;
        }

        protected virtual bool DoDestroyComponents(GameObject target)
        {
            foreach(var comp in ComponentTypesAndEnabled)
                if(comp.Value)
                    DoDestroyByType(target, comp.Key);
            return true;
        }

        public virtual bool DoDestroyByType(GameObject target, Type componentType)
        {
            foreach(var co in target.GetComponentsInChildren(componentType, true))
            {
                if(ShouldIgnoreObject(co.gameObject))
                    continue;

                try
                {
                    UnityObjectHelpers.DestroyAppropriate(co, true);
                }
                catch(Exception e)
                {
                    PumkinTools.LogWarning(e.Message);
                }
            }
            return true;
        }

        protected virtual void Finish(GameObject target)
        {
            var sb = new StringBuilder();
            foreach(var kv in ComponentTypesAndEnabled)
                if(kv.Value)
                    sb.Append($"{kv.Key.Name}s, ");

            if(sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            string names = sb.ToString();
            if(string.IsNullOrWhiteSpace(names))
                names = UIDefs.Name;

            PumkinTools.Log($"Successfully removed all <b>{names}</b> from <b>{target.name}</b>");
        }

        protected virtual bool ShouldIgnoreObject(GameObject obj)
        {
            bool ignore = RemoveComponentsModule.IgnoreList.ShouldIgnoreTransform(obj.transform);
            if(ignore)
                PumkinTools.Log($"Ignoring <b>{obj.name}</b> because it's in the ignore list.");
            return ignore;
        }

        GUIContent _content;
    }
}
#endif