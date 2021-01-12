using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.Settings;
using Pumkin.AvatarTools2.Tools;
using Pumkin.Core;
using Pumkin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Tools
{
    [AutoLoad("tools_quickSetupAvatar", "VRChat", ParentModuleID = DefaultIDs.Modules.Tools_SetupAvatar)]
    public class QuickSetupAvatar : ToolBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Quick Setup Avatar", -100);

        static PropertyInfo LegacyBlendShapeNormalsPropertyInfo
        {
            get
            {
                if(_legacyBlendShapeNormalsPropertyInfo != null)
                {
                    return _legacyBlendShapeNormalsPropertyInfo;
                }

                Type modelImporterType = typeof(ModelImporter);
                _legacyBlendShapeNormalsPropertyInfo = modelImporterType.GetProperty(
                    "legacyComputeAllNormalsFromSmoothingGroupsWhenMeshHasBlendShapes",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                );

                return _legacyBlendShapeNormalsPropertyInfo;
            }
        }

        static SetRendererAnchors anchorSetter = new SetRendererAnchors();

        protected override bool DoAction(GameObject target)
        {
            var settings = Settings as QuickSetupAvatar_Settings;

            if(settings.setImportSettings)
                SetImportSettings(target);

            if(settings.setAnchorsToSpine)
                anchorSetter.TryExecute(target);

            return true;
        }

        void SetImportSettings(GameObject target)
        {
            var skinnedMeshes = target.GetComponentsInChildren<SkinnedMeshRenderer>(true).Select(x => x.sharedMesh);
            var incorrectMeshes = GetMeshesWithIncorrectBlendShapeNormalsSetting(skinnedMeshes);

            if(incorrectMeshes.Count > 0)
                EnableLegacyBlendShapeNormals(incorrectMeshes);
        }

        // Blendshape normals
        static HashSet<Mesh> GetMeshesWithIncorrectBlendShapeNormalsSetting(IEnumerable<Mesh> avatarMeshes)
        {
            HashSet<Mesh> incorrectlyConfiguredMeshes = new HashSet<Mesh>();
            foreach(Mesh avatarMesh in avatarMeshes)
            {
                if(!AssetDatabase.Contains(avatarMesh))
                    continue;

                string meshAssetPath = AssetDatabase.GetAssetPath(avatarMesh);
                if(string.IsNullOrEmpty(meshAssetPath))
                    continue;

                ModelImporter avatarImporter = AssetImporter.GetAtPath(meshAssetPath) as ModelImporter;
                if(avatarImporter == null)
                    continue;

                if(avatarImporter.importBlendShapeNormals != ModelImporterNormals.Calculate)
                    continue;

                bool useLegacyBlendShapeNormals = (bool)LegacyBlendShapeNormalsPropertyInfo.GetValue(avatarImporter);
                if(useLegacyBlendShapeNormals)
                    continue;

                if(!incorrectlyConfiguredMeshes.Contains(avatarMesh))
                    incorrectlyConfiguredMeshes.Add(avatarMesh);
            }

            return incorrectlyConfiguredMeshes;
        }

        static void EnableLegacyBlendShapeNormals(IEnumerable<Mesh> meshesToFix)
        {
            HashSet<string> meshAssetPaths = new HashSet<string>();
            foreach(Mesh meshToFix in meshesToFix)
            {
                if(!AssetDatabase.Contains(meshToFix))
                    continue;

                string meshAssetPath = AssetDatabase.GetAssetPath(meshToFix);
                if(string.IsNullOrEmpty(meshAssetPath))
                    continue;

                if(meshAssetPaths.Contains(meshAssetPath))
                    continue;

                meshAssetPaths.Add(meshAssetPath);
            }

            foreach(string meshAssetPath in meshAssetPaths)
            {
                ModelImporter avatarImporter = AssetImporter.GetAtPath(meshAssetPath) as ModelImporter;
                if(avatarImporter == null)
                    continue;

                if(avatarImporter.importBlendShapeNormals != ModelImporterNormals.Calculate)
                    continue;

                LegacyBlendShapeNormalsPropertyInfo.SetValue(avatarImporter, true);
                avatarImporter.SaveAndReimport();
            }
        }


        private static PropertyInfo _legacyBlendShapeNormalsPropertyInfo;
    }
}
