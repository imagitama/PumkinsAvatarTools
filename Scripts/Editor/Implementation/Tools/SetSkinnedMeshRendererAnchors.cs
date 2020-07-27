using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Implementation.Settings;
using Pumkin.UnityTools.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Tools.SubTools
{
    [AutoLoad("tools_setRenderAnchors", "tools")]
    [UIDefinition("Set SkinnedMeshRenderer Anchors")]
    class SetSkinnedMeshRendererAnchors : SubToolBase
    {
        protected override SerializedObject SerializedSettings { get => serializedSettings; }
        
        SetSkinnedMeshRendererAnchor_settings settings;
        SerializedObject serializedSettings;

        public SetSkinnedMeshRendererAnchors() { }        

        protected override void SetupSettings()
        {
            settings = ScriptableObject.CreateInstance<SetSkinnedMeshRendererAnchor_settings>();            
            serializedSettings = new SerializedObject(settings);
        }

        protected override bool DoAction(GameObject target)
        {
            var renders = target.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            var anim = target.GetComponent<Animator>();                   

            Transform bone = null;
            string path = null;            

            if(settings.anchorType == SetSkinnedMeshRendererAnchor_settings.AnchorType.HumanBone)
            {
                if(!anim.isHuman)
                    Debug.LogError($"{target.name} isn't humanoid");
                bone = anim.GetBoneTransform(settings.humanBone);
                path = $"{settings.humanBone.GetType()}.{settings.humanBone.ToString()}";
            }
            else
            {
                path = settings.bonePath;
                if(!string.IsNullOrEmpty(path))
                    bone = target.transform.Find(path);
            }

            if(!bone)
            {
                Debug.LogError($"Couldn't find bone at '{path}'");
                return false;
            }                        
                
            var so = new SerializedObject(renders);
            var anchors = so.FindProperty("m_ProbeAnchor");
            if(anchors == null)
                return false;
                
            anchors.objectReferenceValue = bone;
            so.ApplyModifiedPropertiesWithoutUndo();            
            return true;
        }
    }
}
