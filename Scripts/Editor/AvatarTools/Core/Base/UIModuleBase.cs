using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using UnityEngine.SceneManagement;

namespace Pumkin.AvatarTools2.Modules
{
    [Serializable]
    public abstract class UIModuleBase : IUIModule, IDisposable
    {
        public string GameConfigurationString { get; set; }
        public List<IUIModule> ChildModules { get; set; }
        public List<IItem> SubItems { get; set; }
        public virtual GUIContent GUIContent
        {
            get
            {
                return _content ?? (_content = new GUIContent(UIDefs.Name, UIDefs.Description));
            }
            set
            {
                _content = value;
            }
        }
        bool IsMainMenuModule { get; set; }
        public bool CanUpdate
        {
            get
            {
                return _allowUpdate;
            }

            set
            {
                if(_allowUpdate == value)
                    return;

                if(_allowUpdate = value)    //Intentional assign + check
                    SetupUpdateCallback(ref updateCallback, true);
                else
                    SetupUpdateCallback(ref updateCallback, false);
            }
        }

        public bool CanDrawSceneGUI
        {
            get
            {
                return _allowSceneGUI;
            }

            set
            {
                if(_allowSceneGUI == value)
                    return;

                if(_allowSceneGUI = value)    //Intentional assign + check
                    SetupOnSceneGUIDelegate(true);
                else
                    SetupOnSceneGUIDelegate(false);
            }
        }

        public abstract UIDefinition UIDefs { get; set; }

        protected GUIContent _content;

        EditorApplication.CallbackFunction updateCallback;

        bool _allowUpdate = false;
        bool _allowSceneGUI = false;

        bool firstDraw = true;

        bool shouldDrawHeader = true;
        bool shouldDrawBorder = true;
        bool shouldDrawDescriptionBox = false;
        bool shouldDrawChildrenInHorizontalPairs = false;

        public UIModuleBase()
        {
            var autoLoad = GetType().GetCustomAttribute<AutoLoadAttribute>(false);

            if(autoLoad)
                IsMainMenuModule = !autoLoad.HasParent;

            SubItems = new List<IItem>();
            ChildModules = new List<IUIModule>();
            if(UIDefs == null)
                UIDefs = new UIDefinition("Module", "A default module description. Did the UI definition fail to load?");

            shouldDrawHeader = !UIDefs.HasStyle(UIModuleStyles.NoHeader);
            shouldDrawBorder = !UIDefs.HasStyle(UIModuleStyles.NoBorder);
            shouldDrawDescriptionBox = UIDefs.HasStyle(UIModuleStyles.DrawDescriptionBox);
            shouldDrawChildrenInHorizontalPairs = UIDefs.HasStyle(UIModuleStyles.DrawChildrenInHorizontalPairs);
        }

        void SetupUpdateCallback(ref EditorApplication.CallbackFunction callback, bool add)
        {
            if(callback == null)
            {
                PumkinTools.LogVerbose($"Setting up Update callback for <b>{UIDefs.Name}</b>");
                callback = new EditorApplication.CallbackFunction(CheckThenUpdate);
            }

            if(!add)
                EditorApplication.update -= callback;
            else
                EditorApplication.update += callback;
        }

        void SetupOnSceneGUIDelegate(bool add)
        {
            if(add)
            {
                PumkinTools.LogVerbose($"Setting up OnSceneGUI callback for <b>{UIDefs.Name}</b>");
                SceneView.onSceneGUIDelegate += CheckThenOnSceneGUI;
            }
            else
            {
                SceneView.onSceneGUIDelegate -= CheckThenOnSceneGUI;
            }
        }

        void CheckThenUpdate()
        {
            if(CanUpdate)
                Update();
        }

        public virtual void Update() { }

        public virtual void Start() { }

        public void DrawUI()
        {
            if(firstDraw)
            {
                Start();
                firstDraw = false;
            }

            if(shouldDrawHeader)
                DrawHeader();

            Action drawContent = () =>
                DrawContent();

            if(UIDefs.IsExpanded || !shouldDrawHeader)
            {
                if(shouldDrawBorder)
                {
                    UIHelpers.DrawInVerticalBox(drawContent, Styles.ModuleBox);
                }
                else
                {
                    drawContent.Invoke();
                    EditorGUILayout.Space();
                }
            }
            else
                GUILayout.Space(3f);
        }

        public void CheckThenOnSceneGUI(SceneView sceneView)
        {
            if(CanDrawSceneGUI)
                OnSceneGUI(sceneView);
        }

        public virtual void OnSceneGUI(SceneView sceneView) { }

        public virtual void DrawHeader()
        {
            var style = IsMainMenuModule ? Styles.MenuFoldout : Styles.SubMenuFoldout;
            UIDefs.IsExpanded = UIHelpers.DrawFoldout(UIDefs.IsExpanded, GUIContent, true, style);
        }

        public virtual void DrawContent()
        {
            if(SubItems.Count > 0)
                EditorGUILayout.Space();

            if(shouldDrawDescriptionBox && !string.IsNullOrEmpty(UIDefs.Description))
                EditorGUILayout.HelpBox($"{UIDefs.Description}", MessageType.Info);

            DrawChildren();
        }

        public virtual void DrawSettings()
        {
            
        }

        public virtual void DrawChildren()
        {
            if(shouldDrawChildrenInHorizontalPairs)
                UIHelpers.DrawUIPairs(SubItems);
            else
            {
                foreach(var sub in SubItems)
                {
                    if(sub == null)
                        continue;
                    EditorGUI.BeginDisabledGroup(!sub.EnabledInUI);
                    sub.DrawUI();
                    EditorGUI.EndDisabledGroup();
                }
            }

            if(SubItems.Count > 0)
                EditorGUILayout.Space();

            ChildModules?.ForEach(c => c?.DrawUI());
        }

        public virtual void OrderChildren()
        {
            if(SubItems?.Count == 0 && ChildModules?.Count == 0)
                return;

            SubItems = SubItems?.OrderBy(t => t?.UIDefs?.OrderInUI)?.ToList() ?? SubItems;

            //Try to order by whether or not item has a settings button so stuff pairs up nicer
            //if(shouldDrawChildrenInHorizontalPairs) //TODO: Figure out the draw alone thing
            //    SubItems = SubItems?.OrderBy(t => t?.Settings != null || t.UIDefs.DrawAlone).ToList() ?? SubItems;

            ChildModules = ChildModules.OrderBy(t => t.UIDefs.OrderInUI).ToList();
            ChildModules.ForEach(x => x.OrderChildren());
        }

        public bool ReplaceSubItem(IItem oldItem, IItem newItem)
        {
            int index = SubItems.IndexOf(oldItem);
            if(index == -1)
                return false;

            SubItems[index] = newItem;

            PumkinTools.LogVerbose($"Replaced {oldItem.UIDefs.Name} with {newItem.UIDefs.Name}");
            return true;
        }

        public void Dispose()
        {
            EditorApplication.update -= CheckThenUpdate;
            SceneView.onSceneGUIDelegate -= CheckThenOnSceneGUI;
        }

        public static implicit operator bool(UIModuleBase module)
        {
            return !ReferenceEquals(module, null);
        }
    }
}