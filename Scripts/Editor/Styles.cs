using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.UI
{
    static class Styles
    {
        public static GUIStyle MenuFoldout { get; private set; }
        public static GUIStyle TitleLabel { get; private set; }
        public static GUIStyle EditorLine { get; private set; }

        static Styles()
        {
            //EditorStyles
            MenuFoldout = new GUIStyle("foldout")
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold
            };

            TitleLabel = new GUIStyle("label")
            {
                fontSize = 16,
                stretchHeight = true,
                fixedHeight = 24,
            };

            EditorLine = new GUIStyle("box")
            {
                border = new RectOffset(1, 1, 1, 1),
                margin = new RectOffset(5, 5, 1, 1),
                padding = new RectOffset(1, 1, 1, 1),
            };
        }
    }
}
