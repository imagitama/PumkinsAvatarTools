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
using Pumkin.AvatarTools2.VRChat.MaterialPreview;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Tools
{
    [AutoLoad("tools_fallbackShadersPreview", "VRChat", ParentModuleID = DefaultIDs.Modules.TestAvatar)]
    public class FallbackMaterialPreview : ToolBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Toggle Fallback Shader Preview");

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
                    newMat = MaterialManager.CreateFallbackMaterial(newMat, Color.white);

                    var mat = materials.GetArrayElementAtIndex(i);
                    mat.objectReferenceValue = newMat;
                }

                serial.ApplyModifiedProperties();
                AssetDatabase.SaveAssets();
                PumkinTools.Log($"Set fallback materials for <b>{r.gameObject.name}</b>");
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
