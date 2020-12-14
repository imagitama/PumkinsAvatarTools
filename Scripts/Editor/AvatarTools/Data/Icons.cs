#if UNITY_EDITOR
using Pumkin.Core.Helpers;
using System;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.UI
{
    public static class Icons
    {
        const string RESOURCE_FOLDER_PREFIX = "Pumkin/Icons/";

        public static GUIContent Settings { get; private set; }
        public static GUIContent Options { get; private set; }
        public static GUIContent Default { get; private set; }
        public static GUIContent Add { get; private set; }
        public static GUIContent Remove { get; private set; }
        public static GUIContent RemoveAll { get; private set; }

        //Types
        public static GUIContent AvatarDescriptor { get; private set; }
        public static GUIContent Bone { get; private set; }


        static Icons()
        {
            Default = EditorGUIUtility.IconContent("DefaultAsset Icon");
            Settings = EditorGUIUtility.IconContent("Settings");
            Options = EditorGUIUtility.IconContent("LookDevPaneOption", "Options");

            Add = EditorGUIUtility.IconContent("Toolbar Plus");
            Remove = EditorGUIUtility.IconContent("Toolbar Minus");
            RemoveAll = EditorGUIUtility.IconContent("vcs_delete");

            Bone = new GUIContent(GetIconTexureFromReources("DynamicBone")) ?? Default;
            AvatarDescriptor = new GUIContent(GetTypeIconTextureFromResources("AvatarDescriptor")) ?? Default;
        }

        /// <summary>
        /// Loads icon texture from unity resources if type name matches texture name (ex: GameObject or GameObject-icon)
        /// </summary>
        /// <param name="typeNameFull">Must be full type name path here, but texture name only needs the type name</param>
        /// <returns></returns>
        public static Texture GetTypeIconTextureFromResources(string typeNameFull)
        {
            Type type = TypeHelpers.GetType(typeNameFull);
            Texture tex = null;
            if (type != null)
                tex = GetIconTexureFromReources(type.Name);
            else
                tex = GetIconTexureFromReources(typeNameFull);
            return tex;
        }

        /// <summary>
        /// Loads icon texture from unity resources if type name matches texture name (ex: GameObject or GameObject-icon)
        /// </summary>
        /// <returns></returns>
        public static Texture GetTypeIconTextureFromResources<T>()
        {
            return GetTypeIconTextureFromResources(typeof(T));
        }

        /// <summary>
        /// Loads icon texture from unity resources if type name matches texture name (ex: GameObject or GameObject-icon)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Texture GetTypeIconTextureFromResources(Type type)
        {
            return GetIconTexureFromReources(type.Name);
        }

        /// <summary>
        /// Gets icon texture from default unity icons or properties defined on this class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Texture GetIconTextureFromType<T>()
        {
            return GetIconTextureFromType(typeof(T));
        }

        /// <summary>
        /// Gets icon texture from default unity icons or properties defined on this class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Texture GetIconTextureFromType(Type type)
        {
            if (type == null)
                return null;

            Texture tex = null;

            //Check properties of this class for GUIContent with the same name as our type
            var prop = typeof(Icons).GetProperty(type.Name);
            if(prop != null && prop != default)
            {
                var cont = prop.GetValue(typeof(Icons)) as GUIContent;
                if(cont != null && cont.image != null)
                    tex = cont.image;
            }

            //If not found try getting it from resources
            if(!tex)
                tex = GetTypeIconTextureFromResources(type);

            if(tex)
                return tex;

            //If not found try getting it from default unity icons. Disable logger as it always throws errors when done at initialization
            return FuncHelpers.InvokeWithoutUnityLogger(() =>
            {
                return EditorGUIUtility.IconContent($"{type.Name} Icon")?.image ?? Default.image;
            });
        }

        static Texture GetIconTexureFromReources(string name)
        {
            var tx = Resources.Load<Texture>(RESOURCE_FOLDER_PREFIX + name + "-Icon");
            if(tx == null)
                tx = Resources.Load<Texture>(RESOURCE_FOLDER_PREFIX + name);
            return tx;
        }
    }
}
#endif