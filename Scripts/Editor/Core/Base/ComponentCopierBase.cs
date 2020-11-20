#if UNITY_EDITOR
using Pumkin.AvatarTools.UI;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Pumkin.Core.Extensions;
using Pumkin.Core.Helpers;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core;

namespace Pumkin.AvatarTools.Base
{
    public abstract class ComponentCopierBase : IComponentCopier
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string GameConfigurationString { get; set; }
        public int OrderInUI { get; set; }

        protected bool fixReferences = false;
        public virtual GUIContent Content
        {
            get
            {
                if(_content == null)
                    _content = CreateGUIContent();
                return _content;
            }
            set => _content = value;
        }
        public abstract string ComponentTypeNameFull { get; }
        public virtual ISettingsContainer Settings => settings;
        public bool ExpandSettings { get; private set; }
        public bool Active { get; set; }

        CopierSettingsContainerBase settings;

        public Type ComponentType
        {
            get
            {
                if(_componentType == null)
                    _componentType = TypeHelpers.GetType(ComponentTypeNameFull);
                return _componentType;
            }
        }

        public bool EnabledInUI { get; set; } = true;

        GUIContent _content;
        Type _componentType;

        public ComponentCopierBase()
        {
            var uiDefAttr = GetType().GetCustomAttribute<UIDefinitionAttribute>(false);
            if(uiDefAttr != null)   //Don't want default values if attribute missing, so not using uiDefAttr?.Description ?? "whatever"
            {
                Name = uiDefAttr.FriendlyName;
                Description = uiDefAttr.Description;
                OrderInUI = uiDefAttr.OrderInUI;
            }
            else
            {
                Name = GetType().Name;
                Description = "Base Copier description";
                OrderInUI = 0;
            }
            Content = CreateGUIContent();
            SetupSettings();
        }

        protected virtual GUIContent CreateGUIContent()
        {
            return new GUIContent(Name, Icons.GetIconTextureFromType(ComponentType));
        }

        protected virtual void SetupSettings()
        {
            settings = ScriptableObject.CreateInstance<CopierSettingsContainerBase>();
        }

        public virtual void DrawUI(params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(options);
            {
                Active = EditorGUILayout.ToggleLeft(Content, Active);
                if(Settings != null)
                    ExpandSettings = GUILayout.Toggle(ExpandSettings, Icons.Options, Styles.Icon);
            }
            EditorGUILayout.EndHorizontal();

            //Draw settings here
            if(Settings == null || !ExpandSettings)
                return;

            EditorGUILayout.Space();

            UIHelpers.DrawIndented(EditorGUI.indentLevel + 1, () =>
            {
                Settings.Editor.OnInspectorGUINoScriptField();
            });

            EditorGUILayout.Space();
        }

        public bool TryCopyComponents(GameObject objFrom, GameObject objTo)
        {
            try
            {
                if(Prepare(objFrom, objTo) && DoCopy(objFrom, objTo))
                {
                    Finish(objFrom, objTo);
                    return true;
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
            return false;
        }

        protected virtual bool Prepare(GameObject objFrom, GameObject objTo)
        {
            if((!objFrom || !objTo) || (objFrom == objTo))
                return false;
            if(ComponentType == null)
            {
                PumkinTools.Log($"{ComponentTypeNameFull}: Couldn't find component type");
                return false;
            }

            //TODO: Register copier and destroyer in some kind of manager
            //if(settings.removeAllBeforeCopying)
            //{
            //    var desType = TypeHelpers.GetType($"{ComponentType.Name}Destroyer");
            //    if(desType != null)
            //    {
            //        var des = Activator.CreateInstance(desType) as IComponentDestroyer;
            //        des?.TryDestroyComponents(objTo);
            //    }
            //}

            return true;
        }

        protected virtual void Finish(GameObject objFrom, GameObject objTo)
        {
            PumkinTools.Log($"<b>{Name}</b> copier completed successfully.");
        }

        protected virtual bool ShouldIgnoreObject(GameObject obj)
        {
            return ComponentCopiersModule.IgnoreList.ShouldIgnoreTransform(obj.transform);
        }

        protected virtual bool DoCopy(GameObject objFrom, GameObject objTo)
        {
            var compsFrom = objFrom.GetComponentsInChildren(ComponentType, true);

            foreach(var coFrom in compsFrom)
            {
                if(!coFrom || ShouldIgnoreObject(coFrom.gameObject))
                    continue;

                var transPath = coFrom.transform.GetPathInHierarchy();
                var trans = objTo.transform.FindOrCreate(transPath, settings.createGameObjects, objFrom.transform);
                if(!trans)
                    continue;

                var existComps = trans.gameObject.GetComponents(ComponentType);

                ComponentUtility.CopyComponent(coFrom);
                ComponentUtility.PasteComponentAsNew(trans.gameObject);

                var addedComp = trans.gameObject.GetComponents(ComponentType)
                    .Except(existComps)
                    .FirstOrDefault();

                FixReferences(addedComp, objTo.transform);
            }
            return true;
        }

        protected virtual void FixReferences(Component comp, Transform targetHierarchyRoot)
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
                    var transTarget = targetHierarchyRoot.Find(tPath);
                    if(transTarget != null)
                        x.objectReferenceValue = transTarget;
                }
            });

            serialComp.ApplyModifiedProperties();
        }
    }
}
#endif