using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.Core.Extensions
{
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Skips last element and returns collection. Named this to avoid conflict with later .NET versions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="col"></param>
        /// <returns></returns>
        public static ICollection<T> SkipLastElement<T>(this ICollection<T> col)
        {
            if(col.Count == 0)
                return col;

            var result = col.Take(col.Count - 1);
            return result.ToList() as ICollection<T>;
        }
    }
}
