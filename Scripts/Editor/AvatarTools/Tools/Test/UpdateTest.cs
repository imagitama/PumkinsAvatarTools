#if UNITY_EDITOR
//using Pumkin.UnityTools.Attributes;
//using Pumkin.UnityTools.Implementation.Tools;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEditor;
//using UnityEngine;

//namespace Pumkin.AvatarTools.Debug
//{
//    [AutoLoad("test_update")]
//    class UpdateTest : SubToolBase
//    {
//        public UpdateTest()
//        {
//            Name = "Update Test";
//            AllowUpdate = false;
//        }

//        public override void DrawUI()
//        {
//            AllowUpdate = EditorGUILayout.Toggle(Content, AllowUpdate);
//        }

//        protected override bool DoAction(GameObject target)
//        {
//            Debug.Log("Nothing");
//            return false;
//        }

//        public override void Update()
//        {
//            base.Update();
//            Debug.Log($"'{Name}' is Updating...");
//        }
//    }
//}
#endif