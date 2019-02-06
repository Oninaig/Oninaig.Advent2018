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
            bool profiling = true;
            if (profiling)
            {
                var test = PotTools.InitPotCave("sampleinput.txt");
                var faster = test.ProcessGenerationsFast(1000000);
            }
            else
            {
                var test = PotTools.InitPotCave("sampleinput.txt");
                var slower = test.ProcessGenerations(25000);
                test = PotTools.InitPotCave("sampleinput.txt");
                var faster = test.ProcessGenerationsFast(25000);

                var percentFaster = (((faster - slower) / (double)slower) * 100) * -1;
                Debug.WriteLine($"Faster method is {percentFaster.ToString("N2")}% faster.");
            }
            
            //Console.ReadLine();
        }
    }
}
