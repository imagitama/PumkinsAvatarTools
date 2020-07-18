using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Implementation.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.PumkinsAvatarTools.Scripts.Editor.Implementation.Tools
{
    [AutoLoad("test_update")]
    class UpdateTest : SubToolBase
    {        
        public UpdateTest()
        {
            Name = "Update Test";
            AllowUpdate = true;            
        }

        public override void DrawUI()
        {            
            AllowUpdate = EditorGUILayout.Toggle(Content, AllowUpdate);            
        }

        public override bool DoAction(GameObject target)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            base.Update();
            Debug.Log($"'{Name}' is Updating...");
        }
    }
}
