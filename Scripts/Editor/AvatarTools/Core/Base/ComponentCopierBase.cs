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
using UnityEditorInternal;
using UnityEngine;

namespace Pumkin.AvatarTools2.Copiers
{
    /// <summary>
    /// Base copier class that copies all components of multiple types by their full name. Also fixes references.
    /// </summary>
    public abstract class ComponentCopierBase : IComponentCopier
    {
        const string COPY_ALL_PROPERTIES_STRING = "__all__";    //TODO: Move to PropertyDefinitions

        public abstract string[] ComponentTypesFullNames { get; }

        Dictionary<Type, bool> ComponentTypesAndEnabled { get; set; } = new Dictionary<Type, bool>();

        public bool Active { get; set; }

        public bool EnabledInUI { get; set; } = true;

        public GUIContent Content
        {
            get
            {
                if(_content == null)
                    _content = CreateGUIContent();
                return _content;
            }
        }

        protected virtual GUIContent CreateGUIContent()
        {
            return new GUIContent(UIDefs.Name, Icons.GetIconTextureFromType(FirstValidType));
        }

        public virtual UIDefinition UIDefs { get; set; }

        public Type FirstValidType { get; private set; }

        public ISettingsContainer Settings { get; }

        public ComponentCopierBase()
        {
            ComponentTypesAndEnabled = new Dictionary<Type, bool>(ComponentTypesFullNames.Length);

            for(int i = 0; i < ComponentTypesFullNames.Length; i++)
            {
                string tName = ComponentTypesFullNames[i];

                if(!string.IsNullOrWhiteSpace(tName))
                {
                    var type = TypeHelpers.GetTypeAnywhere(tName);
                    if(type != null)
                        ComponentTypesAndEnabled[type] = false;
                }
            }

            FirstValidType = ComponentTypesAndEnabled.FirstOrDefault(c => c.Key != null).Key;

            if(!UIDefs)
            {
                string name = FirstValidType?.Name;
                UIDefs = new UIDefinition(StringHelpers.ToTitleCase(name) ?? "Invalid Copier");
            }

            Settings = this.GetOrCreateSettingsContainer();
        }

        public void DrawUI(params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(options);
            {
                Active = EditorGUILayout.ToggleLeft(Content, Active);
                if(Settings != null)
                    UIDefs.ExpandSettings = GUILayout.Toggle(UIDefs.ExpandSettings, Icons.Options, Styles.Icon);
            }
            EditorGUILayout.EndHorizontal();

            //Draw settings here
            if(Settings == null || !UIDefs.ExpandSettings)
                return;

            EditorGUILayout.Space();

            UIHelpers.DrawIndented(EditorGUI.indentLevel + 1, () =>
            {
                Settings?.DrawUI();
            });

            EditorGUILayout.Space();
        }

        public bool TryCopyComponents(GameObject objFrom, GameObject target)
        {
            try
            {
                if(Prepare(target, objFrom) && DoCopyComponents(target, objFrom))
                {
                    Finish(target, objFrom);
                    return true;
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
            return false;
        }

        protected virtual bool Prepare(GameObject target, GameObject objFrom)
        {
            if(ComponentTypesAndEnabled.Count == 0)
            {
                PumkinTools.LogError($"Can't use {GetType().Name} as it doesn't have any valid types.");
                return false;
            }

            if(!target)
                return false;

            SetComponentEnabedFromTypeEnablerAttributes();

            Undo.RegisterCompleteObjectUndo(target, $"Copy Components - {UIDefs.Name}");
            return true;
        }

        /// <summary>
        /// Enable and disable components based on bool fields with TypeEnablerFieldAttribute in Settings
        /// </summary>
        void SetComponentEnabedFromTypeEnablerAttributes()
        {
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
        }

        protected bool DoCopyComponents(GameObject target, GameObject objFrom)
        {
            foreach(var type_enabled in ComponentTypesAndEnabled)
                if(type_enabled.Value)
                    DoCopyByType(target, objFrom, type_enabled.Key);
            return true;
        }

        protected virtual bool DoCopyByType(GameObject target, GameObject objFrom, Type componentType)
        {
            var set = Settings as CopierSettingsContainerBase;
            bool createGameObjects = set != null && set.createGameObjects;

            string[] propNames = new string[] { "__all__" };//set.PropertyNames;

            var fromComps = objFrom.GetComponentsInChildren(componentType, true);

            foreach(var comp in fromComps)
            {
                if(ShouldIgnoreObject(comp.gameObject))
                    continue;

                if(!comp || ShouldIgnoreObject(comp.gameObject))
                {
                    PumkinTools.LogVerbose($"<b>{UIDefs.Name}</b> copier: Ignoring {comp.gameObject.name}");
                    continue;
                }

                var transPath = comp.transform.GetPathInHierarchy();
                var trans = target.transform.FindOrCreate(transPath, createGameObjects, objFrom.transform);
                if(!trans)
                    continue;


                Component addedComp;
                if(propNames.Length == 1 && propNames[0] == COPY_ALL_PROPERTIES_STRING)
                    addedComp = CopyWholeComponent(comp, trans);
                else
                    addedComp = CopyComponentProperties(comp, trans, propNames);

                FixReferences(addedComp, target.transform, set.createGameObjects);
            }
            return true;
        }

        protected Component CopyWholeComponent(Component coFrom, Transform transTo)
        {
            Type coFromType = coFrom.GetType();
            var existComps = transTo.gameObject.GetComponents(coFromType);

            ComponentUtility.CopyComponent(coFrom);
            ComponentUtility.PasteComponentAsNew(transTo.gameObject);

            var newComps = transTo.gameObject.GetComponents(coFromType);

            return newComps.Except(existComps)
                .FirstOrDefault();
        }

        protected Component CopyComponentProperties(Component compFrom, Transform transTo, params string[] propertyNames)
        {
            var compTo = transTo.gameObject.AddComponent(compFrom.GetType());
            var serialCompFrom = new SerializedObject(compFrom);
            var serialCompTo = new SerializedObject(compTo);

            foreach(var name in propertyNames)
            {
                if(string.IsNullOrWhiteSpace(name))
                    continue;

                var prop = serialCompFrom.FindProperty(name);
                if(prop == null)
                {
                    PumkinTools.LogVerbose($"<b>{UIDefs.Name}</b> Copier: Can't find property with name {name} on {compFrom}. Skipping");
                    continue;
                }
                serialCompTo.CopyFromSerializedProperty(prop);
            }
            serialCompTo.ApplyModifiedProperties();
            return compTo;
        }

        protected virtual void Finish(GameObject target, GameObject objFrom)
        {
            var sb = new StringBuilder();
            foreach(var kv in ComponentTypesAndEnabled)
                if(kv.Value)
                    sb.Append($"{kv.Key.Name}s, ");

            if(sb.Length > 2)
                sb.Remove(sb.Length - 2, 2);

            string names = sb.ToString();
            if(string.IsNullOrWhiteSpace(names))
                names = UIDefs.Name;

            PumkinTools.Log($"Successfully copied all <b>{names}</b> from <b>{objFrom.name}</b> to <b>{target.name}</b>");
        }

        protected virtual bool ShouldIgnoreObject(GameObject obj)
        {
            return ComponentCopiersModule.IgnoreList.ShouldIgnoreTransform(obj.transform);
        }

        protected virtual void FixReferences(Component comp, Transform targetHierarchyRoot, bool createGameObjects)
        {
            if(!comp)
                return;

            var serialComp = new SerializedObject(comp);

            serialComp.ForEachPropertyVisible(true, x =>
            {
                if(x.propertyType != SerializedPropertyType.ObjectReference || x.name == "m_Script")
                    return;

                var rf = x.objectReferenceValue;
                var trans = x.objectReferenceValue as Transform;
                if(trans != null)
                {
                    var tPath = trans.GetPathInHierarchy();
                    var transTarget = targetHierarchyRoot.FindOrCreate(tPath, createGameObjects, trans);
                    if(transTarget != null)
                        x.objectReferenceValue = transTarget;
                }
            });

            serialComp.ApplyModifiedProperties();
        }


        private GUIContent _content;
    }
}