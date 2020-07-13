using Pumkin.AvatarTools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools.SubTools
{
    [AllowAutoLoad]
    class ResetPose : SubToolBase
    {
        public ResetPose()
        {
            Name = "Reset Transforms";
            Description = "Resets the location, rotation and scale of your avatar";
            ParentModuleID = "main_tools";
        }

        public override bool Prepare(GameObject target)
        {
            if(!base.Prepare(target))
                return false;
            var prefType = PrefabUtility.GetPrefabAssetType(target);
            if(prefType == PrefabAssetType.MissingAsset || prefType == PrefabAssetType.NotAPrefab)
            {
                Debug.Log("Not a prefab");
                return false;
            }
            return true;
        }

        public override bool Execute(GameObject target)
        {
            if(!Prepare(target))
                return false;

            foreach(var t in target.GetComponentsInChildren<Transform>())
            {
                if(t == t.root) 
                    continue; //Skip root
                SerializedObject serialTrans = new SerializedObject(t);
                SerializedProperty rot = serialTrans.FindProperty("m_LocalRotation");
                SerializedProperty pos = serialTrans.FindProperty("m_LocalPosition");
                SerializedProperty scale = serialTrans.FindProperty("m_LocalScale");
                
                PrefabUtility.RevertPropertyOverride(rot, InteractionMode.AutomatedAction);
                PrefabUtility.RevertPropertyOverride(pos, InteractionMode.AutomatedAction);
                PrefabUtility.RevertPropertyOverride(scale, InteractionMode.AutomatedAction);
                serialTrans.ApplyModifiedPropertiesWithoutUndo();
            }
            return true;
        }
    }
}
