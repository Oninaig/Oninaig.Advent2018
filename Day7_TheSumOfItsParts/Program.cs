using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day7_TheSumOfItsParts
{
    class Program
    {
        static void Main(string[] args)
        {
            //var test = StepProcessor.GenerateStepMapFromFile("sampleinput.txt");
            //test.PrintOrder();

            var puzzle = StepProcessor.GenerateStepMapFromFile("puzzleinput.txt");
            puzzle.PrintOrder();
            Console.ReadLine();
        }
    }
}
