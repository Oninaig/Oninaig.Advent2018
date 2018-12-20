using System;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using Day7_TheSumOfItsParts.Process;
using Day7_TheSumOfItsParts.Process.Helpers;
using Day7_TheSumOfItsParts.Production;

namespace Day7_TheSumOfItsParts
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            StepProcessor.EnableWorkConstant = true;
            var puzzle = StepProcessor.GenerateStepMapFromFile("puzzleinput.txt");
            var puzzleFactory = new StepProcessingFactory(puzzle, 5);
            puzzleFactory.Init();
            puzzleFactory.FindSolution();

            //StepProcessor.EnableWorkConstant = false;
            //var test = StepProcessor.GenerateStepMapFromFile("sampleinput.txt");

            //var testFactory = new StepProcessingFactory(test, 2);
            //testFactory.Init();
            //testFactory.FindSolution();
            //testFactory.WorkProcessingOrder.DumpPackages();

            Console.ReadLine();
        }

        private static void TestStationOnTimeClockTimeStep(object sender, ElapsedEventArgs e)
        {
            Dumper.WriteLine("Hello from timeclock event handler!");
        }
    }
}