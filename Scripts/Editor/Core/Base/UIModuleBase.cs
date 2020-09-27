#if UNITY_EDITOR
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Pumkin.Core.Attributes;
using Pumkin.Core.Helpers;

namespace Pumkin.AvatarTools.Base
{
    public abstract class UIModuleBase : IUIModule
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string GameConfigurationString { get; set; }
        public bool IsExpanded { get; set; }
        public List<IUIModule> ChildModules { get; set; }
        public List<IItem> SubItems { get; set; }
        public virtual bool IsHidden { get; set; }
        public virtual GUIContent GUIContent
        {
            get
            {
                return _content ?? (_content = new GUIContent(Name, Description));
            }
            set
            {
                _content = value;
            }
        }
        public int OrderInUI { get; set; }

        protected GUIContent _content;

        UIDefinitionAttribute uiDefinition;

        bool firstDraw = true;

        bool shouldDrawHeader = true;
        bool shouldDrawBorder = true;
        bool shouldDrawDescriptionBox = false;
        bool shouldDrawChildrenInHorizontalPairs = false;

        public UIModuleBase()
        {
            uiDefinition = GetType().GetCustomAttribute<UIDefinitionAttribute>(false);
            if(uiDefinition != null)   //Don't want default values if attribute missing, so not using uiDefAttr?.Description ?? "whatever"
            {
                Name = uiDefinition.FriendlyName;
                Description = uiDefinition.Description;
                OrderInUI = uiDefinition.OrderInUI;

                shouldDrawHeader = !uiDefinition.HasStyle(UIModuleStyles.NoHeader);
                shouldDrawBorder = !uiDefinition.HasStyle(UIModuleStyles.NoBorder);

                shouldDrawDescriptionBox = uiDefinition.HasStyle(UIModuleStyles.DrawDescriptionBox);
                shouldDrawChildrenInHorizontalPairs = uiDefinition.HasStyle(UIModuleStyles.DrawChildrenInHorizontalPairs);
            }
            else
            {
                Name = "Generic Module";
                Description = "A generic module";
                GameConfigurationString = "generic";
            }

            IsExpanded = false;

            SubItems = new List<IItem>();
            ChildModules = new List<IUIModule>();
        }

        public virtual void Start() { }

        public void DrawUI()
        {
            if(firstDraw)
            {
                Start();
                firstDraw = false;
            }
            Action drawContent = () =>
            {
                if(shouldDrawHeader)
                    DrawHeader();
                if(IsExpanded || !shouldDrawHeader)
                    DrawContent();
            };

            if(shouldDrawBorder)
            {
                UIHelpers.VerticalBox(drawContent);
            }
            else
            {
                drawContent.Invoke();
                EditorGUILayout.Space();
            }
        }

        public virtual void DrawHeader()
        {
            IsExpanded = UIHelpers.DrawFoldout(IsExpanded, GUIContent, true, Styles.MenuFoldout);
        }

        public virtual void DrawContent()
        {
            EditorGUILayout.Space();

            if(shouldDrawDescriptionBox && !string.IsNullOrEmpty(Description))
                EditorGUILayout.HelpBox($"{Description}", MessageType.Info);

            DrawChildren();
        }

        public virtual void DrawChildren()
        {
            if(shouldDrawChildrenInHorizontalPairs)
                UIHelpers.DrawUIPairs(SubItems);
            else
                SubItems.ForEach(s => s?.DrawUI());


            EditorGUILayout.Space();
            ChildModules?.ForEach(c => c?.DrawUI());
        }

        public virtual void OrderChildren()
        {
            if(SubItems?.Count == 0 && ChildModules?.Count == 0)
                return;

            SubItems = SubItems.OrderBy(t => t.OrderInUI).ToList();

            //Try to order by settings so stuff pairs up nicer if needed
            if(shouldDrawChildrenInHorizontalPairs)
                SubItems = SubItems.OrderBy(t => t.Settings != null).ToList();

            ChildModules = ChildModules.OrderBy(t => t.OrderInUI).ToList();
            ChildModules.ForEach(x => x.OrderChildren());
        }

        public static implicit operator bool(UIModuleBase module)
        {
            return !ReferenceEquals(module, null);
        }
    }
}
#endif