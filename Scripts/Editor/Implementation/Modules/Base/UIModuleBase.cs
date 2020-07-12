using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools
{
    abstract class UIModuleBase : IUIModule
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string GameConfigurationString { get; set; }
        public List<ISubTool> SubTools { get; set; }        
        public bool IsExpanded { get; set; }

        GUIContent _content;
        GUIContent Content
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

        public UIModuleBase()
        {
            Name = "Generic Module";
            Description = "A generic module";
            GameConfigurationString = "generic";
            IsExpanded = false;
        }

        public virtual void Draw()
        {
            IsExpanded = EditorGUILayout.Toggle(new GUIContent(Name, Description), IsExpanded);

            if(IsExpanded)
            {
                foreach(var tool in SubTools)
                {
                    if(tool != null)
                        tool.DrawUI();
                    else
                        Debug.Log($"{tool} is null");

                }
            }
        }
    }
}
