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
        public static GUIStyle MenuFoldout { get; set; }        

        static Styles()
        {
            //EditorStyles
            MenuFoldout = new GUIStyle("foldout")
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold                
            };                        
        }
    }
}
