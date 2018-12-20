using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Workers = new List<Worker>(maxWorkers);
            //WorkDict = new Dictionary<int, List<WorkingStep>>();
            WorkProcessingOrder = new WorkPackages();
            //WorkQueue = new Queue<List<WorkingStep>>();
            //Processing = new List<WorkingStep>();
            //Processed = new List<WorkingStep>();
            workPackageCount = 0;

        }

        public List<Worker> Workers { get; }
        public int MaxWorkers { get; set; }
        public StepMap ProcessingSteps { get; }

        //public Dictionary<int, List<WorkingStep>> WorkDict { get; }
        public WorkPackages WorkProcessingOrder { get; }
        public Queue<List<WorkingStep>> WorkQueue { get; }
        public List<WorkingStep> Processing { get; }

        public List<WorkingStep> Processed { get; }

        public void Work()
        {
        }

        private void hireWorkers()
        {
            //for (var i = 0; i < MaxWorkers; i++)
            //    Workers.Add(new Worker(i));
        }

        public void Init()
        {
            initJobQueue();
            Dumper.WriteLine("");
            hireWorkers();
        }

        //public void FindSolution()
        //{
        //    var availableSteps =
        //        WorkProcessingOrder.Packages.SelectMany(x => x.Value.EligibleSteps).Where(x=>!x.HasPrerequisites).Distinct();

        //}

        public void FindSolution()
        {
            var runningEfficientTime = 0;
            var runningTime = 0;
            Queue<WorkingStep> InProgress = new Queue<WorkingStep>();
            while (WorkProcessingOrder.HasWorkToDo)
            {
                var availableWorkers = MaxWorkers;

                //What are we going to work on during this time step
                //First lets grab any tasks we are still working on from our last time step
                var todaysWork = new List<WorkingStep>();
                while (InProgress.Any() && availableWorkers > 0)
                {
                    todaysWork.Add(InProgress.Dequeue());
                    availableWorkers--;
                }

                //Then we grab any tasks that are eligible to be processed during this time step.
                var eligibleSteps = WorkProcessingOrder.Packages.SelectMany(x => x.Value.EligibleSteps)
                    .Where(x => !x.HasPrerequisites && !x.IsCompleted).Distinct().Where(x=> !todaysWork.Contains(x)).OrderBy(x => x.WorkRequired);
                var fastestTask = eligibleSteps.FirstOrDefault();
                var fastestInProgressTime = todaysWork.OrderByDescending(x => x.RemainingWorkRequired).LastOrDefault()?.RemainingWorkRequired;
                var fastestTime = (fastestTask?.RemainingWorkRequired < fastestInProgressTime ? fastestTask?.RemainingWorkRequired : fastestInProgressTime) ?? fastestTask.RemainingWorkRequired;

                foreach (var eligibleStep in eligibleSteps)
                {
                    if (availableWorkers > 0)
                    {
                        todaysWork.Add(eligibleStep);
                        availableWorkers--;
                    }
                    else
                        break;
                }

                foreach (var tsk in todaysWork)
                {
                    WorkProcessingOrder.SetAssigned(tsk);
                    if (WorkProcessingOrder.DoSetAmountOfWork(tsk, fastestTime))
                        WorkProcessingOrder.SetCompleted(tsk);
                    else
                        InProgress.Enqueue(tsk);
                }



                //foreach (var tsk in eligibleSteps)
                //{

                //    WorkProcessingOrder.SetAssigned(tsk);
                //    availableWorkers--;
                //    if (WorkProcessingOrder.DoSetAmountOfWork(tsk, fastestTime))
                //    {
                //        availableWorkers++;
                //        WorkProcessingOrder.SetCompleted(tsk);
                //    }
                //    else
                //        InProgress.Enqueue(tsk);
                //}
                


                runningTime += fastestTime;
                WorkProcessingOrder.DumpPackages(false);


                ////out of every task we are currently processing, find the fastest one and subtract its work time from all other working tasks
                //var fastestTime = int.MaxValue;
                //var fastPrioTask = InProgress.Any()
                //    ? InProgress.ToList().OrderBy(x => x.RemainingWorkRequired).First() : null;

                //var fastestPriorityTime = InProgress.Any()
                //    ? InProgress.ToList().OrderBy(x => x.RemainingWorkRequired).First().RemainingWorkRequired
                //    : int.MaxValue;
                //var fastestTaskToRun = tasksToRun.LastOrDefault();

                ////if our priority task is faster, use that as baseline
                //if (fastestTaskToRun?.RemainingWorkRequired < fastestPriorityTime)
                //    fastestTime = fastestTaskToRun.RemainingWorkRequired;
                //else
                //    fastestTime = fastestPriorityTime;

                //if (fastestTime == int.MaxValue)
                //    continue;
                ////Process the in-progress tasks
                //var tempQueue = new Queue<WorkingStep>();
                //while (InProgress.Any())
                //{
                //    var inProgressTask = InProgress.Dequeue();
                //    if (WorkProcessingOrder.DoSetAmountOfWork(inProgressTask, fastestTime))
                //        availableWorkers++;
                //    else
                //        tempQueue.Enqueue(inProgressTask);
                //}

                //InProgress = tempQueue;




                //if (availableWorkers == 0)
                //{
                //    //no available workers means we are still processing tasks from the last package, so we redo this one
                //    continue;
                //};
                //if (availableWorkers < 0)
                //    throw new Exception("Somehow we have negative available workers");

                //if (!needToWait)
                //{
                //    foreach (var tsk in tasksToRun)
                //    {

                //        WorkProcessingOrder.SetAssigned(tsk);

                //        availableWorkers--;
                //        if (WorkProcessingOrder.DoSetAmountOfWork(tsk, fastestTime))
                //        {
                //            availableWorkers++;
                //        }
                //        else
                //            InProgress.Enqueue(tsk);
                //    }
                //}
            


                //runningTime += fastestTime;
                //WorkProcessingOrder.DumpPackages(false);
            }

            //for (int i = 0; i < WorkProcessingOrder.Packages.Count; i++)
            //{
            //    bool needToWait = false;
            //    var kvp = WorkProcessingOrder.Packages.ElementAt(i);
            //    Dumper.WriteLine($"Currently processing package: {kvp.Key}");
            //    if (i > 0 && kvp.Value.EligibleSteps.Intersect(WorkProcessingOrder.Packages.ElementAt(i - 1).Value.EligibleSteps).Any())
            //    {
            //        var prevPackage = WorkProcessingOrder.Packages.ElementAt(i - 1).Value.EligibleSteps;
            //        var sameElements = kvp.Value.EligibleSteps.Intersect(prevPackage);
            //        var nonRepeatedElements = prevPackage.Where(x => !kvp.Value.EligibleSteps.Contains(x));
            //        if (nonRepeatedElements.Any(x => !x.IsCompleted))
            //            needToWait = true;
            //    }

            //    var tasksToRun = kvp.Value.EligibleSteps.Where(x => !x.IsAssigned && !x.IsCompleted).OrderBy(x => x.RemainingWorkRequired).Take(MaxWorkers - InProgress.Count).OrderByDescending(x => x.RemainingWorkRequired).ToList();





            //    //out of every task we are currently processing, find the fastest one and subtract its work time from all other working tasks
            //    var fastestTime = int.MaxValue;
            //    var fastPrioTask = InProgress.Any()
            //        ? InProgress.ToList().OrderBy(x => x.RemainingWorkRequired).First() : null;

            //    var fastestPriorityTime = InProgress.Any()
            //        ? InProgress.ToList().OrderBy(x => x.RemainingWorkRequired).First().RemainingWorkRequired
            //        : int.MaxValue;
            //    var fastestTaskToRun = tasksToRun.LastOrDefault();

            //    //if our priority task is faster, use that as baseline
            //    if (fastestTaskToRun?.RemainingWorkRequired < fastestPriorityTime)
            //        fastestTime = fastestTaskToRun.RemainingWorkRequired;
            //    else
            //        fastestTime = fastestPriorityTime;

            //    if (fastestTime == int.MaxValue)
            //        continue;
            //    //Process the in-progress tasks
            //    var tempQueue = new Queue<WorkingStep>();
            //    while (InProgress.Any())
            //    {
            //        var inProgressTask = InProgress.Dequeue();
            //        if (WorkProcessingOrder.DoSetAmountOfWork(inProgressTask, fastestTime))
            //            availableWorkers++;
            //        else
            //            tempQueue.Enqueue(inProgressTask);
            //    }

            //    InProgress = tempQueue;




            //    if (availableWorkers == 0)
            //    {
            //        //no available workers means we are still processing tasks from the last package, so we redo this one
            //        i--;
            //        continue;
            //    };
            //    if (availableWorkers < 0)
            //        throw new Exception("Somehow we have negative available workers");

            //    if (!needToWait)
            //    {
            //        foreach (var tsk in tasksToRun)
            //        {

            //            WorkProcessingOrder.SetAssigned(tsk);

            //            availableWorkers--;
            //            if (WorkProcessingOrder.DoSetAmountOfWork(tsk, fastestTime))
            //            {
            //                availableWorkers++;
            //            }
            //            else
            //                InProgress.Enqueue(tsk);
            //        }
            //    }
            //    else
            //    {
            //        i--;
            //    }



            //    runningTime += fastestTime;
            //    WorkProcessingOrder.DumpPackages(false);
            //}
            //    //foreach (var kvp in WorkProcessingOrder.Packages)
            //    //{
            //    //    Dumper.WriteLine($"Currently processing package: {kvp.Key}");
            //    //    var availableWorkers = MaxWorkers;


            //    //    var tasksToRun = kvp.Value.EligibleSteps.Where(x=> !x.IsAssigned && !x.IsCompleted).OrderBy(x=>x.RemainingWorkRequired).Take(MaxWorkers - InProgress.Count).OrderByDescending(x=>x.RemainingWorkRequired).ToList();





            //    //    //out of every task we are currently processing, find the fastest one and subtract its work time from all other working tasks
            //    //    var fastestTime = int.MaxValue;
            //    //    var fastestPriorityTask = InProgress.Any()
            //    //        ? InProgress.ToList().OrderBy(x => x.RemainingWorkRequired).First().RemainingWorkRequired
            //    //        : int.MaxValue;
            //    //    var fastestTaskToRun = tasksToRun.LastOrDefault();

            //    //    //if our priority task is faster, use that as baseline
            //    //    if (fastestTaskToRun?.RemainingWorkRequired < fastestPriorityTask)
            //    //        fastestTime = fastestTaskToRun.RemainingWorkRequired;
            //    //    else
            //    //        fastestTime = fastestPriorityTask;
            //    //    if (fastestTime == int.MaxValue)
            //    //        continue;
            //    //    //Process the in-progress tasks
            //    //    var tempQueue = new Queue<WorkingStep>();
            //    //    while (InProgress.Any())
            //    //    {
            //    //        availableWorkers--;
            //    //        var inProgressTask = InProgress.Dequeue();
            //    //        if (!WorkProcessingOrder.DoSetAmountOfWork(inProgressTask, fastestTime))
            //    //            tempQueue.Enqueue(inProgressTask);
            //    //    }

            //    //    InProgress = tempQueue;

            //    //    if (availableWorkers == 0)
            //    //        continue;
            //    //    if (availableWorkers < 0)
            //    //        throw new Exception("Somehow we have negative available workers");

            //    //    foreach (var tsk in tasksToRun)
            //    //    {
            //    //        WorkProcessingOrder.SetAssigned(tsk);
            //    //        availableWorkers--;
            //    //        if (WorkProcessingOrder.DoSetAmountOfWork(tsk, fastestTime))
            //    //        {
            //    //            availableWorkers++;
            //    //        }
            //    //        else
            //    //            InProgress.Enqueue(tsk);
            //    //    }

            //    //    runningTime += fastestTime;
            //    //    WorkProcessingOrder.DumpPackages(false);





            //    //}


            Dumper.WriteLine($"Solution is: {runningTime} seconds.");
        }

        private void initJobQueue()
        {
            //Same strategy as StepMap.PrintOrder
            var steps = ProcessingSteps.Map.Values.Where(x => !x.HasUnmappedPrerequisites).OrderBy(x => x.StepName);
            if (!steps.Any())
                return;

            var eligibleSteps = steps.Select(x => new WorkingStep(x).Init());
            WorkProcessingOrder.AddWorkPackage(++workPackageCount,new List<WorkingStep>(eligibleSteps));
            //WorkQueue.Enqueue(new List<WorkingStep>(eligibleSteps));

            var nextStep = steps.First();
            if (nextStep.CanProcess)
                foreach (var dependent in ProcessingSteps.Map.Values.Where(x => x.DependsOn(nextStep))
                    .OrderBy(x => x.StepName))
                    dependent.MarkPrerequisiteAsMapped(nextStep);

            Console.Write($"{nextStep.StepName}");
            ProcessingSteps.Map.Remove(nextStep.StepName);
            initJobQueue();
        }
    }
}