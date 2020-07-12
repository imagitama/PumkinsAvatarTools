using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.UI
{
    static class Styles
    {
        public static GUIStyle MenuDropdown { get; set; }
        public static GUIStyle Foldout { get; set; }

        static Styles()
        {
            //EditorStyles
            MenuDropdown = new GUIStyle("ShurikenDropdown");
            Foldout = EditorStyles.foldout;            
        }
    }
}
