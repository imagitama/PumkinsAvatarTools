using Pumkin.AvatarTools.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools
{
    abstract class SubToolBase : ISubTool
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string GameConfigurationString { get; set; }
        public bool AllowUpdate
        {
            get
            {
                return _allowUpdate;
            }

            set
            {
                if(_allowUpdate == value)
                    return;

                _allowUpdate = value;
                if(value)
                    SetupUpdateCallback();
                else
                    updateCallback = null;
                //EditorApplication.update -= updateCallback;                               
            }
        }

        bool _allowUpdate;
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
    

        EditorApplication.CallbackFunction updateCallback;

        public SubToolBase() 
        {
            Name = "Base Tool";
            Description = "Base tool description";
            CategoryName = "uncategorized";
            GameConfigurationString = "generic";
        }

        void SetupUpdateCallback()
        {
            if(updateCallback != null)
                return;

            Debug.Log($"Setting up Update callback for {Name}");
            updateCallback = new EditorApplication.CallbackFunction(Update);
            EditorApplication.update += updateCallback;
        }

        public SubToolBase(string name, string description, string category)
        {
            Name = name;
            Description = description;
            CategoryName = category;
        }

        public virtual void DrawUI()
        {
            if(GUILayout.Button(Content))
                Execute(AvatarTools.SelectedAvatar);            
        }

        public virtual bool Prepare(GameObject target)
        {
            if(!target)
            {
                Debug.LogError("No avatar selected");
                return false;
            }

            Debug.Log($"Registering undo: {Name}");
            Undo.RegisterFullObjectHierarchyUndo(target, Name);
            if(target.scene.name == null) //In case it's a prefab instance, which it probably is
                PrefabUtility.RecordPrefabInstancePropertyModifications(target);

            return true;
        }

        public virtual void Update()
        {
            if(!AllowUpdate)
                return;            
        }

        public abstract bool Execute(GameObject target);
        
        //public abstract void Finalize(GameObject target);
    }
}
