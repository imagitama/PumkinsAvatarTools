using Pumkin.Core.Extensions;
using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.UI.Credits
{
    class CreditsList
    {
        string header;
        bool drawHeader;

        readonly Entry[] entries;

        public CreditsList(string headerText, params Entry[] entries)
        {
            header = headerText;

            if(!string.IsNullOrWhiteSpace(header))
                drawHeader = true;

            this.entries = entries;
        }

        public void Draw()
        {
            if(entries.IsNullOrEmpty())
                return;

            if(drawHeader)
                EditorGUILayout.LabelField(header, EditorStyles.boldLabel);

            UIHelpers.DrawInVerticalBox(() =>
            {
                foreach(var c in entries)
                    c.Draw();
            }, EditorStyles.helpBox);
        }
    }

    abstract class Entry
    {
        public abstract void Draw();
    }

    class NoteEntry : Entry
    {
        string Name, Note;

        public NoteEntry(string name, string note)
        {
            Name = name;
            Note = note;
        }

        public override void Draw() => EditorGUILayout.LabelField(this);

        public override string ToString() => $"{Name} - {Note}";

        public static implicit operator string(NoteEntry c) => c.ToString();

    }

    class URLEntry : Entry
    {
        string Name, URL;


        public URLEntry(string name, string url)
        {
            Name = name;
            URL = url;
        }

        public override void Draw()
        {
            if(GUILayout.Button(this, Styles.UrlLabel))
                Application.OpenURL(URL);
        }

        public override string ToString() => $"{Name}";

        public static implicit operator string(URLEntry c) => c.ToString();
    }
}
