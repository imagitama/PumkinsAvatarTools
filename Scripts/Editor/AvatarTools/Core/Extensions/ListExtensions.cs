using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.PumkinsAvatarTools.Scripts.Editor.Core.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Resizes the list and fills it with default values when expanding
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size">New size</param>
        public static void ResizeWithDefaults<T>(this List<T> list, int size)
        {
            if(size == list.Count)
                return;
            if(size <= 0)
            {
                list.Clear();
                return;
            }

            var toAdd = size - list.Count;

            if(toAdd <= 0)
                list.RemoveRange(size, toAdd * -1);
            else
                for(var i = 0; i < toAdd; i++)
                    list.Add(default(T));
        }
    }
}
