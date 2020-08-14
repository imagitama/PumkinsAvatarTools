#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pumkin.UnityTools.Helpers
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
    }
}
#endif