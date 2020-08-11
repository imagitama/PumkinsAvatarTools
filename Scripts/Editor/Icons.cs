using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.UI
{
    static class Icons
    {
        public static GUIContent Settings { get; private set; }

        static Icons()
        {
            Settings = EditorGUIUtility.IconContent("Settings");            
        }
    }
}
