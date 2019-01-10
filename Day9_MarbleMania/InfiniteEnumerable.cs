using System.Collections;
using System.Collections.Generic;

namespace Day9_MarbleMania
{
    public class InfiniteEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> enumerable;

        public InfiniteEnumerable(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            while (true)
                foreach (var ele in enumerable)
                    yield return ele;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}