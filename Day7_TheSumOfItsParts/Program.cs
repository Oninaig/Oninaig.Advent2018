using System;
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
            //var test = StepProcessor.GenerateStepMapFromFile("sampleinput.txt");
            //test.PrintOrder();

            var puzzle = StepProcessor.GenerateStepMapFromFile("sampleinput.txt");
            //todo change this so it doesnt modify the master step list
            //puzzle.PrintOrder();

            var testFactory = new StepProcessingFactory(puzzle, 2);
            testFactory.Init();
            testFactory.WorkProcessingOrder.DumpPackages();
            Console.ReadLine();
        }

        private static void TestStationOnTimeClockTimeStep(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Hello from timeclock event handler!");
        }
    }
}