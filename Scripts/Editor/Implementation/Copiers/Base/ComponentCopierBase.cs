﻿#if UNITY_EDITOR
using Pumkin.Extensions;
using Pumkin.Interfaces.ComponentCopier;
using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Helpers;
using Pumkin.AvatarTools.Implementation.Modules;
using Pumkin.AvatarTools.Implementation.Settings;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Copiers
{
    abstract class ComponentCopierBase : IComponentCopier
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
            set
            {
                _content = value;
            }
        }
        public abstract string ComponentTypeNameFull { get; }
        public virtual SettingsContainerBase Settings { get => null; }
        public bool ExpandSettings { get; private set; }
        public bool Active { get; set; }

        public Type ComponentType
        {
            get
            {
                if(_componentType == null)
                    _componentType = TypeHelpers.GetType(ComponentTypeNameFull);
                return _componentType;
            }
        }

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

        protected virtual void SetupSettings() { }

        public virtual void DrawUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                Active = EditorGUILayout.ToggleLeft(Content, Active);
                if(Settings)
                    ExpandSettings = GUILayout.Toggle(ExpandSettings, Icons.Options, Styles.IconButton);
            }
            EditorGUILayout.EndHorizontal();

            //Draw settings here
            if(!Settings || !ExpandSettings)
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
                PumkinTools.Log($"{Name}: Couldn't find component type");
                return false;
            }

            return true;
        }

        protected virtual void Finish(GameObject objFrom, GameObject objTo)
        {
            PumkinTools.Log($"{ComponentTypeNameFull} copier completed successfully.");
        }


        protected virtual bool DoCopy(GameObject objFrom, GameObject objTo)
        {
            var compsFrom = objFrom.GetComponentsInChildren(ComponentType, true);

            foreach(var coFrom in compsFrom)
            {
                var transPath = coFrom.transform.GetPathInHierarchy();
                var trans = objTo.transform.Find(transPath);
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