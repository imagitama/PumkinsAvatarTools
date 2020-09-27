using System;
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
        {
            return new FlipFlopEnumerator<T>(collection);
        }
    }
}
