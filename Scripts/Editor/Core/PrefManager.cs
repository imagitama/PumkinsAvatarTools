#if UNITY_EDITOR
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Core
{
    /// <summary>
    /// Gets and sets preferences on a per project basis
    /// </summary>
    public static class PrefManager
    {
        static readonly string prefString = $"Pumkin.AvatarTools-{Application.productName}";

        public static void SetBool(string name, bool value) =>
            EditorPrefs.SetBool($"{prefString}.{name}", value);

        public static void SetFloat(string name, float value) =>
            EditorPrefs.SetFloat($"{prefString}.{name}", value);

        public static void SetInt(string name, int value) =>
            EditorPrefs.SetInt($"{prefString}.{name}", value);

        public static void SetString(string name, string value) =>
            EditorPrefs.SetString($"{prefString}.{name}", value);

        public static bool GetBool(string name) =>
            EditorPrefs.GetBool($"{prefString}.{name}");

        public static bool GetBool(string name, bool defaultValue) =>
            EditorPrefs.GetBool(name, defaultValue);

        public static float GetFloat(string name) =>
            EditorPrefs.GetFloat($"{prefString}.{name}");

        public static float GetFloat(string name, float defaultValue) =>
            EditorPrefs.GetFloat(name, defaultValue);

        public static int GetInt(string name) =>
            EditorPrefs.GetInt($"{prefString}.{name}");

        public static int GetInt(string name, int defaultValue) =>
            EditorPrefs.GetInt(name, defaultValue);

        public static string GetString(string name) =>
            EditorPrefs.GetString($"{prefString}.{name}");

        public static string GetString(string name, string defaultValue) =>
            EditorPrefs.GetString($"{prefString}.{name}", defaultValue);
    }
}
#endif