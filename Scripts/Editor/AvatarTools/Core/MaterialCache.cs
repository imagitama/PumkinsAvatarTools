using Pumkin.AvatarTools2;
using Pumkin.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.Core
{
    public class MaterialCache
    {
        const string DEFAULT_CACHE_PATH = "_materialcache";

        string CachePath
        {
            get
            {
                if(_cachePath == null)
                    _cachePath = DEFAULT_CACHE_PATH;
                return _cachePath;
            }

            set => _cachePath = value;
        }
        string _cachePath;

        //static List<string> _materialGUIDs = new List<string>();
        //static List<string> _fallbackGUIDs = new List<string>();

        Dictionary<Material, Material> cache = new Dictionary<Material, Material>();

        private MaterialCache() { }

        public MaterialCache(string cachePath = null, params Material[] materials)
        {
            this.CachePath = cachePath;
            UpdateCache(materials);
        }

        /// <summary>
        /// Gets or creates a material in the cache folder
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public Material GetCachedCopy(Material original, out bool wasCreated)
        {
            CachePath = DEFAULT_CACHE_PATH;
            wasCreated = false;
            if(original == null)
                return null;

            //Try to get from cache dictionary
            if(cache.TryGetValue(original, out Material cached))
                return cached;

            //Try to get from cache folder
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(original, out string guid, out long _);
            if(string.IsNullOrWhiteSpace(guid) || guid == Guid.Empty.ToString())
                return null;

            var loadedMat = AssetDatabase.LoadAssetAtPath<Material>($"Assets/{CachePath}/{guid}.mat");

            if(loadedMat)
                return loadedMat;

            //If missing create a new copy in the cache with the guid of our original material as the name
            var fullOriginalPath = AssetDatabase.GetAssetPath(original);
            if(string.IsNullOrWhiteSpace(fullOriginalPath))
                return null;

            var newFolderPath = $"Assets/{DEFAULT_CACHE_PATH}";
            var fullNewPath = $"{newFolderPath}/{guid}.mat";

            //Create cache folder
            if(!AssetDatabase.IsValidFolder(newFolderPath))
                AssetDatabase.CreateFolder("Assets", DEFAULT_CACHE_PATH);

            PumkinTools.LogVerbose($"Creating and caching fallback material: {fullOriginalPath} => {fullNewPath}");

            if(AssetDatabase.CopyAsset(fullOriginalPath, fullNewPath))
            {
                wasCreated = true;
                return AssetDatabase.LoadAssetAtPath<Material>(fullNewPath);
            }
            return null;
        }

        public void UpdateCache(Material[] materials)
        {
            if(materials.IsNullOrEmpty())
                return;

            cache.Clear();
            var cachedMatsArray = AssetDatabase.LoadAllAssetsAtPath(CachePath);

            foreach(var mat in materials)
            {
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(mat, out string guid, out long _);
                if(string.IsNullOrWhiteSpace(guid))
                    continue;
                var cached = cachedMatsArray.FirstOrDefault(m => m.name == guid) as Material;
                if(cached)
                    cache.Add(mat, cached);
            }
        }

        public Material GetOriginalFromCached(Material material)
        {
            if(!material)
                return null;

            if(!GUID.TryParse(material.name, out GUID guid))
                return null;

            var path = AssetDatabase.GUIDToAssetPath(guid.ToString());
            if(string.IsNullOrEmpty(path))
                return null;

            return AssetDatabase.LoadAssetAtPath<Material>(path);
        }

        //static void Serialize()
        //{
        //    _materialGUIDs = new List<string>();
        //    _fallbackGUIDs = new List<string>();

        //    foreach(var kv in cache)
        //    {
        //        if(kv.Key && kv.Value)
        //        {
        //            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(kv.Key, out string matGuid, out long _);
        //            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(kv.Key, out string fallGuid, out long _);

        //            _materialGUIDs.Add(matGuid);
        //            _fallbackGUIDs.Add(fallGuid);
        //        }
        //    }
        //}

        //static void Deserialize()
        //{
        //    cache = new Dictionary<Material, Material>();

        //    for(int i = 0; i < _materialGUIDs.Count; i++)
        //    {
        //        var matPath = AssetDatabase.GUIDToAssetPath(_materialGUIDs[i]);
        //        var fallPath = AssetDatabase.GUIDToAssetPath(_fallbackGUIDs[i]);

        //        if(string.IsNullOrEmpty(matPath) || string.IsNullOrEmpty(fallPath))
        //            continue;

        //        var mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        //        var fall = AssetDatabase.LoadAssetAtPath<Material>(fallPath);

        //        if(!mat || !fall)
        //            continue;

        //        cache[mat] = fall;
        //    }
        //}
    }
}
