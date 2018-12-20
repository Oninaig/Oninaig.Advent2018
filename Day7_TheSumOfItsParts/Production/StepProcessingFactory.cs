using System;
using System.Collections.Generic;
using System.Linq;
using Day7_TheSumOfItsParts.Process;
using Day7_TheSumOfItsParts.Process.Helpers;

namespace Day7_TheSumOfItsParts.Production
{
    /// <summary>
    ///     This is not a factory in the software development sense, but rather a literal factory where steps are processed
    ///     (think a real factory irl).
    /// </summary>
    public class StepProcessingFactory
    {
        private int workPackageCount;

        public StepProcessingFactory(StepMap stepsToProcess, int maxWorkers)
        {
            ProcessingSteps = stepsToProcess;
            MaxWorkers = maxWorkers;
            WorkProcessingOrder = new WorkProcessingOrder();
            workPackageCount = 0;
        }

        public int MaxWorkers { get; set; }
        public StepMap ProcessingSteps { get; }
        public WorkProcessingOrder WorkProcessingOrder { get; }

        public void Init()
        {
            initJobQueue();
            Dumper.WriteLine("");
        }


        public void FindSolution()
        {
            var runningTime = 0;
            var InProgress = new Queue<Step>();
            while (WorkProcessingOrder.HasWorkToDo)
            {
                var availableWorkers = MaxWorkers;

                //What are we going to work on during this time step
                //First lets grab any tasks we are still working on from our last time step
                var todaysWork = new List<Step>();
                while (InProgress.Any() && availableWorkers > 0)
                {
                    todaysWork.Add(InProgress.Dequeue());
                    availableWorkers--;
                }

                //Then we grab any tasks that are eligible to be processed during this time step that aren't already in our current work queue (todaysWork).
                var eligibleSteps = WorkProcessingOrder.Packages
                    .SelectMany(x => x.Value.EligibleSteps)
                    .Where(x => !x.HasPrerequisites && !x.IsCompleted)
                    .Distinct()
                    .Where(x => !todaysWork.Contains(x))
                    .OrderBy(x => x.WorkRequired);

                var fastestTask = eligibleSteps.FirstOrDefault();

                var fastestInProgressTime = todaysWork.OrderByDescending(x => x.RemainingWorkRequired).LastOrDefault()
                    ?.RemainingWorkRequired;

                //This line just says "use whatever the fastest remaining time is between in-progress and new tasks.
                var fastestTime =
                    (fastestTask?.RemainingWorkRequired < fastestInProgressTime
                        ? fastestTask?.RemainingWorkRequired
                        : fastestInProgressTime) ?? fastestTask.RemainingWorkRequired;

                foreach (var eligibleStep in eligibleSteps)
                    if (availableWorkers > 0)
                    {
                        todaysWork.Add(eligibleStep);
                        availableWorkers--;
                    }
                    else
                    {
                        break;
                    }

                foreach (var tsk in todaysWork)
                {
                    WorkProcessingOrder.FlagStepAsAssigned(tsk);
                    if (WorkProcessingOrder.DoSetAmountOfWork(tsk, fastestTime))
                        WorkProcessingOrder.FlagStepAsCompleted(tsk);
                    else
                        InProgress.Enqueue(tsk);
                }


                runningTime += fastestTime;
                WorkProcessingOrder.DumpPackages(false);
            }


            Dumper.WriteLine($"Solution is: {runningTime} seconds.");
        }

        private void initJobQueue()
        {
            //Same strategy as StepMap.PrintOrder
            var steps = ProcessingSteps.Map.Values.Where(x => !x.HasUnmappedPrerequisites).OrderBy(x => x.StepName);
            if (!steps.Any())
                return;

            var eligibleSteps = steps.Select(x => new Step(x).Init());
            WorkProcessingOrder.AddWorkPackage(++workPackageCount, new List<Step>(eligibleSteps));

            var nextStep = steps.First();
            if (nextStep.CanProcessMapping)
                foreach (var dependent in ProcessingSteps.Map.Values.Where(x => x.DoesHaveDependencyOn(nextStep))
                    .OrderBy(x => x.StepName))
                    dependent.MarkPrerequisiteAsMapped(nextStep);

            Console.Write($"{nextStep.StepName}");
            ProcessingSteps.Map.Remove(nextStep.StepName);
            initJobQueue();
        }
    }
}