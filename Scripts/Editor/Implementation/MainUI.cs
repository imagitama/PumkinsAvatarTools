using Pumkin.AvatarTools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.UI
{
    class MainUI
    {
        List<IUIModule> UIModules;

        public MainUI(List<IUIModule> modules)
        {
            UIModules = modules;   
        }

        public void Draw()
        {
            foreach(var mod in UIModules)
            {
                if(mod != null)
                {
                    mod.Draw();
                    EditorGUILayout.Space();
                }
                else
                {
                    Debug.Log($"{mod} is null");
                }
            }
        }

        public IUIModule FindModule(string name)
        {
            return UIModules.FirstOrDefault(s => string.Equals(name, s.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        public void AddModule(IUIModule module)
        {
            if(module != null)
                UIModules.Add(module);
        }

        public bool RemoveModule(IUIModule module)
        {
            return UIModules.Remove(module);
        }
        
        public int RemoveModule(string name)
        {
            return UIModules.RemoveAll(m => string.Equals(m.Name, name, StringComparison.InvariantCultureIgnoreCase));            
        }
    }
}
