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

namespace Pumkin.AvatarTools.Implementation.Modules
{
    abstract class UIModuleBase : IUIModule
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string GameConfigurationString { get; set; }
        public bool IsExpanded { get; set; }
        public List<IUIModule> ChildModules { get; set; }
        public List<ISubTool> SubTools { get; set; }
        public GUIContent LabelContent
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
        public string ParentModuleID { get; set; }
        public int OrderInUI { get; set; }

        protected GUIContent _content;

        public UIModuleBase()
        {
            Name = "Generic Module";
            Description = "A generic module";            
            GameConfigurationString = "generic";
            IsExpanded = false;
            
            SubTools = new List<ISubTool>();
            ChildModules = new List<IUIModule>();
        }

        public virtual void Draw()
        {
            //GUILayout.BeginArea(rect, GUI.skin.box);            
            IsExpanded = UIHelpers.DrawFoldout(IsExpanded, LabelContent, true, Styles.MenuFoldout);

            if(IsExpanded)
            {
                foreach(var tool in SubTools)
                    tool?.DrawUI();

                EditorGUILayout.Space();

                foreach(var child in ChildModules)
                    child?.Draw();
            }            
        }
    }
}
