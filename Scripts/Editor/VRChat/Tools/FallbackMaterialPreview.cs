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

        MaterialCache cache = new MaterialCache();

        TrustRanks trustRank = TrustRanks.Friend;

        public FallbackMaterialPreview()
        {
            PumkinTools.OnAvatarSelectionChanged += OnAvatarSelectionChanged;
        }

        protected override bool DoAction(GameObject target)
        {
            //Get renderers and check if any material names are GUIDs
            var renders = target.GetComponentsInChildren<Renderer>(true);
            bool hasFallbacks = HasCachedMaterials(renders);

            //If they are, restore materials, if they aren't set fallbacks
            if(hasFallbacks)
                RevertFallbacks(renders);
            else
                SetFallbacks(renders);

            return true;
        }

        /// <summary>
        /// Assigns back the original materials to the renderers
        /// </summary>
        /// <param name="renders"></param>
        private void RevertFallbacks(Renderer[] renders)
        {
            foreach(var r in renders)
            {
                var serial = new SerializedObject(r);
                var materials = serial.FindProperty("m_Materials");

                for(int i = 0; i < r.sharedMaterials.Length; i++)
                {
                    var newMat = cache.GetOriginalMaterialFromCached(r.sharedMaterials[i]);
                    if(!newMat)
                    {
                        PumkinTools.LogWarning($"Can't find original material for material <b>slot {i}</b> on <b>{r.gameObject.name}</b>");
                        continue;
                    }
                    var mat = materials.GetArrayElementAtIndex(i);
                    mat.objectReferenceValue = newMat;
                }
                PumkinTools.Log($"Restored materials for <b>{r.gameObject.name}</b>");
                serial.ApplyModifiedProperties();
            }
        }

        /// <summary>
        /// Sets all materials on the renderers to copies using fallback shaders
        /// </summary>
        /// <param name="renders"></param>
        private void SetFallbacks(Renderer[] renders)
        {
            foreach(var r in renders)
            {
                var serial = new SerializedObject(r);
                var materials = serial.FindProperty("m_Materials");

                for(int i = 0; i < r.sharedMaterials.Length; i++)
                {
                    var oldMat = r.sharedMaterials[i];
                    if(AssetDatabaseHelpers.IsBuiltInAsset(oldMat))
                        continue;

                    var newMat = cache.GetCachedCopy(oldMat, out bool _);
                    SetFallbackShader(newMat, oldMat);

                    var mat = materials.GetArrayElementAtIndex(i);
                    mat.objectReferenceValue = newMat;
                }
                serial.ApplyModifiedProperties();
                PumkinTools.Log($"Set fallback materials for <b>{r.gameObject.name}</b>");
            }
        }

        /// <summary>
        /// Sets the shader of the material to a fallback variant
        /// </summary>
        /// <param name="referenceMaterial">Old material needed to decide what the new one should fallback to. If null <paramref name="material"/> is used</param>
        /// <param name="material">Material to set fallback shader for</param>
        void SetFallbackShader(Material material, Material referenceMaterial = null)
        {
            if(!material)
                return;
            else if(!referenceMaterial)
                referenceMaterial = material;

            string currShaderName = referenceMaterial.shader.name;
            var trustRankColor = TrustRankColors.GetColorForRank(trustRank);

            if(referenceMaterial.HasProperty("_MainTex"))
            {
                material.shader = shader_diffuse; //default to standard Diffuse

                if(currShaderName.Contains("Cutout") && !currShaderName.Contains("Toon"))
                    material.shader = shader_cutout_diffuse;

                if(!currShaderName.Contains("Cutout") && currShaderName.Contains("Toon"))
                    material.shader = shader_toon_opaque;

                if(currShaderName.Contains("Cutout") && currShaderName.Contains("Toon"))
                    material.shader = shader_toon_cutout;

                if(currShaderName.Contains("Transparent"))
                    material.shader = shader_transparent_diffuse;

                if(currShaderName.Contains("Unlit"))
                    material.shader = shader_unlit;
            }
            else
            {
                material.shader = Shader.Find("Fallback/Matcap");
                material.SetColor("_MatcapColorTintHolyMolyDontReadThis", trustRankColor);
            }
        }

        private void OnAvatarSelectionChanged(GameObject newSelection)
        {
            if(avatar)
            {
                var renders = avatar.GetComponentsInChildren<Renderer>(true);
                if(HasCachedMaterials(renders))
                    RevertFallbacks(renders);
            }
            avatar = newSelection;
        }

        /// <summary>
        /// Returns true if any material in renderers has a GUID as it's name
        /// </summary>
        /// <param name="renderers"></param>
        /// <returns></returns>
        static bool HasCachedMaterials(Renderer[] renderers)
        {
            return renderers
                .Select(r => GUID.TryParse(r.sharedMaterial.name, out GUID _))
                .Any(b => b);
        }

        public override void Dispose()
        {
            PumkinTools.OnAvatarSelectionChanged -= OnAvatarSelectionChanged;
            base.Dispose();
        }
    }
}
