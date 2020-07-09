using Pumkin.AvatarTools.Interfaces;
using System.Collections.Generic;
using UnityEditor;

namespace Pumkin.AvatarTools.Implementation.Tools
{
    class ToolsModule : IMainMenuModule
    {        
        public List<ISubTool> SubTools { get; protected set; }
        Dictionary<string, List<ISubTool>> ToolCache { get; set;}

        public ToolsModule(List<ISubTool> tools)
        {
            SubTools = tools;
            ToolCache = new Dictionary<string, List<ISubTool>>();

            foreach(var tool in SubTools)
            {
                string cat = tool.CategoryName?.ToLower() ?? "uncategorized";
                if(!ToolCache.ContainsKey(cat))
                    ToolCache.Add(cat, new List<ISubTool>());   //TODO: Create a class for handling categories

                ToolCache[cat].Add(tool);
            }
        }        

        public void Draw()
        {
            foreach(KeyValuePair<string, List<ISubTool>> kv in ToolCache)
            {
                EditorGUILayout.LabelField($"{kv.Key}:");
                foreach(ISubTool sub in kv.Value)
                    sub.DrawUI();
                EditorGUILayout.Space();
            }
        }
    }
}
