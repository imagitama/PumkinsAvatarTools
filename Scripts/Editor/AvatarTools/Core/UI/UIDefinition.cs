using Pumkin.AvatarTools2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.Core.UI
{
    /// <summary>
    /// Used to define UI related stuff.
    /// Note that not all of these are applicable to every UI item
    /// </summary>
    public class UIDefinition
    {
        const string MAIN_FOLDER_SUFFIX = "/Settings/UI";

        string SavePath
        {
            get
            {
                if(_savePath == null)
                    _savePath = $"{SaveFolder}{GetType().Name}.json";
                return _savePath;
            }
        }

        string SaveFolder
        {
            get
            {
                if(_saveFolder == null)
                    _saveFolder = $"{PumkinTools.MainFolderPath}/{MAIN_FOLDER_SUFFIX}";
                return _saveFolder;
            }
        }

        string Header
        {
            get
            {
                if(_header == null)
                    _header = $"//{GetType().FullName}";
                return _header;
            }
        }

        /// <summary>
        /// The name of this item in the UI
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of this item in the UI
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The order of which this will item will be drawn in the UI
        /// </summary>
        public int OrderInUI { get; set; }

        /// <summary>
        /// Module styles for this item. Only works with UI Modules
        /// </summary>
        public List<UIModuleStyles> ModuleStyles { get; set; }

        /// <summary>
        /// Whether or not this item should be expanded in the UI
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Whether or not this item should be hidden in the UI
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Whether or not this items should have it's settings expanded in the UI
        /// </summary>
        public bool ExpandSettings { get; set; }

        private UIDefinition()
        {
            LoadFromConfigFile(SavePath);

            PumkinToolsWindow.OnWindowDestroyed += PumkinToolsWindow_OnWindowDestroyed;
        }

        public UIDefinition(string name, string description, int orderInUI, params UIModuleStyles[] moduleStyles) : this()
        {
            Name = name;
            Description = description;
            OrderInUI = orderInUI;
            ModuleStyles = moduleStyles?.ToList() ?? new List<UIModuleStyles>();

            IsExpanded = false;
            IsHidden = false;
            ExpandSettings = false;
        }

        public UIDefinition(string name, string description, params UIModuleStyles[] moduleStyles)
            : this(name, description, 0, null) { }

        public UIDefinition(string name, int orderInUI, params UIModuleStyles[] moduleStyles)
            : this(name, null, orderInUI, moduleStyles) { }

        public UIDefinition(string name, params UIModuleStyles[] moduleStyles)
            : this(name, null, 0, moduleStyles) { }

        /// <summary>
        /// Forces IItem to draw alone even when DrawChildrenInHorizontalPairs is defined in module. Only works for IItem
        /// </summary>
        //public bool DrawAlone { get; set; }
        //TODO: figure this one out

        public bool HasStyle(UIModuleStyles style) =>
            ModuleStyles.Exists(t => t == style);

        public bool SaveToConfigFile(string filePath)
        {
            return false;
            try
            {
                string json = Header + '\n' + JsonUtility.ToJson(this, true);
                File.WriteAllText(SavePath, json);
            }
            catch(Exception e)
            {
                PumkinTools.LogException(e);
                return false;
            }
            return true;
        }

        public bool LoadFromConfigFile(string filePath)
        {
            try
            {
                if(!File.Exists(SavePath))
                    return false;

                var lines = File.ReadAllLines(SavePath);
                if(lines.Length == 0)
                    return false;

                if(lines[0] != Header)
                    PumkinTools.LogVerbose($"Trying to load settings for {GetType().Name} but the file header is not valid for this container");

                JsonUtility.FromJsonOverwrite(string.Concat(lines), this);
            }
            catch(Exception e)
            {
                PumkinTools.LogException(e);
                if(File.Exists(SavePath))
                    File.Delete(SavePath);
                return false;
            }
            return true;
        }


        private void PumkinToolsWindow_OnWindowDestroyed()
        {
            SaveToConfigFile(SavePath);
        }

        private void ConfigurationManager_BeforeConfigurationChanged(string newString)
        {
            SaveToConfigFile(SavePath);
        }


        public static implicit operator bool(UIDefinition uid)
        {
            return !ReferenceEquals(uid, null);
        }

        private string _savePath;
        private string _saveFolder;
        private string _header;
    }
}
