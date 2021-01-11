using Pumkin.AvatarTools2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pumkin.AvatarTools2.UI;
using System.IO;

public static class SettingsManager
{
    const string FOLDER_NAME = "Pumkin Tools";

    public static event Action SaveSettingsCallback;
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
        PumkinToolsWindow.OnWindowDisabled -= PumkinToolsWindow_OnWindowDisabled;
        PumkinToolsWindow.OnWindowDisabled += PumkinToolsWindow_OnWindowDisabled;

        ConfigurationManager.BeforeConfigurationChanged -= ConfigurationManger_BeforeConfigurationChanged;
        ConfigurationManager.BeforeConfigurationChanged += ConfigurationManger_BeforeConfigurationChanged;

        UIBuilder.OnUIBuildFinished -= LoadAllSettings;
        UIBuilder.OnUIBuildFinished += LoadAllSettings;

        UIBuilder.BeforeUIBuildCallback -= ClearEvents;
        UIBuilder.BeforeUIBuildCallback += ClearEvents;
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
