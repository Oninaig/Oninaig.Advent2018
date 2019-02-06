using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12_SubterraneanSustainability
{
    public static class Extensions
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static List<T> GetRangeSafe<T>(this List<T> data, int index, int count)
        {
            if (index + count > data.Count)
            {
                var maxRemaining = data.Count - index;
                return data.GetRange(index, maxRemaining);
            }

            return data.GetRange(index, count);
        }
    }
}
