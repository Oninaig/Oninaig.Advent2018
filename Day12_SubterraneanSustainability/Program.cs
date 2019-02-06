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
            var test = PotTools.InitPotCave("sampleinput.txt");
            var slower = test.ProcessGenerations(1000000);
            test = PotTools.InitPotCave("sampleinput.txt");
            var faster = test.ProcessGenerationsFast(1000000);

            var percentFaster = (((faster - slower) / (double) slower) * 100) * -1;
            Console.WriteLine($"Faster method is {percentFaster}% faster.");
            Console.ReadLine();
        }
    }
}
