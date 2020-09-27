#if UNITY_EDITOR
using UnityEngine;

namespace Pumkin.Core.Extensions
{
    static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            return go.GetComponent<T>() ?? go.AddComponent<T>();
        }
    }
}
#endif