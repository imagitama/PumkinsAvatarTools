using System;
using UnityEngine;

namespace Pumkin.Core.Extensions
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            return go.GetComponent<T>() ?? go.AddComponent<T>();
        }

        public static Component GetOrAddComponent(this GameObject go, Type type)
        {
            return go.GetComponent(type) ?? go.AddComponent(type);
        }

        public static Component GetOrAddComponent(this GameObject go, string typeName)
        {
            var type = Pumkin.Core.Helpers.TypeHelpers.GetTypeAnywhere(typeName);
            if(type == null)
                return null;

            return go.GetOrAddComponent(type);
        }
    }
}