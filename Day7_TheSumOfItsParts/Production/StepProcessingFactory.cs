using System;
using System.Collections.Generic;
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
            hireWorkers();
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