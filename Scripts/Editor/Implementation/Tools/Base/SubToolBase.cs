using Pumkin.AvatarTools.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools
{
    abstract class SubToolBase : ISubTool
    {
        public string Name { get; set; } = "Base tool";
        public string Description { get; set; } = "Base tool description";
        public string CategoryName { get; set; } = "uncategorized";

        public SubToolBase() { }        

        public SubToolBase(string name, string description, string category)
        {
            Name = name;
            Description = description;
            CategoryName = category;
        }

        public virtual void DrawUI()
        {
            if(GUILayout.Button(Name))
                Execute(AvatarTools.SelectedAvatar);
        }

        public abstract bool Execute(GameObject avatar);        
    }
}
