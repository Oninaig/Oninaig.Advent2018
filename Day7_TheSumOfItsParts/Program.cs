using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Day7_TheSumOfItsParts.Production;

namespace Day7_TheSumOfItsParts
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = StepProcessor.GenerateStepMapFromFile("sampleinput.txt");
            //test.PrintOrder();

            var puzzle = StepProcessor.GenerateStepMapFromFile("puzzleinput.txt");
            //todo change this so it doesnt modify the master step list
            //puzzle.PrintOrder();

            var testFactory = new StepProcessingFactory(test, 2);
            testFactory.Init();
            Console.ReadLine();
        }
    }
}
