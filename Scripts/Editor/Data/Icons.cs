#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.UI
{
    static class Icons
    {
        public static GUIContent Settings { get; private set; }

        static Icons()
        {
            Settings = EditorGUIUtility.IconContent("Settings");            
        }

        public static Texture GetIconFromType(Type type)
        {
            return null;
        }
    }
}
#endif