using Pumkin.AvatarTools2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pumkin.AvatarTools2.UI;
using System.IO;
using UnityEditor;

public static class SettingsManager
{
    const string FOLDER_NAME = "Pumkin Tools";

    /// <summary>
    /// Subscribe to this event to get a callback when saving settings
    /// </summary>
    public static event Action SaveSettingsCallback;
    /// <summary>
    /// Subscribe to this event to get a callback when loading settings
    /// </summary>
    public static event Action LoadSettingsCallback;

    public static string SettingsPath
    {
        get
        {
            if(_settingsPath == null)
                _settingsPath = Path.GetFullPath($"{Application.dataPath}/../ProjectSettings/{FOLDER_NAME}/");
            return _settingsPath;
        }
    }

    static SettingsManager()
    {
        LoadSettingsCallback = null;
        SaveSettingsCallback = null;

        PumkinToolsWindow.OnWindowDisabled += PumkinToolsWindow_OnWindowDisabled;

        ConfigurationManager.BeforeConfigurationChanged += ConfigurationManger_BeforeConfigurationChanged;

        UIBuilder.OnUIBuildFinished += LoadAllSettings;

        UIBuilder.BeforeUIBuildCallback += ClearEvents;
    }

    public static bool SaveToJson(object obj, string pathInSettings)
    {//TODO: Make this work
        string json = EditorJsonUtility.ToJson(obj);
        if(string.IsNullOrWhiteSpace(json) || json == "{}")
            return false;

        try
        {
            Directory.CreateDirectory(SettingsPath);
            File.WriteAllText($"{SettingsPath}{pathInSettings}", json);
        }
        catch
        {
            return false;
        }
        return true;
    }

    public static void LoadFromJson<T>(string pathInSettings, T objectToOverwrite)
    {
        try
        {
            string json = File.ReadAllText($"{SettingsPath}{pathInSettings}");
            if(!string.IsNullOrWhiteSpace(json) && json != "{}")
                EditorJsonUtility.FromJsonOverwrite(json, objectToOverwrite);
        }
        catch { }
    }

    private static void PumkinToolsWindow_OnWindowDisabled()
    {
        EnsureParentsAssigned();
        SaveSettingsCallback?.Invoke();
    }

    static void ConfigurationManger_BeforeConfigurationChanged(string newConfiguration)
    {
        SaveSettingsCallback?.Invoke();
    }

    static void ClearEvents()
    {
        SaveSettingsCallback = null;
        LoadSettingsCallback = null;
    }

    private static void LoadAllSettings()
    {
        EnsureParentsAssigned();
        LoadSettingsCallback?.Invoke();
    }

    static void EnsureParentsAssigned()
    {
        foreach(var item in IDManager.Items.Values)
            item.UIDefs.OwnerName = item.GetType().Name;

        foreach(var mod in IDManager.Modules.Values)
            mod.UIDefs.OwnerName = mod.GetType().Name;
    }

    static string _settingsPath;
}
