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
    [AutoLoad("tools_setRenderAnchors", "tools_setupAvatar")]
    [UIDefinition("Set Renderer Anchors", Description = "Sets up anchors overrides on your renderers")]
    class SetSkinnedMeshRendererAnchors : SubToolBase
    {        
        public override SettingsContainer Settings { get => settings; }

        SetSkinnedMeshRendererAnchor_settings settings;        

        public SetSkinnedMeshRendererAnchors() { }        

        protected override void SetupSettings()
        {
            settings = ScriptableObject.CreateInstance<SetSkinnedMeshRendererAnchor_settings>();                        
        }

        protected override bool Prepare(GameObject target)
        {
            return (settings.setSkinnedMeshRenderers || settings.setMeshRenderers) &&
                base.Prepare(target);
        }

        protected override bool DoAction(GameObject target)
        {
            var anim = target.GetComponent<Animator>();
            Transform bone = null;
            string path = null;
            
            var renders = new List<Component>();
            if(settings.setMeshRenderers)
                renders.AddRange(target.GetComponentsInChildren<MeshRenderer>(true));
            if(settings.setSkinnedMeshRenderers)
                renders.AddRange(target.GetComponentsInChildren<SkinnedMeshRenderer>(true));            

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
                
            var so = new SerializedObject(renders.ToArray());
            var anchors = so.FindProperty("m_ProbeAnchor");
            if(anchors == null)
                return false;
                
            anchors.objectReferenceValue = bone;
            so.ApplyModifiedPropertiesWithoutUndo();            
            return true;
        }
    }
}
