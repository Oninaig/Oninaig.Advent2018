using System;
using System.Collections.Generic;
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

        public StepProcessingFactory(StepMap stepsToProcess, int maxWorkers)
        {
            this.ProcessingSteps = stepsToProcess;
            this.MaxWorkers = maxWorkers;
            this.Workers = new List<Worker>(maxWorkers);
        }

        public void StartWorkDay()
        {
            //Same strategy as StepMap.PrintOrder
            var steps = ProcessingSteps.Map.Values.Where(x=>!x.HasPrerequisites).OrderBy(x=>x.StepName);
            if (!steps.Any())
                return;

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
        }
    }
}
