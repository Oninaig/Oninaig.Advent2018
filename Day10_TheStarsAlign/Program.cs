using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10_TheStarsAlign
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Stars("sample.txt");
            test.DumpStarSystem();
            test.TimeStep();
            Console.ReadLine();
        }
    }
}
