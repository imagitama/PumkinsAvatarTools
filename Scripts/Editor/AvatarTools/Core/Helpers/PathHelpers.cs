using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Pumkin.Core.Helpers
{
    public static class PathHelpers
    {
        /// <summary>
        /// Returns path as a string array split by / or \
        /// </summary>
        public static string[] GetPathAsArray(string path)
        {
            if(string.IsNullOrEmpty(path))
                return null;

            return path.Split('\\', '/');
        }

        /// <summary>
        /// Get path without object name.
        /// </summary>
        /// <returns>Returns all text before last / or \. A paths ending like this (Armature/Hips/) will return Armature/ </returns>
        public static string GetPathNoName(string path)
        {
            if(string.IsNullOrWhiteSpace(path))
                return path;

            string reverse = new string(path.ToArray().Reverse().ToArray());
            char[] slashes = new char[] { '/', '\\' };
            int firstSlash = reverse.IndexOfAny(slashes);

            if(firstSlash == 0)
            {
                if(firstSlash + 1 < reverse.Length)
                    firstSlash = reverse.IndexOfAny(slashes, firstSlash + 1);
                else
                    return "";
            }

            if(firstSlash == -1)
                return "";


            reverse = reverse.Substring(firstSlash);
            string s = new string(reverse.ToArray().Reverse().ToArray());
            return s;
        }

        /// <summary>
        /// Checkes whether two paths are equal
        /// </summary>
        /// <param name="path"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool PathsAreEqual(string path, string other)
        {
            return string.Equals(NormalizePath(path), NormalizePath(other), StringComparison.OrdinalIgnoreCase);
        }

        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToLowerInvariant();
        }

        /// <summary>
        /// Changes path from full windows path to a local path in the Assets folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Path starting with Assets</returns>
        public static string AbsolutePathToLocalAssetsPath(string path)
        {
            if(path.StartsWith(Application.dataPath))
                path = "Assets" + path.Substring(Application.dataPath.Length);
            return path;
        }

        /// <summary>
        /// Changes a path in Assets to an absolute windows path
        /// </summary>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public static string LocalAssetsPathToAbsolutePath(string localPath)
        {
            localPath = NormalizePathSlashes(localPath);
            const string assets = "Assets/";
            if(localPath.StartsWith(assets))
            {
                localPath = localPath.Remove(0, assets.Length);
                localPath = $"{Application.dataPath}/{localPath}";
            }
            return localPath;
        }

        /// <summary>
        /// Replaces all forward slashes \ with back slashes /
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string NormalizePathSlashes(string path)
        {
            if(!string.IsNullOrEmpty(path))
                path = path.Replace('\\', '/');
            return path;
        }

        public static string FullTypeNameAsPath(string fullName)
        {
            if(string.IsNullOrWhiteSpace(fullName))
                return fullName;

            var arr = GetPathAsArray(fullName.Replace('.', '/'));
            return string.Join("/", arr);
        }

        public static string NamespaceAsPath(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                return name;

            var arr = GetPathAsArray(name.Replace('.', '/'));
            return string.Join("/", arr.Take(arr.Length - 1));
        }
    }
}