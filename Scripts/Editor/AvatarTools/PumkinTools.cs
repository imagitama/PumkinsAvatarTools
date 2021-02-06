using System;
using Pumkin.Core;
using Pumkin.Core.Helpers;
using UnityEngine;
using Pumkin.Core.Logger;
using System.IO;
using System.Linq;

namespace Pumkin.AvatarTools2
{
    public static class PumkinTools
    {
        public static event Delegates.SelectedChangeHandler OnAvatarSelectionChanged;

        public static string MainFolderPath
        {
            get
            {
                if(_toolFolderPath != null)
                    return _toolFolderPath;

                //Get all folders starting with PumkinsAvatarTools (might end in -master if cloned) and get the the folder that has a Scripts folder inside
                string[] folder = Directory.GetDirectories(Application.dataPath, "PumkinsAvatarTools*", SearchOption.AllDirectories);
                if(folder.Length > 0)
                {
                    foreach(string f in folder)
                    {
                        var subdirs = Directory.GetDirectories(f, "*", SearchOption.TopDirectoryOnly);
                        string sub = subdirs.FirstOrDefault(x => x.Equals(f + "\\Scripts"));
                        if(!string.IsNullOrEmpty(sub))
                        {
                            _toolFolderPath = f + '/';
                            return _toolFolderPath;
                        }
                    }
                }
                return _toolFolderPath;
            }
        }

        public static string ResourceFolderPath
        {
            get
            {
                return MainFolderPath + "Resources/";
            }
        }

        public static GameObject SelectedAvatar
        {
            get => _selectedAvatar;
            set
            {
                GameObject newAvatar = value;
                if(_selectedAvatar != value)
                {
                    if(newAvatar != null)
                        newAvatar = newAvatar.transform.root.gameObject;
                    try
                    {
                        AvatarSelectionChanged(value);
                    }
                    catch(Exception e)
                    {
                        PumkinTools.LogException(e);
                    }
                }
                _selectedAvatar = newAvatar;
            }
        }

        const string SELECTED_AVATAR_PREF_NAME = "selectedAvatar";
        //
        static PumkinTools()
        {
            // Load selected avatar
            SelectedAvatar = PrefManager.GetUnityObject<GameObject>(SELECTED_AVATAR_PREF_NAME);
        }

        public static void AvatarSelectionChanged(GameObject newSelection)
        {
            PrefManager.SetObject(SELECTED_AVATAR_PREF_NAME, newSelection);
            OnAvatarSelectionChanged?.Invoke(newSelection);
        }

        //Version
        public static Version version { get; } = new Version(2, 0);
        static bool isWipVersion = true;

        public static string versionSuffix
        {
            get
            {
                if(_versionSuffix == null)
                    _versionSuffix = isWipVersion ? " - WIP" : "";
                return _versionSuffix;
            }
        }

        #region Logger

        public static PumkinLogger Logger { get; } = new PumkinLogger("blue", "Pumkin Tools");
        public static PumkinLogger VerboseLogger { get; } = new PumkinLogger("red", "Pumkin Tools Verbose");

        public static void Log(string msg, UnityEngine.Object context)
        {
            Logger.LogFormat(LogType.Log, context, msg);
        }

        public static void Log(string msg)
        {
            Logger.LogFormat(LogType.Log, null, msg);
        }

        public static void LogWarning(string msg)
        {
            Logger.LogFormat(LogType.Warning, null, msg);
        }

        public static void LogWarning(string msg, UnityEngine.Object context)
        {
            Logger.LogFormat(LogType.Warning, context, msg);
        }

        public static void LogError(string msg)
        {
            Logger.LogFormat(LogType.Error, null, msg);
        }

        public static void LogError(string msg, UnityEngine.Object context)
        {
            Logger.LogFormat(LogType.Error, context, msg);
        }

        public static void LogVerbose(string msg, LogType logType = LogType.Log, UnityEngine.Object context = null)
        {
            VerboseLogger.LogFormat(logType, context, msg);
        }

        public static void LogException(Exception e)
        {
            Logger.LogException(e);
        }

        #endregion

        #region Property fields

        static GameObject _selectedAvatar;
        static string _toolFolderPath;
        static string _versionSuffix = null;

        #endregion
    }
}