#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Helpers;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace Pumkin.AvatarTools.Implementation.Modules
{
    public abstract class UIModuleBase : IUIModule
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string GameConfigurationString { get; set; }
        public bool IsExpanded { get; set; }
        public List<IUIModule> ChildModules { get; set; }
        public List<ISubItem> SubItems { get; set; }
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

        public UIModuleBase()
        {
            uiDefinition = GetType().GetCustomAttribute<UIDefinitionAttribute>(false);            
            if(uiDefinition != null)   //Don't want default values if attribute missing, so not using uiDefAttr?.Description ?? "whatever"
            {
                Name = uiDefinition.FriendlyName;
                Description = uiDefinition.Description;
                OrderInUI = uiDefinition.OrderInUI;
                
                shouldDrawHeader = !uiDefinition.ModuleStyles.Exists(t => t == UIModuleStyles.NoHeader);
                shouldDrawBorder = !uiDefinition.ModuleStyles.Exists(t => t == UIModuleStyles.NoBorder);
            }            
            else
            {
                Name = "Generic Module";
                Description = "A generic module";
                GameConfigurationString = "generic";
            }

            IsExpanded = false;
            
            SubItems = new List<ISubItem>();
            ChildModules = new List<IUIModule>();            
        }

        public virtual void Start() { }

        public void Draw()
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
            if(!string.IsNullOrEmpty(Description))            
                EditorGUILayout.HelpBox($"{Description}", MessageType.Info);
            
            DrawChildren();
        }        

        public virtual void DrawChildren()
        {
            foreach(var tool in SubItems)
                tool?.DrawUI();

            EditorGUILayout.Space();
            foreach(var child in ChildModules)
                child?.Draw();
        }

        public virtual void OrderChildren()
        {
            if(SubItems?.Count == 0 && ChildModules?.Count == 0)
                return;

            SubItems = SubItems.OrderBy(t => t.OrderInUI).ToList();
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