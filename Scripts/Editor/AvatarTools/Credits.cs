using Pumkin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Pumkin.AvatarTools2.UI
{
    static class Credits
    {
        static readonly List<CreditsEntry> credits = new List<CreditsEntry>()
        {
            new CreditsEntry("Xiexe", "Original fallback shaders"),
            new CreditsEntry("Dreadrith", "Reset pose to avatar"),
        };

        public static void DrawUI()
        {
            EditorGUILayout.LabelField("Thanks to the following people for their help!");
            UIHelpers.DrawInVerticalBox(() =>
            {
                foreach(var c in credits)
                    c.Draw();
            }, EditorStyles.helpBox);
        }

        class CreditsEntry
        {
            string Name, Note;

            public CreditsEntry(string name, string note)
            {
                Name = name;
                Note = note;
            }

            public void Draw() => EditorGUILayout.LabelField(this);

            public override string ToString() => $"{Name} - {Note}";

            public static implicit operator string(CreditsEntry c) => c.ToString();

        }
    }
}
