#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.UI
{
    static class Styles
    {
        const string RESOURCE_FOLDER_PREFIX = "Pumkin/UI/";

        public static GUIStyle MenuFoldout { get; private set; }
        public static GUIStyle SubMenuFoldout { get; private set; }
        public static GUIStyle TitleLabel { get; private set; }
        public static GUIStyle EditorLine { get; private set; }
        public static GUIStyle Box { get; private set; }
        public static GUIStyle IconButton { get; private set; }
        public static GUIStyle Icon { get; private set; }
        public static GUIStyle MediumIconButton { get; private set; }
        public static GUIStyle TextIconButton { get; private set; }
        public static GUIStyle MediumButton { get; private set; }
        public static GUIStyle BigButton { get; private set; }

        public static GUIStyle ModuleBox { get; private set; }
        public static GUIStyle CopierBox { get; private set; }
        public static GUIStyle SubToolButton { get; private set; }
        public static GUIStyle CopierCopyButton { get; private set; }
        public static GUIStyle RightAlignedLabel { get; private set; }

        public static float CopierTabHeight { get; private set; } = 22f;

        static Styles()
        {
            MenuFoldout = new GUIStyle("OffsetDropDown")
            {
                fontSize = 14,
                fixedHeight = 28,
                fontStyle = FontStyle.Normal,
                contentOffset = new Vector2(8, -1),
                padding = new RectOffset(4, 4, 4, 4),
                stretchWidth = true,
            };
            {
                //var module = Resources.Load<Texture2D>(RESOURCE_FOLDER_PREFIX +"module_pulldown");
                //var moduleAct = Resources.Load<Texture2D>(RESOURCE_FOLDER_PREFIX +"module_pulldown act");
                //var moduleFocus = Resources.Load<Texture2D>(RESOURCE_FOLDER_PREFIX + "module_pulldown focus");

                //if(module)
                //    MenuFoldout.normal.background = module;
                //if(moduleAct)
                //    MenuFoldout.active.background = moduleAct;
                //if(moduleFocus)
                //    MenuFoldout.focused.background = moduleFocus;
            }

            SubMenuFoldout = MenuFoldout;

            //SubMenuFoldout = new GUIStyle("OffsetDropDown")
            //{
            //    fontSize = 12,
            //    fixedHeight = 22,
            //    fontStyle = FontStyle.Normal,
            //    contentOffset = new Vector2(5f, 0),
            //};

            TitleLabel = new GUIStyle("label")
            {
                padding = new RectOffset(1, 1, 3, 3),
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

            //Box = new GUIStyle("box")
            //{
            //    margin = new RectOffset(3, 3, 3, 4),
            //    padding = new RectOffset(10, 10, 6, 6),
            //    border = new RectOffset(6, 6, 6, 6),
            //    fontSize = 12,
            //    alignment = TextAnchor.UpperLeft,
            //};

            Box = new GUIStyle("helpBox")
            {
                margin = new RectOffset(4, 4, 3, 3),
                padding = new RectOffset(6, 6, 6, 6),
                border = new RectOffset(6, 6, 6, 6),
                fontSize = 12,
                alignment = TextAnchor.UpperLeft,
                stretchWidth = true,
                stretchHeight = false,
            };

            ModuleBox = new GUIStyle("helpBox")
            {
                margin = new RectOffset(4, 4, 3, 3),
                padding = new RectOffset(6, 6, 6, 6),
                border = new RectOffset(6, 6, 6, 6),
                fontSize = 12,
                alignment = TextAnchor.UpperLeft,
                stretchWidth = true,
                stretchHeight = false,
            };

            CopierBox = new GUIStyle("helpBox")
            {
                margin = new RectOffset(3, 3, 3, 3),
                padding = new RectOffset(10, 10, 6, 3),
                border = new RectOffset(6, 6, 6, 6),
                fontSize = 12,
                alignment = TextAnchor.UpperLeft,
                stretchWidth = true,
            };

            IconButton = new GUIStyle("button")
            {
                fixedWidth = 18f,
                fixedHeight = 18f,
                imagePosition = ImagePosition.ImageOnly,
                padding = new RectOffset(0, 0, 0, 0),
            };

            Icon = new GUIStyle("label")
            {
                fixedWidth = 18f,
                fixedHeight = 18f,
                imagePosition = ImagePosition.ImageOnly,
                alignment = TextAnchor.LowerRight,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                border = new RectOffset(0, 0, 0, 0),
            };

            MediumButton = new GUIStyle("button")
            {
                fixedHeight = 22f,
            };

            MediumIconButton = new GUIStyle("button")
            {
                fixedWidth = MediumButton.fixedHeight,
                fixedHeight = MediumButton.fixedHeight,
                imagePosition = ImagePosition.ImageOnly,
                padding = new RectOffset(0, 0, 0, 0),
            };

            TextIconButton = new GUIStyle("button")
            {
                stretchHeight = false,
                fixedHeight = 20,
            };

            BigButton = new GUIStyle("button")
            {
                fixedHeight = 28f,
            };

            RightAlignedLabel = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.UpperRight,
                stretchWidth = false,
            };

            SubToolButton = MediumButton;
            CopierCopyButton = BigButton;
        }
    }
}
#endif