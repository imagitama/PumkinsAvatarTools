//using Pumkin.AvatarTools2.UI;
//using System;
//using System.Linq;
//using UnityEditor;
//using UnityEditorInternal;
//using UnityEngine;
//using Pumkin.Core.Extensions;
//using Pumkin.Core.Helpers;
//using Pumkin.AvatarTools2.Interfaces;
//using Pumkin.AvatarTools2.Modules;
//using Pumkin.Core.UI;
//using Pumkin.AvatarTools2.Settings;

//namespace Pumkin.AvatarTools2.Copiers
//{
//    /// <summary>
//    /// Base copier class that copies all components of type by it's full name. Also fixes references
//    /// </summary>
//    public abstract class ComponentCopierBase : IComponentCopier
//    {
//        public string GameConfigurationString { get; set; }

//        public virtual GUIContent Content
//        {
//            get
//            {
//                if(_content == null)
//                    _content = CreateGUIContent();
//                return _content;
//            }
//            set => _content = value;
//        }

//        public abstract string ComponentTypeFullName { get; }

//        public bool Active { get; set; }

//        public Type ComponentType
//        {
//            get
//            {
//                if(_componentType == null)
//                    _componentType = TypeHelpers.GetType(ComponentTypeFullName);
//                return _componentType;
//            }
//        }

//        public virtual UIDefinition UIDefs { get; set; }

//        public virtual ISettingsContainer Settings => _baseSettings;

//        public bool EnabledInUI { get; set; }

//        protected bool shouldFixReferences = false;

//        GUIContent _content;
//        Type _componentType;
//        CopierSettingsContainerBase _baseSettings;

//        public ComponentCopierBase()
//        {
//            if(UIDefs == null)
//            {
//                string name = ComponentType?.Name ?? GetType().Name;
//                UIDefs = new UIDefinition(StringHelpers.ToTitleCase(name));
//            }
//            SetupSettings();
//        }

//        protected virtual GUIContent CreateGUIContent()
//        {
//            return new GUIContent(UIDefs.Name, Icons.GetIconTextureFromType(ComponentType));
//        }

//        protected virtual void SetupSettings()
//        {
//            _baseSettings = ScriptableObject.CreateInstance<CopierSettingsContainerBase>();
//        }

//        public virtual void DrawUI(params GUILayoutOption[] options)
//        {
//            EditorGUILayout.BeginHorizontal(options);
//            {
//                Active = EditorGUILayout.ToggleLeft(Content, Active);
//                if(Settings != null)
//                    UIDefs.ExpandSettings = GUILayout.Toggle(UIDefs.ExpandSettings, Icons.Options, Styles.Icon);
//            }
//            EditorGUILayout.EndHorizontal();

//            //Draw settings here
//            if(Settings == null || !UIDefs.ExpandSettings)
//                return;

//            EditorGUILayout.Space();

//            UIHelpers.DrawIndented(EditorGUI.indentLevel + 1, () =>
//            {
//                Settings.Editor.OnInspectorGUINoScriptField();
//            });

//            EditorGUILayout.Space();
//        }

//        public bool TryCopyComponents(GameObject objFrom, GameObject objTo)
//        {
//            try
//            {
//                if(Prepare(objFrom, objTo) && DoCopyComponents(objFrom, objTo))
//                {
//                    Finish(objFrom, objTo);
//                    return true;
//                }
//            }
//            catch(Exception e)
//            {
//                Debug.LogException(e);
//            }
//            return false;
//        }

//        protected virtual bool Prepare(GameObject objFrom, GameObject objTo)
//        {
//            if((!objFrom || !objTo) || (objFrom == objTo))
//                return false;

//            if(ComponentType == null)
//            {
//                PumkinTools.Log($"{ComponentTypeFullName}: Couldn't find component type");
//                return false;
//            }
//            return true;
//        }

//        protected virtual bool ShouldIgnoreObject(GameObject obj)
//        {
//            return ComponentCopiersModule.IgnoreList.ShouldIgnoreTransform(obj.transform);
//        }

//        protected virtual bool DoCopyComponents(GameObject objFrom, GameObject objTo)
//        {
//            var compsFrom = objFrom.GetComponentsInChildren(ComponentType, true);

//            var set = Settings as CopierSettingsContainerBase;
//            bool createGameObjects = set != null && set.createGameObjects;
//            string[] propNames = set.PropertyNames;

//            foreach(var coFrom in compsFrom)
//            {
//                if(!coFrom || ShouldIgnoreObject(coFrom.gameObject))
//                {
//                    PumkinTools.LogVerbose($"<b>{UIDefs.Name}</b> copier: Ignoring {coFrom.gameObject.name}");
//                    continue;
//                }

//                var transPath = coFrom.transform.GetPathInHierarchy();
//                var trans = objTo.transform.FindOrCreate(transPath, createGameObjects, objFrom.transform);
//                if(!trans)
//                    continue;


//                Component addedComp;
//                if(propNames.IsNullOrEmpty())
//                    addedComp = CopyEverything(coFrom, trans);
//                else
//                    addedComp = CopyProperties(coFrom, trans, propNames);

//                FixReferences(addedComp, objTo.transform, createGameObjects);
//            }
//            return true;
//        }

//        protected Component CopyEverything(Component coFrom, Transform transTo)
//        {
//            var existComps = transTo.gameObject.GetComponents(ComponentType);

//            ComponentUtility.CopyComponent(coFrom);
//            ComponentUtility.PasteComponentAsNew(transTo.gameObject);

//            var newComps = transTo.gameObject.GetComponents(ComponentType);

//            return newComps.Except(existComps)
//                .FirstOrDefault();

//        }

//        protected Component CopyProperties(Component compFrom, Transform transTo, params string[] propertyNames)
//        {
//            var compTo = transTo.gameObject.AddComponent(compFrom.GetType());
//            var serialCompFrom = new SerializedObject(compFrom);
//            var serialCompTo = new SerializedObject(compTo);

//            foreach(var name in propertyNames)
//            {
//                if(string.IsNullOrWhiteSpace(name))
//                    continue;

//                var prop = serialCompFrom.FindProperty(name);
//                if(prop == null)
//                {
//                    PumkinTools.LogVerbose($"<b>{UIDefs.Name}</b> Copier: Can't find property with name {name} on {compFrom}. Skipping");
//                    continue;
//                }
//                serialCompTo.CopyFromSerializedProperty(prop);
//            }
//            serialCompTo.ApplyModifiedProperties();
//            return compTo;
//        }

//        protected virtual void FixReferences(Component comp, Transform targetHierarchyRoot, bool createGameObjects)
//        {
//            if(!comp)
//                return;

//            var serialComp = new SerializedObject(comp);

//            serialComp.ForEachPropertyVisible(true, x =>
//            {
//                if(x.propertyType != SerializedPropertyType.ObjectReference || x.name == "m_Script")
//                    return;

//                var rf = x.objectReferenceValue;
//                var trans = x.objectReferenceValue as Transform;
//                if(trans != null)
//                {
//                    var tPath = trans.GetPathInHierarchy();
//                    var transTarget = targetHierarchyRoot.FindOrCreate(tPath, createGameObjects, trans);
//                    if(transTarget != null)
//                        x.objectReferenceValue = transTarget;
//                }
//            });

//            serialComp.ApplyModifiedProperties();
//        }

//        protected virtual void Finish(GameObject objFrom, GameObject objTo)
//        {
//            PumkinTools.Log($"<b>{UIDefs.Name}</b> copier completed successfully.");
//        }
//    }
//}