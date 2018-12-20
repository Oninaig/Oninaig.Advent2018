using System;
using System.Diagnostics;

namespace Day7_TheSumOfItsParts.Process.Helpers
{
    public static class Dumper
    {
        public static void WriteLine(string output)
        {
            Debug.WriteLine(output);
            Console.WriteLine(output);
        }
    }
}