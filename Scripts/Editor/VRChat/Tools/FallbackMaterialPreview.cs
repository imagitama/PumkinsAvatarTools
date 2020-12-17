using Pumkin.AvatarTools2.Tools;
using Pumkin.Core;
using Pumkin.Core.Extensions;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Tools
{
    [AutoLoad("tools_fallbackShadersPreview", "VRChat", ParentModuleID = DefaultIDs.Modules.TestAvatar)]
    public class FallbackMaterialPreview : ToolBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Toggle Fallback Shader Preview");

        readonly Shader shader_diffuse = Shader.Find("Diffuse");
        readonly Shader shader_cutout_diffuse = Shader.Find("Transparent/Cutout/Diffuse");
        readonly Shader shader_toon_opaque = Shader.Find("Fallback/Toon/Opaque");
        readonly Shader shader_toon_cutout = Shader.Find("Fallback/Toon/Cutout");
        readonly Shader shader_transparent_diffuse = Shader.Find("Transparent/Diffuse");
        readonly Shader shader_unlit = Shader.Find("Fallback/Unlit");

        GameObject avatar = null;

        MaterialCache cache = new MaterialCache(".fallback_cache");

        TrustRanks trustRank = TrustRanks.Friend;

        public FallbackMaterialPreview()
        {
            PumkinTools.OnAvatarSelectionChanged += OnAvatarSelectionChanged;
        }

        protected override bool DoAction(GameObject target)
        {
            //Get renderers and check if any material names are guids
            var renders = target.GetComponentsInChildren<Renderer>(true);
            var hasFallbacks = renders
                .Select(r => GUID.TryParse(r.sharedMaterial.name, out GUID _))
                .Any(b => b);

            //If they are, restore materials, if they aren't set fallbacks
            if(hasFallbacks)
                RevertFallbacks(renders);
            else
                SetFallbacks(renders);

            return true;
        }

        private void RevertFallbacks(Renderer[] renders)
        {
            foreach(var r in renders)
            {
                var serial = new SerializedObject(r);
                var materials = serial.FindProperty("m_Materials");

                for(int i = 0; i < r.sharedMaterials.Length; i++)
                {
                    var newMat = cache.GetOriginalFromCached(r.sharedMaterials[i]);
                    if(!newMat)
                    {
                        PumkinTools.LogError($"Can't find original material for material in slot <b>{i}</b> on <b>{r.gameObject}</b>");
                        continue;
                    }
                    var mat = materials.GetArrayElementAtIndex(i);
                    mat.objectReferenceValue = newMat;
                }
                PumkinTools.Log($"Restored materials for <b>{r.gameObject.name}</b>");
                serial.ApplyModifiedProperties();
            }
        }

        private void SetFallbacks(Renderer[] renders)
        {
            foreach(var r in renders)
            {
                var serial = new SerializedObject(r);
                var materials = serial.FindProperty("m_Materials");

                for(int i = 0; i < r.sharedMaterials.Length; i++)
                {
                    var newMat = cache.GetCachedCopy(r.sharedMaterials[i], out bool wasCreated);

                    var mat = materials.GetArrayElementAtIndex(i);
                    mat.objectReferenceValue = newMat;
                    if(wasCreated)
                        SetFallbackShader(newMat);
                }
                serial.ApplyModifiedProperties();
                PumkinTools.Log($"Set fallback materials for <b>{r.gameObject.name}</b>");
            }

        }

        void SetFallbackShader(Material mat)
        {
            string currShaderName = mat.shader.name;
            var trustRankColor = TrustRankColors.GetColorForRank(trustRank);

            if(mat.HasProperty("_MainTex"))
            {
                mat.shader = shader_diffuse; //default to standard Diffuse

                if(currShaderName.Contains("Cutout") && !currShaderName.Contains("Toon"))
                    mat.shader = shader_cutout_diffuse;

                if(!currShaderName.Contains("Cutout") && currShaderName.Contains("Toon"))
                    mat.shader = shader_toon_opaque;

                if(currShaderName.Contains("Cutout") && currShaderName.Contains("Toon"))
                    mat.shader = shader_toon_cutout;

                if(currShaderName.Contains("Transparent"))
                    mat.shader = shader_transparent_diffuse;

                if(currShaderName.Contains("Unlit"))
                    mat.shader = shader_unlit;
            }
            else
            {
                mat.shader = Shader.Find("Fallback/Matcap");
                mat.SetColor("_MatcapColorTintHolyMolyDontReadThis", trustRankColor);
            }
        }

        private void OnAvatarSelectionChanged(GameObject newSelection)
        {
            if(avatar)
                RevertFallbacks(avatar.GetComponentsInChildren<Renderer>(true));
            avatar = newSelection;
        }
        public override void Dispose()
        {
            PumkinTools.OnAvatarSelectionChanged -= OnAvatarSelectionChanged;
            base.Dispose();
        }
    }
}
