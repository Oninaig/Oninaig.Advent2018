using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4_ReposeRecord
{
    public static class StringExtensions
    {
        public static string ExtractString(string input, char from, char to)
        {
            var startPos = input.IndexOf(from);
            var endPos = input.IndexOf(to, startPos);
            return input.Substring(startPos, endPos - startPos + 1);
        }
    }
}
