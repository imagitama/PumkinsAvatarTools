#if UNITY_EDITOR
using Pumkin.Core.Helpers;
using System;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.UI
{
    static class Icons
    {
        public static GUIContent Settings { get; private set; }
        public static GUIContent Options { get; private set; }
        public static GUIContent Default { get; private set; }
        public static GUIContent DynamicBone { get; private set; }
        public static GUIContent DynamicBoneCollider { get; private set; }

        static Icons()
        {
            Default = EditorGUIUtility.IconContent("DefaultAsset Icon");
            Settings = EditorGUIUtility.IconContent("Settings");
            Options = EditorGUIUtility.IconContent("LookDevPaneOption", "Options");

            DynamicBone = new GUIContent(Resources.Load<Texture>("Icons/DynamicBone-Icon")) ?? Default;
            DynamicBoneCollider = new GUIContent(Resources.Load<Texture>("Icons/DynamicBoneCollider-Icon")) ?? Default;
        }

        /// <summary>
        /// Loads icon texture from unity resources if type name matches texture name (ex: GameObject or GameObject-icon)
        /// </summary>
        /// <param name="typeNameFull">Must be full type name path here, but texture name only needs the type name</param>
        /// <returns></returns>
        public static Texture GetTypeIconFromResources(string typeNameFull)
        {
            Type type = TypeHelpers.GetType(typeNameFull);
            var tex = Resources.Load<Texture>(type.Name + "-Icon");
            if(tex == null)
                tex = Resources.Load<Texture>(type.Name);
            return tex;
        }

        /// <summary>
        /// Loads icon texture from unity resources if type name matches texture name (ex: GameObject or GameObject-icon)
        /// </summary>
        /// <returns></returns>
        public static Texture GetTypeIconFromResources<T>()
        {
            var tex = Resources.Load<Texture>(typeof(T).Name + "-Icon");
            if(tex == null)
                tex = Resources.Load<Texture>(typeof(T).Name);
            return tex;
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
            var prop = typeof(Icons).GetProperty(type.Name);
            if(prop != null)
            {
                var cont = prop.GetValue(typeof(Icons)) as GUIContent;
                if(cont != null && cont.image != null)
                    return cont.image;
            }

            return FuncHelpers.InvokeWithoutUnityLogger(() =>
            {
                return EditorGUIUtility.IconContent($"{type.Name} Icon")?.image ?? Default.image;
            });
        }
    }
}
#endif