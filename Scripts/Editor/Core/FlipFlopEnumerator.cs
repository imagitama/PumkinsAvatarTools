using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.Core
{
    public class FlipFlopEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerable<T> collection;
        private readonly IEnumerator<T> enumerator;
        public T Current => enumerator.Current;
        object IEnumerator.Current => enumerator.Current;
        public bool FlipState { get; private set; }

        bool first = true;

        public FlipFlopEnumerator(IEnumerable<T> enumerable)
        {
            collection = enumerable;
            enumerator = collection.GetEnumerator();
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            if(enumerator.MoveNext())
            {
                if(first)
                    first = false;
                else
                    FlipState = !FlipState;

                return true;
            }
            return false;
        }

        public void Reset()
        {
            enumerator.Reset();
        }
    }
}
