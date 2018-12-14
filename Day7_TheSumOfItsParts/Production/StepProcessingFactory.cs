using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day7_TheSumOfItsParts.Production
{

    /// <summary>
    /// This is not a factory in the software development sense, but rather a literal factory where steps are processed (think a real factory irl).
    /// </summary>
    public class StepProcessingFactory
    {
        public List<Worker> Workers { get; private set; }
        public int MaxWorkers { get; set; }
        public StepMap ProcessingSteps { get; private set; }

        public Dictionary<int, List<WorkingStep>> WorkDict { get;private set; }
        public Queue<List<WorkingStep>> WorkQueue { get; private set; }
        public List<WorkingStep> Processing { get; private set; }

        public List<WorkingStep> Processed { get; private set; }

        private int workPackageCount;
        public StepProcessingFactory(StepMap stepsToProcess, int maxWorkers)
        {
            this.ProcessingSteps = stepsToProcess;
            this.MaxWorkers = maxWorkers;
            this.Workers = new List<Worker>(maxWorkers);
            this.WorkDict = new Dictionary<int, List<WorkingStep>>();
            this.WorkQueue = new Queue<List<WorkingStep>>();
            this.Processing = new List<WorkingStep>();
            this.Processed = new List<WorkingStep>();
            this.workPackageCount = 0;
        }
        public void Work()
        {

        }
        
        private void hireWorkers()
        {
            for (int i = 0; i < MaxWorkers; i++)
                Workers.Add(new Worker(i));
        }
        public void Init()
        {
            initJobQueue();
            hireWorkers();
        }

        private void initJobQueue()
        {
            //Same strategy as StepMap.PrintOrder
            var steps = ProcessingSteps.Map.Values.Where(x=>!x.HasPrerequisites).OrderBy(x=>x.StepName);
            if (!steps.Any())
                return;

            WorkDict.Add(++workPackageCount, new List<WorkingStep>(steps.Select(x=> new WorkingStep(x).Init())));
            WorkQueue.Enqueue(new List<WorkingStep>(steps.Select(x=> new WorkingStep(x).Init())));

            var nextStep = steps.First();
            if (nextStep.CanProcess)
            {
                //If we can process, that means we can remove this step from its dependent steps
                foreach (var dependent in ProcessingSteps.Map.Values.Where(x => x.DependsOn(nextStep)).OrderBy(x => x.StepName))
                {
                    dependent.RemovePrerequisite(nextStep);
                }
            }

            Console.Write($"{nextStep.StepName}");
            ProcessingSteps.Map.Remove(nextStep.StepName);
            initJobQueue();
        }
    }
}
