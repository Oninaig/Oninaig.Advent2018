using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12_SubterraneanSustainability
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = PotTools.InitPotCave("puzzleinput.txt");
            test.ProcessGenerations(20);
            Console.ReadLine();
        }
    }
}
