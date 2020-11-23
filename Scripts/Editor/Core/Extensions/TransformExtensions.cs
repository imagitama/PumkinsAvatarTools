#if UNITY_EDITOR
using Pumkin.Core.Helpers;
using UnityEditor;
using UnityEngine;

namespace Pumkin.Core.Extensions
{
    public static class TransformExtensions
    {
        public static string GetPathInHierarchy(this Transform transform, bool skipRootTransform = true)
        {
            if(!transform)
                return string.Empty;

            string path = string.Empty;
            if(transform != transform.root)
            {
                if(!skipRootTransform)
                    path = transform.transform.root.name + "/";
                path += (AnimationUtility.CalculateTransformPath(transform.transform, transform.transform.root));
            }
            else
            {
                if(!skipRootTransform)
                    path = transform.transform.root.name;
            }
            return path;
        }

        /// <summary>
        /// Finds or creates a child transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="transformPath">Child transform path</param>
        /// <param name="createIfMissing">Whether we're alowed to create</param>
        /// <param name="rootReference">Reference root transform to copy values from</param>
        /// <returns>Transform if found or created, or null if not allowed to create</returns>
        public static Transform FindOrCreate(this Transform transform, string transformPath, bool createIfMissing = false, Transform rootReference = null)
        {
            var t = transform.Find(transformPath);

            if(t == null && createIfMissing)
            {

                string[] arr = transformPath?.Split('\\', '/') ?? new string[0];
                if(arr.Length > 0)
                {
                    string path = "";
                    for(int i = 0; i < arr.Length; i++)
                    {
                        path += (arr[i] + '/');
                        var path2 = (arr[i] + (arr.Length > 1 ? "/" : ""));
                        var tNew = transform.Find(path);
                        var tNew2 = transform.Find(path2);

                        if(tNew == null)
                        {
                            string s = PathHelpers.GetPathNoName(path);
                            string ss = PathHelpers.GetPathNoName(path2);
                            var parent = transform.Find(s);
                            var parent2 = transform.Find(ss);

                            if(!parent)
                                return null;

                            tNew = new GameObject(arr[i]).transform;
                            tNew.parent = parent;

                            var trans = rootReference.Find(s + arr[i]);
                            if(trans)
                            {
                                tNew.localScale = Vector3.one;
                                tNew.localPosition = trans.localPosition;
                                tNew.localRotation = trans.localRotation;
                                tNew.localEulerAngles = trans.localEulerAngles;
                                tNew.localScale = trans.localScale;

                                tNew.gameObject.tag = trans.gameObject.tag;
                                tNew.gameObject.layer = trans.gameObject.layer;
                                tNew.gameObject.SetActive(trans.gameObject.activeSelf);

                            }
                            else
                            {
                                tNew.localPosition = Vector3.zero;
                                tNew.localRotation = Quaternion.identity;
                                tNew.localEulerAngles = Vector3.zero;
                                tNew.localScale = Vector3.one;
                            }
                            t = tNew;
                        }
                    }
                }
            }
            return t;
        }
    }
}
#endif