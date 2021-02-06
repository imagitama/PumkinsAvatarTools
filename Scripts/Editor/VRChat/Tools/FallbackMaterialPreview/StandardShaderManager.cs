using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.MaterialPreview
{
    internal static class StandardShaderManager
    {
        static ReflectionType DetermineWorkflow(Material mat)
        {
            if(mat.HasProperty("_SpecGlossMap") && mat.HasProperty("_SpecColor"))
                return ReflectionType.Specular;
            if(mat.HasProperty("_MetallicGlossMap") && mat.HasProperty("_Metallic"))
                return ReflectionType.Metallic;
            return ReflectionType.Dielectric;
        }

        public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
        {
            switch(blendMode)
            {
                case BlendMode.Opaque:
                    material.SetOverrideTag("RenderType", string.Empty);
                    material.SetInt("_SrcBlend", 1);
                    material.SetInt("_DstBlend", 0);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;
                    break;
                case BlendMode.Cutout:
                    material.SetOverrideTag("RenderType", "TransparentCutout");
                    material.SetInt("_SrcBlend", 1);
                    material.SetInt("_DstBlend", 0);
                    material.SetInt("_ZWrite", 1);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 2450;
                    break;
                case BlendMode.Fade:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", 5);
                    material.SetInt("_DstBlend", 10);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
                case BlendMode.Transparent:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", 1);
                    material.SetInt("_DstBlend", 10);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
            }
        }

        static AlphaType GetSmoothnessMapChannel(Material material)
        {
            int smoothnessChannel = (int)material.GetFloat("_SmoothnessTextureChannel");
            if(smoothnessChannel == 1)
                return AlphaType.AlbedoAlpha;
            return AlphaType.SpecularMetallicAlpha;
        }

        static void SetMaterialKeywords(Material material, StandardShaderManager.ReflectionType workflowType)
        {
            Texture texture = null;
            if(material.HasProperty("_BumpMap"))
                texture = material.GetTexture("_BumpMap");
            else if(material.HasProperty("_DetailNormalMap"))
                texture = material.GetTexture("_DetailNormalMap");

            if(texture != null)
                SetKeyword(material, "_NORMALMAP", texture);

            if(workflowType == ReflectionType.Specular)
                SetKeyword(material, "_SPECGLOSSMAP", material.GetTexture("_SpecGlossMap"));
            else if(workflowType == ReflectionType.Metallic)
                SetKeyword(material, "_METALLICGLOSSMAP", material.GetTexture("_MetallicGlossMap"));

            if(material.HasProperty("_ParallaxMap"))
                SetKeyword(material, "_PARALLAXMAP", material.GetTexture("_ParallaxMap"));

            texture = null;
            if(material.HasProperty("_DetailAlbedoMap"))
                texture = material.GetTexture("_DetailAlbedoMap");
            else if(material.HasProperty("_DetailNormalMap"))
                texture = material.GetTexture("_DetailNormalMap");
            if(texture != null)
                SetKeyword(material, "_DETAIL_MULX2", texture);

            bool state = (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == MaterialGlobalIlluminationFlags.None;

            SetKeyword(material, "_EMISSION", state);
            if(material.HasProperty("_SmoothnessTextureChannel"))
            {
                SetKeyword(material, "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A", GetSmoothnessMapChannel(material) == AlphaType.AlbedoAlpha);
            }
        }

        public static void MaterialChanged(Material material)
        {
            if(material.HasProperty("_Mode"))
                SetupMaterialWithBlendMode(material, (BlendMode)material.GetInt("_Mode"));
            SetMaterialKeywords(material, DetermineWorkflow(material));
        }

        static void SetKeyword(Material m, string keyword, bool state)
        {
            if(state)
                m.EnableKeyword(keyword);
            else
                m.DisableKeyword(keyword);

        }

        public enum ReflectionType
        {
            Specular,
            Metallic,
            Dielectric
        }

        public enum BlendMode
        {
            Opaque,
            Cutout,
            Fade,
            Transparent
        }

        public enum AlphaType
        {
            SpecularMetallicAlpha,
            AlbedoAlpha
        }
    }
}