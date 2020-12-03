#if UNITY_EDITOR
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.AvatarTools.Modules;
using Pumkin.Core.UI;

namespace Pumkin.AvatarTools.Base
{
    [Serializable]
    public abstract class UIModuleBase : IUIModule
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
        public abstract UIDefinition UIDefs { get; set; }

        [SerializeField] protected GUIContent _content;

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
            if(!UIDefs)
                UIDefs = new UIDefinition("Generic Module", "A generic Module");

            shouldDrawHeader = !UIDefs.HasStyle(UIModuleStyles.NoHeader);
            shouldDrawBorder = !UIDefs.HasStyle(UIModuleStyles.NoBorder);
            shouldDrawDescriptionBox = UIDefs.HasStyle(UIModuleStyles.DrawDescriptionBox);
            shouldDrawChildrenInHorizontalPairs = UIDefs.HasStyle(UIModuleStyles.DrawChildrenInHorizontalPairs);
        }

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
            {
                DrawContent();
            };

            if(UIDefs.IsExpanded || !shouldDrawHeader)
            {
                if(shouldDrawBorder)
                {
                    UIHelpers.VerticalBox(drawContent, Styles.ModuleBox);
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
                    EditorGUI.BeginDisabledGroup(!sub.UIDefs.EnabledInUI);
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
            if(shouldDrawChildrenInHorizontalPairs)
                SubItems = SubItems?.OrderBy(t => t?.Settings != null).ToList() ?? SubItems;

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

        public static implicit operator bool(UIModuleBase module)
        {
            return !ReferenceEquals(module, null);
        }
    }
}
#endif