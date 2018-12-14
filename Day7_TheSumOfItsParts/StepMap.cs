using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day7_TheSumOfItsParts
{
    public class StepMap
    {
        public Dictionary<string, Step> Map { get; private set; }

        public StepMap()
        {
            this.Map = new Dictionary<string, Step>();
        }

        public void AddStep(string stepName, string preReqName)
        {
            Step preReqStep;
            Step currStep;

            //If we already have this step in our dictionary, add to it instead of creating a new step.
            if (Map.ContainsKey(stepName))
                currStep = Map[stepName];
            else
                currStep = new Step(stepName);

            if (Map.ContainsKey(preReqName))
                preReqStep = Map[preReqName];
            else
                preReqStep = new Step(preReqName);

            Map[preReqName] = preReqStep;
            Map[stepName] = currStep.AddPrerequisite(preReqStep);
        }

       

        

        public void PrintOrder()
        {
            var steps = Map.Values.Where(x=>!x.HasPrerequisites).OrderBy(x=>x.StepName);
            if (!steps.Any())
                return;

            var nextStep = steps.First();
            if (nextStep.CanProcess)
            {
                //If we can process, that means we can remove this step from its dependent steps
                foreach (var dependent in Map.Values.Where(x => x.DependsOn(nextStep)).OrderBy(x => x.StepName))
                {
                    dependent.RemovePrerequisite(nextStep);
                }
            }

            Console.Write($"{nextStep.StepName}");
            Map.Remove(nextStep.StepName);
            PrintOrder();
        }

    }
}
