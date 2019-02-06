using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12_SubterraneanSustainability
{
    class Program
    {
        static void Main(string[] args)
        {
            bool profiling = false;
            if (profiling)
            {
                var test = PotTools.InitPotCave("sampleinput.txt");
                var faster = test.ProcessGenerations(1000000);
            }
            else
            {
                
                var test = PotTools.InitPotCave("sampleinput.txt");
                var faster = test.ProcessGenerations(1000000);
            }
            
            //Console.ReadLine();
        }
    }
}
