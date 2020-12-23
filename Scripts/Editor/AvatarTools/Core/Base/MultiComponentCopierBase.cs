//using Pumkin.AvatarTools2.Interfaces;
//using Pumkin.AvatarTools2.Modules;
//using Pumkin.AvatarTools2.Settings;
//using Pumkin.AvatarTools2.UI;
//using Pumkin.Core.Extensions;
//using Pumkin.Core.Helpers;
//using Pumkin.Core.UI;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEditor;
//using UnityEngine;

//namespace Pumkin.AvatarTools2.Copiers
//{
//    /// <summary>
//    /// Base copier class that copies all components of multiple types by their full name. Also fixes references.
//    /// </summary>
//    public abstract class MultiComponentCopierBase : IComponentCopier
//    {
//        public string GameConfigurationString { get; set; }

//        public string[] ComponentTypeFullNames { get; set; }

//        public bool Active { get; set; }

//        public bool EnabledInUI { get; set; } = true;

//        public GUIContent Content { get; }

//        public UIDefinition UIDefs { get; set; }

//        protected Type[] componentTypes;

//        ISettingsContainer Settings;

//        public MultiComponentCopierBase()
//        {
//            //Get types of components and filter out nulls
//            componentTypes = ComponentTypeFullNames
//                .Where(c => !string.IsNullOrWhiteSpace(c))
//                .Select(c => TypeHelpers.GetType(c))
//                 .ToArray();

//            for(int i = 0; i < ComponentTypeFullNames.Length; i++)
//            {
//                string tName = ComponentTypeFullNames[i];

//                if(!string.IsNullOrWhiteSpace(tName))
//                    componentTypes[i] = TypeHelpers.GetType(tName);
//                else
//                    PumkinTools.LogVerbose($"{tName} is invalid");
//            }

//            if(!UIDefs)
//                UIDefs = new UIDefinition(componentTypes[0]?.Name ?? "Invalid Destroyer");
//        }

//        protected virtual bool DoCopyByType(GameObject objFrom, GameObject objTo, Type componentType, CopierSettingsContainerBase settings = null)
//        {
//            //TODO: Finish this
//            var set = Settings as CopierSettingsContainerBase;
//            bool createGameObjects = set != null && set.createGameObjects;
//            string[] propNames = set.PropertyNames;

//            foreach(var comp in objFrom.GetComponentsInChildren(componentType, true))
//            {
//                if(ShouldIgnoreObject(comp.gameObject))
//                    continue;

//                if(!comp || ShouldIgnoreObject(comp.gameObject))
//                {
//                    PumkinTools.LogVerbose($"<b>{UIDefs.Name}</b> copier: Ignoring {comp.gameObject.name}");
//                    continue;
//                }

//                var transPath = comp.transform.GetPathInHierarchy();
//                var trans = objTo.transform.FindOrCreate(transPath, createGameObjects, objFrom.transform);
//                if(!trans)
//                    continue;


//                Component addedComp;
//                if(propNames.IsNullOrEmpty())
//                    addedComp = CopyEverything(comp, trans);
//                else
//                    addedComp = CopyProperties(comp, trans, propNames);

//                FixReferences(addedComp, objTo.transform, set.createGameObjects);
//            }
//            return true;
//        }

//        protected override bool DoCopyComponents(GameObject objFrom, GameObject objTo)
//        {
//            foreach(var comp in componentTypes)
//                DoCopyByType(objFrom, objTo, comp);
//            return true;
//        }

//        protected override void Finish(GameObject objFrom, GameObject objTo)
//        {
//            StringBuilder sb = new StringBuilder();
//            for(int i = 0; i < componentTypes.Length; i++)
//            {
//                sb.Append($"{componentTypes[i].Name}");
//                if(i != componentTypes.Length - 1)
//                    sb.Append(", ");
//            }

//            PumkinTools.Log($"Successfully copied all <b>{sb.ToString()}</b> from <b>{objFrom.name}</b>");
//        }

//        public bool TryCopyComponents(GameObject objFrom, GameObject objTo)
//        {
//            throw new NotImplementedException();
//        }

//        public void DrawUI(params GUILayoutOption[] options)
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

//        protected virtual bool ShouldIgnoreObject(GameObject obj)
//        {
//            return ComponentCopiersModule.IgnoreList.ShouldIgnoreTransform(obj.transform);
//        }
//    }
//}
