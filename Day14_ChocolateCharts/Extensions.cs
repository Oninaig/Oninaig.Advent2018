using System;
using System.Collections.Generic;

namespace Day14_ChocolateCharts
{
    public static class Extensions
    {
        public static IEnumerable<int> AsNumbers(this string input)
        {
            foreach (var c in input) yield return int.Parse(c.ToString());
        }

        public static int Count(this int variable)
        {
            return variable == 0 ? 1 : (int) Math.Floor(Math.Log10(Math.Abs(variable))) + 1;
        }

        public static int DigitAt(this int number, int index)
        {
            while (number >= 10 * index)
                number /= 10;

            return number % 10;
        }

        public static int UnsafeParse(string s)
        {
            var value = 0;
            for (var i = 0; i < s.Length; i++) value = value * 10 + (s[i] - '0');

            return value;
        }

        public static int CharToInt(this char input)
        {
            var result = -1;
            if (input >= 48 && input <= 57)
                result = input - '0';
            return result;
        }
    }
}