using Pumkin.AvatarTools2;
using Pumkin.AvatarTools2.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.Core.UI
{
    /// <summary>
    /// Used to define UI related stuff.
    /// Note that not all of these are applicable to every UI item
    /// </summary>
    [Serializable]
    public sealed class UIDefinition
    {
        const string FOLDER_NAME = "UI";
        const string NAME_SUFFIX = "_UIDefs";

        string JSONHeader
        {
            get
            {
                if(_header == null && !string.IsNullOrWhiteSpace(OwnerName))
                    _header = $"//{OwnerName}";
                return _header;
            }
        }

        string SavePath
        {
            get => _savePath = $"{SaveFolder}{OwnerName}{NAME_SUFFIX}.json";
        }

        string SaveFolder
        {
            get => $"{SettingsManager.SettingsPath}/{FOLDER_NAME}/";
        }

        string Header
        {
            get
            {
                if(_header == null)
                    _header = $"//{GetType().Name}";
                return _header;
            }
        }

        /// <summary>
        /// Name of the owner class. Used in the filename when saving to a config file
        /// </summary>
        public string OwnerName { get; set; }

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
        public bool IsExpanded { get => _isExpanded; set => _isExpanded = value; }

        /// <summary>
        /// Whether or not this item should be hidden in the UI
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Whether or not this items should have it's settings expanded in the UI
        /// </summary>
        public bool ExpandSettings { get => _expandSettings; set => _expandSettings = value; }

        private UIDefinition()
        {
            SettingsManager.SaveSettingsCallback -= SettingsManager_SaveSettingsCallback;
            SettingsManager.SaveSettingsCallback += SettingsManager_SaveSettingsCallback;

            SettingsManager.LoadSettingsCallback -= SettingsManager_LoadSettingsCallback;
            SettingsManager.LoadSettingsCallback += SettingsManager_LoadSettingsCallback;
        }

        private void SettingsManager_LoadSettingsCallback()
        {
            LoadFromConfigFile(SavePath);
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
            if(string.IsNullOrWhiteSpace(OwnerName))
                return false;

            Directory.CreateDirectory(SaveFolder);
            //PumkinTools.LogVerbose($"Saving... {OwnerName}{NAME_SUFFIX}.json");
            try
            {
                string json = JsonUtility.ToJson(this, true);
                if(json == "{}")
                    return false;

                json = JSONHeader + '\n' + JsonUtility.ToJson(this, true);
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
                {
                    PumkinTools.LogVerbose($"Trying to load UI defintion for {OwnerName} but the file header is not valid for this object");
                    return false;
                }
                JsonUtility.FromJsonOverwrite(string.Concat(lines.Skip(1)), this);
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

        private void SettingsManager_SaveSettingsCallback()
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

        [SerializeField] private bool _isExpanded;
        [SerializeField] private bool _expandSettings;
    }
}