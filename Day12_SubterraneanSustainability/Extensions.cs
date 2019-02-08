using System;
using System.Collections.Generic;

namespace Day12_SubterraneanSustainability
{
    public static class Extensions
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            var result = new T[length];
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

        public static int SimplePotRangeHash(this IEnumerable<Pot> data)
        {
            var hash = 17;
            foreach (var pot in data)
                hash = hash * 23 + pot.GetNonUniqueHashCode();

            return hash;
        }
    }
}