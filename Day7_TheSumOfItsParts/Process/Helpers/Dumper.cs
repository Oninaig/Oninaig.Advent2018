using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
