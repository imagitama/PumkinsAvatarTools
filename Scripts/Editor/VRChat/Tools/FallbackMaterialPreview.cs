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

        static class MaterialProperties
        {
            public const string Color = "_Color";
            public const string SpecColor = "_Color";
            public const string Emission = "_Color";
            public const string Shininess = "_Color";
            public const string Cutoff = "_Cutoff";
            public const string MainTexture = "_MainTex";
            public const string OutlineWidth = "_outline_width";
        }

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
                    //SetFallbackShader(newMat, oldMat);
                    SetFallbackShader_VRC(ref newMat);

                    var mat = materials.GetArrayElementAtIndex(i);
                    mat.objectReferenceValue = newMat;
                }
                serial.ApplyModifiedProperties();
                PumkinTools.Log($"Set fallback materials for <b>{r.gameObject.name}</b>");
            }
        }

        public static void SetFallbackShader_VRC(ref Material material)
        {
            if(material == null || material.shader == null)
                material = new Material(Shaders.Matcap);

            bool isUnlit = material.shader.name.Contains("Unlit");
            bool isVertexLit = material.shader.name.Contains("VertexLit");
            bool isToon = material.shader.name.Contains("Toon") || material.HasProperty("_Ramp");
            bool isTransparent = material.shader.name.Contains("Transparent") || material.IsKeywordEnabled("_ALPHABLEND_ON");
            bool isCutout = material.shader.name.Contains("Cutout") || material.IsKeywordEnabled("_ALPHATEST_ON");
            bool isFade = material.shader.name.Contains("Fade");
            bool isParticle = material.shader.name.Contains("Particle");
            bool isSprite = material.shader.name.Contains("Sprite");
            bool isMatcap = material.shader.name.Contains("MatCap");
            bool isToonLit = material.shader.name == "VRChat/Mobile/Toon Lit";
            bool hasOutline = material.IsKeywordEnabled("TINTED_OUTLINE") && material.HasProperty("_outline_width") && material.GetFloat(MaterialProperties.OutlineWidth) == 0f;

            if(isSprite)
                material.shader = Shaders.SpriteDefault;
            else if(isParticle)
                material.shader = Shaders.Particle;
            else if(isMatcap)
            {
                material.shader = Shaders.Matcap;
                material.SetTexture("_MatCap", Shaders.MatcapTexture);
            }
            else if(isToonLit)
                material.shader = Shaders.Toon;
            else if(isToon && !isTransparent && !isFade)
            {
                if(isVertexLit)
                {
                    material.shader = Shaders.VertexLit;
                    if(material.HasProperty(MaterialProperties.Color))
                    {
                        material.SetColor(MaterialProperties.SpecColor, Color.black);
                        material.SetColor(MaterialProperties.Emission, material.GetColor(MaterialProperties.Color) * 0.2f);
                        material.SetFloat(MaterialProperties.Shininess, 0f);
                    }
                }
                else if(isCutout)
                    material.shader = !hasOutline ? Shaders.ToonCutout : Shaders.ToonCutoutDouble;
                else
                    material.shader = !hasOutline ? Shaders.Toon : Shaders.ToonDouble;
            }
            else if(isToon || isUnlit)
            {
                if(!material.HasProperty(MaterialProperties.MainTexture))
                    material.shader = Shaders.UnlitColor;
                else if(isCutout)
                    material.shader = Shaders.UnlitTransparentCutout;
                else if(isTransparent || isFade)
                    material.shader = Shaders.UnlitTransparent;
                else
                    material.shader = Shaders.UnlitTexture;
            }
            else if(isVertexLit)
                material.shader = Shaders.VertexLit;
            else
            {
                material.shader = Shaders.Standard;
                //material = new Material(AssetManagement.FindFallbackShader("Standard"));
                //AssetManagement.SetupBlendMode(material);
                //AssetManagement.CopyMaterialProperty<float>("_Cutoff", material, material);
                //AssetManagement.CopyMaterialProperty<float>("_Glossiness", material, material);
                //AssetManagement.CopyMaterialProperty<float>("_Metallic", material, material);
                //AssetManagement.CopyMaterialProperty<Texture2D>("_BumpMap", material, material);
                //AssetManagement.CopyMaterialProperty<Color>("_EmissionColor", material, material);
                //AssetManagement.CopyMaterialProperty<Texture2D>("_EmissionMap", material, material);
            }
            if(material.HasProperty(MaterialProperties.MainTexture)
               && !material.HasProperty(MaterialProperties.MainTexture)
               && !material.HasProperty("_Diffuse") || !material.HasProperty("_Texture")
               && material.GetTexture(MaterialProperties.MainTexture) == null
               && !material.HasProperty("_Color"))
                material.shader = Shaders.Matcap;
        }

        /// <summary>
        /// Sets the shader of the material to a fallback variant
        /// </summary>
        /// <param name="referenceMaterial">Old material needed to decide what the new one should fallback to. If null <paramref name="material"/> is used</param>
        /// <param name="material">Material to set fallback shader for</param>
        //void SetFallbackShader(Material material, Material referenceMaterial = null)
        //{
        //    if(!material)
        //        return;
        //    else if(!referenceMaterial)
        //        referenceMaterial = material;

        //    string currShaderName = referenceMaterial.shader.name;
        //    var trustRankColor = TrustRankColors.GetColorForRank(trustRank);

        //    if(referenceMaterial.HasProperty("_MainTex"))
        //    {
        //        material.shader = shader_diffuse; //default to standard Diffuse

        //        if(currShaderName.Contains("Cutout") && !currShaderName.Contains("Toon"))
        //            material.shader = shader_cutout_diffuse;

        //        if(!currShaderName.Contains("Cutout") && currShaderName.Contains("Toon"))
        //            material.shader = shader_toon;

        //        if(currShaderName.Contains("Cutout") && currShaderName.Contains("Toon"))
        //            material.shader = shader_toon_cutout;

        //        if(currShaderName.Contains("Transparent"))
        //            material.shader = shader_transparent_diffuse;

        //        if(currShaderName.Contains("Unlit"))
        //            material.shader = shader_unlit_texture;
        //    }
        //    else
        //    {
        //        material.shader = Shader.Find("Fallback/Matcap");
        //        material.SetColor("_MatcapColorTintHolyMolyDontReadThis", trustRankColor);
        //    }
        //}

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

        static class Shaders
        {
            public static Shader SpriteDefault
                => _fbSpriteDefault = _fbSpriteDefault ?? Shader.Find("Sprites/Default");
            public static Shader Particle
                => _fbParticle = _fbParticle ?? Shader.Find("Particles/~Additive-Multiply");
            public static Shader Toon
                => _fbToon = _fbToon ?? Shader.Find("Fallback/Toon/Opaque");
            public static Shader ToonDouble
                => _fbToonDouble = _fbToonDouble ?? Shader.Find("Fallback/Toon/Opaque Double");
            public static Shader VertexLit
                => _fbVertexLit = _fbVertexLit ?? Shader.Find("Legacy Shaders/VertexLit");
            public static Shader ToonCutout
                => _fbToonCutout = _fbToonCutout ?? Shader.Find("Fallback/Toon/Cutout");
            public static Shader ToonCutoutDouble
                => _fbToonCutoutDouble = _fbToonCutoutDouble ?? Shader.Find("Fallback/Toon/Cutout Double");
            public static Shader UnlitColor
                => _fbUnlitColor = _fbUnlitColor ?? Shader.Find("Unlit/Color");
            public static Shader UnlitTransparentCutout
                => _fbUnlitTransparentCutout = _fbUnlitTransparentCutout ?? Shader.Find("Unlit/Transparent Cutout");
            public static Shader UnlitTexture
                => _fbUnlitTexture = _fbUnlitTexture ?? Shader.Find("Fallback/Unlit_Texture");
            public static Shader Matcap
                => _fbMatcap = _fbMatcap ?? Shader.Find("Fallback/Matcap");
            public static Shader Standard
                => _fbStandard = _fbStandard ?? Shader.Find("Standard");

            public static Shader UnlitTransparent
                => _fbUnlitTransparent = _fbUnlitTransparent ?? Shader.Find("Unlit/Transparent");

            public static Texture2D MatcapTexture
                => _matcapTexture = _matcapTexture ?? Resources.Load<Texture2D>("Pumkin/FallbackShaders/Matcap");

            static Texture2D _matcapTexture;
            static Shader _fbSpriteDefault;
            static Shader _fbMatcap;
            static Shader _fbStandard;
            static Shader _fbUnlitTexture;
            static Shader _fbUnlitTransparentCutout;
            static Shader _fbUnlitColor;
            static Shader _fbToonCutoutDouble;
            static Shader _fbToonCutout;
            static Shader _fbVertexLit;
            static Shader _fbToonDouble;
            static Shader _fbToon;
            static Shader _fbParticle;
            static Shader _fbUnlitTransparent;
        }
    }
}
