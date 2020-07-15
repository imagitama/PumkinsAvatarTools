using Pumkin.AvatarTools.Interfaces;
using System;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools
{
    abstract class SubToolBase : ISubTool
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentModuleID { get; set; }
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

        public int OrderInUI { get; set; }

        EditorApplication.CallbackFunction updateCallback;

        public SubToolBase() 
        {
            Name = "Base Tool";
            Description = "Base tool description";
            ParentModuleID = "";
            GameConfigurationString = "generic";
            OrderInUI = 0;
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
            ParentModuleID = category;
        }

        public virtual void DrawUI()
        {
            if(GUILayout.Button(Content))
                TryExecute(AvatarTools.SelectedAvatar);            
        }

        public bool TryExecute(GameObject target)
        {
            try
            {
                if(Prepare(target) && DoAction(target))
                {
                    Finish(target);
                    return true;
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
            return false;
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

        public abstract bool DoAction(GameObject target);

        public virtual void Finish(GameObject target)
        {
            Debug.Log($"{Name} completed successfully.");
        }

        public virtual void Update()
        {
            if(!AllowUpdate)
                return;
        }
    }
}
