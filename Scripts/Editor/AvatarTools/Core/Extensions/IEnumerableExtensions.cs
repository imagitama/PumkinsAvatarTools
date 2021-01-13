using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static FlipFlopEnumerator<T> GetFlipFlopEnumerator<T>(this IEnumerable<T> collection)
            => new FlipFlopEnumerator<T>(collection);

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
            => collection != null && collection.Any();

    }
}
