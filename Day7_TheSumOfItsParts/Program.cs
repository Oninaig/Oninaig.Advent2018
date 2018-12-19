using System;
using System.Threading;
using System.Timers;
using Day7_TheSumOfItsParts.Process;
using Day7_TheSumOfItsParts.Production;

namespace Day7_TheSumOfItsParts
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var test = StepProcessor.GenerateStepMapFromFile("sampleinput.txt");
            ////test.PrintOrder();

            //var puzzle = StepProcessor.GenerateStepMapFromFile("puzzleinput.txt");
            ////todo change this so it doesnt modify the master step list
            ////puzzle.PrintOrder();

            //var testFactory = new StepProcessingFactory(test, 2);
            //testFactory.Init();
            //var testStation = new TimeClockStation();
            //var worker1 = new Worker(1, testStation);
            //var worker1Work = new WorkingStep(new WorkingStepParams(true, 3));
            //worker1.SetWork(worker1Work);

            //var worker2 = new Worker(2, testStation);
            //var worker2Work = new WorkingStep(new WorkingStepParams(true, 5));
            //worker2.SetWork(worker2Work);

            //testStation.Start();
            //worker1.ClockIn();
            //worker2.ClockIn();
            //Thread.Sleep(10000);
            var test = StepProcessor.GenerateStepMapFromFile("sampleinput.txt");
            //test.PrintOrder();

            //var puzzle = StepProcessor.GenerateStepMapFromFile("puzzleinput.txt");
            //todo change this so it doesnt modify the master step list
            //puzzle.PrintOrder();

            var testFactory = new StepProcessingFactory(test, 2);
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