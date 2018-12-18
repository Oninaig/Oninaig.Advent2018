using System;
using Day7_TheSumOfItsParts.Process;
using Day7_TheSumOfItsParts.Production;

namespace Day7_TheSumOfItsParts
{
    internal class Program
    {
        private static void Main(string[] args)
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