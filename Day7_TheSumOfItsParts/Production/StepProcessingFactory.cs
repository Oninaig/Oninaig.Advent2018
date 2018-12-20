using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Day7_TheSumOfItsParts.Process;

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
            Debug.WriteLine("");
            hireWorkers();
        }

        public void FindSolution()
        {
            var runningEfficientTime = 0;
            var runningTime = 0;
            
            foreach (var kvp in WorkProcessingOrder.Packages)
            {
                var availableWorkers = MaxWorkers;
                var tasksToRun = kvp.Value.EligibleSteps.Where(x=> !x.IsCompleted).OrderBy(x=>x.RemainingWorkRequired).Take(MaxWorkers).OrderByDescending(x=>x.RemainingWorkRequired).ToList();
                

               


                //out of every task we are currently processing, find the fastest one and subtract its work time from all other working tasks
                var fastest = tasksToRun.LastOrDefault();
                //Get tasks that are assigned but not completed
                var priorityTasks = tasksToRun.Where(x => x.IsAssigned && !x.IsCompleted);
                if (priorityTasks.Any())
                {
                    var pTaskEnum = tasksToRun.GetEnumerator();
                    while (pTaskEnum.MoveNext() && availableWorkers > 0)
                    {
                        availableWorkers--;
                        WorkProcessingOrder.DoSetAmountOfWork(pTaskEnum.Current, fastest?.WorkRequired ?? int.MinValue);
                    }
                }

                if (availableWorkers == 0)
                    continue;
                if (availableWorkers < 0)
                    throw new Exception("Somehow we have negative available workers");

                foreach (var tsk in tasksToRun)
                {
                    WorkProcessingOrder.SetAssigned(tsk);
                    availableWorkers--;
                    WorkProcessingOrder.DoSetAmountOfWork(tsk, fastest?.WorkRequired ?? int.MinValue);
                    if (tsk.IsCompleted)
                        availableWorkers++;
                }
                runningTime += fastest?.WorkRequired ?? int.MinValue;
                WorkProcessingOrder.DumpPackages();

                ////first task is the slowest and dictates how long this level will take
                //var slowest = tasksToRun.FirstOrDefault();
                //var slowestTime = slowest?.WorkRequired ?? 0;

                //var fastest = tasksToRun.LastOrDefault();
                //var fastestTime = fastest?.WorkRequired ?? 0;

                //if (tasksToRun.Count() == 1)
                //{
                //    runningTime += slowest?.WorkRequired ?? 0;
                //    WorkProcessingOrder.SetAssigned(slowest);
                //    WorkProcessingOrder.SetCompleted(slowest);
                //    continue;
                //}

                ////Set our slowest task as assigned
                //WorkProcessingOrder.SetAssigned(slowest);

                //foreach (var tsk in tasksToRun)
                //{
                //    WorkProcessingOrder.DoSetAmountOfWork(tsk, fastestTime);
                //}



            }
            //foreach (var kvp in WorkProcessingOrder.Packages)
            //{
            //    //First we find the longest running task in our current package
            //    var sortedLargest = kvp.Value.EligibleSteps.OrderByDescending(x => x.WorkRequired);
            //    var longest = sortedLargest.FirstOrDefault(x=> !x.IsAssigned);
            //    if (longest != null)
            //    {
            //        //longest.IsAssigned = true;
            //        WorkProcessingOrder.SetAssigned(longest);
            //        if (runningEfficientTime == 0)
            //            runningEfficientTime = longest.WorkRequired;
            //        else if (runningEfficientTime > longest.WorkRequired)
            //        {
            //            runningEfficientTime -= longest.WorkRequired;
            //            runningTime += longest.WorkRequired;
            //        }
            //    }
            //    else
            //    {
            //        runningTime += runningEfficientTime;
            //        runningEfficientTime = 0;
            //        continue;
            //    }
                
                
            //    //Then, for each of our workers, we divy up the remaining work thats next-hardest
            //    //i < MaxWorkers - 1 becuase we already "assigned" the first (longest) running task in the line above, so we are already down 1 worker.
            //    //also we can only run this part if we have enough work remaining to go around in the first place
            //    if (kvp.Value.EligibleSteps.Count(x=>!x.IsAssigned) > 0 && (MaxWorkers - 1 > 0))
            //    {
            //        for (int i = 0; i < MaxWorkers - 1; i++)
            //        {
            //            var nextHardest = sortedLargest.Skip(i + 1).FirstOrDefault(x=> !x.IsAssigned);
            //            if (nextHardest == null)
            //                break;
            //            WorkProcessingOrder.SetAssigned(nextHardest);
            //            //nextHardest.IsAssigned = true;

            //            //Now we can fit the total time of the next hardest task into our current hardest task, being efficient with our time
            //            if (runningEfficientTime > nextHardest.WorkRequired)
            //            {
            //                runningEfficientTime -= nextHardest.WorkRequired;
            //                runningTime += nextHardest.WorkRequired;
            //            }
            //            else
            //            {
            //                runningTime += runningEfficientTime;
            //                runningEfficientTime = nextHardest.WorkRequired - runningEfficientTime;

            //            }
            //        }
            //    }
            //    else
            //    {
            //        runningTime += longest.WorkRequired;
            //        runningEfficientTime = 0;
            //    }

            //}

            Debug.WriteLine($"Solution is: {runningTime} seconds.");
        }

        private void initJobQueue()
        {
            //Same strategy as StepMap.PrintOrder
            var steps = ProcessingSteps.Map.Values.Where(x => !x.HasPrerequisites).OrderBy(x => x.StepName);
            if (!steps.Any())
                return;

            var eligibleSteps = steps.Select(x => new WorkingStep(x).Init());
            WorkProcessingOrder.AddWorkPackage(++workPackageCount,new List<WorkingStep>(eligibleSteps));
            //WorkQueue.Enqueue(new List<WorkingStep>(eligibleSteps));

            var nextStep = steps.First();
            if (nextStep.CanProcess)
                foreach (var dependent in ProcessingSteps.Map.Values.Where(x => x.DependsOn(nextStep))
                    .OrderBy(x => x.StepName))
                    dependent.RemovePrerequisite(nextStep);

            Console.Write($"{nextStep.StepName}");
            ProcessingSteps.Map.Remove(nextStep.StepName);
            initJobQueue();
        }
    }
}